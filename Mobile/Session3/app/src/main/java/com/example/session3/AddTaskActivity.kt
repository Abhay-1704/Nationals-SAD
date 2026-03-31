package com.example.session3

import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.*
import androidx.appcompat.app.AppCompatActivity
import com.google.android.material.appbar.MaterialToolbar
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import java.util.Calendar

class AddTaskActivity : AppCompatActivity() {

    private var assets         = listOf<AssetModel>()
    private var taskNames      = listOf<TaskNameModel>()
    private var scheduleModels = listOf<ScheduleModelItem>()

    // IDs of assets the user added to the list
    private val selectedAssetIds = mutableListOf<Int>()
    private lateinit var assetListAdapter: AssetListAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.create_pm)

        // ── Back / Cancel buttons both just finish() ─────────────────
        findViewById<MaterialToolbar>(R.id.toolbar_back).setOnClickListener { finish() }
        findViewById<Button>(R.id.btn_cancel).setOnClickListener { finish() }

        // ── Selected-assets ListView ──────────────────────────────────
        assetListAdapter = AssetListAdapter(this, selectedAssetIds) { removedId ->
            selectedAssetIds.remove(removedId)
            assetListAdapter.notifyDataSetChanged()
        }
        findViewById<ListView>(R.id.list_selected_assets).adapter = assetListAdapter

        // ── "Add to list" ─────────────────────────────────────────────
        findViewById<Button>(R.id.btn_add_to_list).setOnClickListener {
            val pos = findViewById<Spinner>(R.id.spinner_asset).selectedItemPosition
            if (assets.isNotEmpty() && pos >= 0) {
                val asset = assets[pos]
                if (!selectedAssetIds.contains(asset.id)) {
                    selectedAssetIds.add(asset.id)
                    assetListAdapter.allAssets = assets
                    assetListAdapter.notifyDataSetChanged()
                } else {
                    Toast.makeText(this, "Asset already added", Toast.LENGTH_SHORT).show()
                }
            }
        }

        // ── Submit ────────────────────────────────────────────────────
        findViewById<Button>(R.id.btn_submit).setOnClickListener { submitTasks() }

        loadDropdowns()
    }

    // ─────────────────────────────────────────────────────────────────
    //  Load spinners from API
    // ─────────────────────────────────────────────────────────────────

    private fun loadDropdowns() {
        Thread {
            try {
                val a  = fetchAssets()
                val t  = fetchTaskNames()
                val sm = fetchScheduleModels()
                runOnUiThread {
                    assets         = a
                    taskNames      = t
                    scheduleModels = sm

                    val makeAdapter = { list: List<String> ->
                        ArrayAdapter(this, android.R.layout.simple_spinner_item, list).also {
                            it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                        }
                    }

                    findViewById<Spinner>(R.id.spinner_asset).adapter =
                        makeAdapter(assets.map { it.assetName })

                    findViewById<Spinner>(R.id.spinner_task_name).adapter =
                        makeAdapter(taskNames.map { it.taskName })

                    findViewById<Spinner>(R.id.spinner_schedule_model).adapter =
                        makeAdapter(scheduleModels.map { it.name })
                }
            } catch (e: Exception) {
                Log.e("DROPDOWN_ERR", e.message.toString())
            }
        }.start()
    }

    // ─────────────────────────────────────────────────────────────────
    //  Submit — builds request from current UI state and POSTs
    // ─────────────────────────────────────────────────────────────────

    private fun submitTasks() {
        if (selectedAssetIds.isEmpty()) {
            Toast.makeText(this, "Please add at least one asset", Toast.LENGTH_SHORT).show()
            return
        }

        val taskPos  = findViewById<Spinner>(R.id.spinner_task_name).selectedItemPosition
        val modelPos = findViewById<Spinner>(R.id.spinner_schedule_model).selectedItemPosition

        if (taskNames.isEmpty() || scheduleModels.isEmpty()) {
            Toast.makeText(this, "Dropdowns not loaded yet", Toast.LENGTH_SHORT).show()
            return
        }

        val selectedTask  = taskNames[taskPos]
        val selectedModel = scheduleModels[modelPos]

        // Read start / end dates from the DatePickers
        val startPicker = findViewById<DatePicker>(R.id.start_date_picker)
        val endPicker   = findViewById<DatePicker>(R.id.end_date_picker)

        val startDate = "%04d-%02d-%02d".format(
            startPicker.year, startPicker.month + 1, startPicker.dayOfMonth)
        val endDate   = "%04d-%02d-%02d".format(
            endPicker.year,   endPicker.month + 1,   endPicker.dayOfMonth)

        val isRunBased = selectedModel.scheduleTypeName
            .contains("Milage", ignoreCase = true)

        Thread {
            try {
                var allOk = true
                for (assetId in selectedAssetIds) {
                    val req = CreatePmTaskRequest(
                        assetId          = assetId,
                        taskId           = selectedTask.id,
                        pmScheduleTypeId = selectedModel.id,
                        startDate        = if (isRunBased) null else startDate,
                        endDate          = if (isRunBased) null else endDate,
                        // Run-based fields — not in the original layout so pass null;
                        // the API will handle them as null (single entry at startKm=0)
                        startKilometer   = null,
                        endKilometer     = null,
                        intervalKilometer = null
                    )

                    val body = Gson().toJson(req)
                        .toRequestBody("application/json".toMediaType())
                    val request = Request.Builder()
                        .url("http://10.0.2.2:5270/api/task")
                        .post(body)
                        .build()

                    val response = OkHttpClient().newCall(request).execute()
                    if (!response.isSuccessful) {
                        Log.e("SUBMIT", "Asset $assetId failed: ${response.code} ${response.body?.string()}")
                        allOk = false
                    }
                }

                runOnUiThread {
                    if (allOk) {
                        Toast.makeText(this, "Task(s) created!", Toast.LENGTH_SHORT).show()
                        finish()   // goes back to MainActivity; onResume() will refresh the list
                    } else {
                        Toast.makeText(this, "One or more tasks failed. Check logs.", Toast.LENGTH_LONG).show()
                    }
                }
            } catch (e: Exception) {
                Log.e("SUBMIT_ERR", e.message.toString())
                runOnUiThread {
                    Toast.makeText(this, "Error: ${e.message}", Toast.LENGTH_LONG).show()
                }
            }
        }.start()
    }

    // ─────────────────────────────────────────────────────────────────
    //  Network helpers
    // ─────────────────────────────────────────────────────────────────

    private fun fetchAssets(): List<AssetModel> {
        val resp = OkHttpClient().newCall(
            Request.Builder().url("http://10.0.2.2:5270/api/task/assets").build()
        ).execute()
        val type = object : TypeToken<List<AssetModel>>() {}.type
        return Gson().fromJson(resp.body?.string(), type)
    }

    private fun fetchTaskNames(): List<TaskNameModel> {
        val resp = OkHttpClient().newCall(
            Request.Builder().url("http://10.0.2.2:5270/api/task/tasks").build()
        ).execute()
        val type = object : TypeToken<List<TaskNameModel>>() {}.type
        return Gson().fromJson(resp.body?.string(), type)
    }

    private fun fetchScheduleModels(): List<ScheduleModelItem> {
        val resp = OkHttpClient().newCall(
            Request.Builder().url("http://10.0.2.2:5270/api/task/schedulemodels").build()
        ).execute()
        val type = object : TypeToken<List<ScheduleModelItem>>() {}.type
        return Gson().fromJson(resp.body?.string(), type)
    }
}