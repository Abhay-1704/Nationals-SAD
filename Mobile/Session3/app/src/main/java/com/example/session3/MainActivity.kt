package com.example.session3

import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.EditText
import android.widget.ListView
import android.widget.Spinner
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.google.android.material.floatingactionbutton.FloatingActionButton
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date
import java.util.Locale
import kotlin.jvm.java

class MainActivity : AppCompatActivity() {
    private lateinit var listView: ListView
    private lateinit var adapter: PmTaskAdapter
    private val tasks = mutableListOf<PmTaskModel>()
    var selectedDate: Date = Date()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom)
            insets
        }

        listView = findViewById(R.id.task_list)
        adapter = PmTaskAdapter(this, tasks)
        listView.adapter = adapter

        val activeDateEdit = findViewById<EditText>(R.id.active_date)
        val sdf = java.text.SimpleDateFormat("yyyy-MM-dd", java.util.Locale.getDefault())
        val today = java.util.Date()

        activeDateEdit.setText(sdf.format(today))
        activeDateEdit.isFocusable = false
        activeDateEdit.isClickable = false

        activeDateEdit.setOnClickListener {

            val calendar = Calendar.getInstance()

            val datePicker = DatePickerDialog(
                this,
                { _, y, m, d ->

                    val selectedCalendar = Calendar.getInstance()
                    selectedCalendar.set(y, m, d)

                    val sdf = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())

                    val selectedDateString = sdf.format(selectedCalendar.time)
                    activeDateEdit.setText(selectedDateString)

                    selectedDate = selectedCalendar.time

                    filterAndDisplayTasks()

                },
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
            )

            datePicker.show()
        }

        Thread {
            try {
                val result = getTasks()
                val assets = getAsset()
                val taskname = getTaskNames()

                Log.i("TASK_RESULT", result.toString())
                Log.i("ASSET_RESULT", assets.toString())
                Log.i("TASKNAME_RESULT", taskname.toString())

                runOnUiThread {
                    tasks.clear()
                    tasks.addAll(result)
                    adapter.notifyDataSetChanged()

                    val names = assets.map{it.assetName}
                    val assetspinnerAdapter = ArrayAdapter(this,android.R.layout.simple_list_item_1,names)
                    findViewById<Spinner>(R.id.assets_list).adapter = assetspinnerAdapter

                    val tasknames = taskname.map{it.taskName}
                    val taskspinnerAdapter = ArrayAdapter(this,android.R.layout.simple_list_item_1,tasknames)
                    findViewById<Spinner>(R.id.select_task).adapter = taskspinnerAdapter
                }
            } catch (e: Exception) {
                Log.e("ERROR", e.message.toString())
            }
        }.start()

        val fab = findViewById<FloatingActionButton>(R.id.Add_item)

        fab.setOnClickListener {
            val intent = Intent(this, AddTaskActivity::class.java)
            startActivity(intent)
        }
    }

    fun getTasks(): List<PmTaskModel> {
        val client = OkHttpClient()
        val request = Request.Builder()
            .url("http://10.0.2.2:5270/api/Task")
            .addHeader("Content-Type", "application/json")
            .build()
        val response = client.newCall(request).execute()
        val jsonString = response.body?.string()
        val type = object : TypeToken<List<PmTaskModel>>() {}.type
        val tasks : List<PmTaskModel> = Gson().fromJson(jsonString,type)
        return tasks
    }

    fun getAsset(): List<AssetModel> {
        val client = OkHttpClient()
        val request = Request.Builder()
            .url("http://10.0.2.2:5270/api/task/assets")
            .build()
        val response = client.newCall(request).execute()
        val json = response.body?.string()
        val type = object : TypeToken<List<AssetModel>>() {}.type
        return Gson().fromJson(json, type)
    }

    fun getTaskNames(): List<TaskNameModel> {
        val client = OkHttpClient()
        val request = Request.Builder()
            .url("http://10.0.2.2:5270/api/task/tasks")
            .build()
        val response = client.newCall(request).execute()
        val json = response.body?.string()
        val type = object : TypeToken<List<TaskNameModel>>() {}.type
        return Gson().fromJson(json, type)
    }

    fun toggleDone(id: Int, isDone: Boolean) {
        Thread{
            try {
                val client = OkHttpClient()
                val body = """{ "taskDone": $isDone }"""
                    .toRequestBody("application/json".toMediaType())
                val request = Request.Builder()
                    .url("http://10.0.2.2:5270/api/task/$id/done")
                    .put(body)
                    .build()
                val response = client.newCall(request).execute()
                if (response.isSuccessful) {
                    Log.i("TOGGLE", "Success")
                } else {
                    Log.e("TOGGLE", "Failed: ${response.code}")
                }
            } catch (e: Exception) {
                Log.e("TOGGLE_ERROR", e.toString())
            }
        }.start()
    }

    fun filterAndDisplayTasks() {
        val sdf = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())
        val activeDate = sdf.parse(findViewById<EditText>(R.id.active_date).text.toString())

        val filtered = tasks.filter { task ->
            if (task.type == "By Milage") {
                true
            } else {
                val taskDate = sdf.parse(task.date)
                val diff = ((taskDate.time - activeDate.time) / (1000 * 60 * 60 * 24)).toInt()

                diff <= 4
            }
        }

        tasks.clear()
        tasks.addAll(filtered)
        adapter.notifyDataSetChanged()
    }
}