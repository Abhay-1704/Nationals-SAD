package com.example.session3

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.CheckBox
import android.widget.TextView

class PmTaskAdapter (private val context: Context, private val tasks : MutableList<PmTaskModel>): BaseAdapter() {
    override fun getCount(): Int {
        return tasks.size
    }

    override fun getItem(p0: Int): Any? {
        return tasks[p0]
    }

    override fun getItemId(p0: Int): Long {
        return tasks[p0].id.toLong()
    }

    override fun getView(
        p0: Int,
        p1: View?,
        p2: ViewGroup?
    ): View? {
        val view = p1 ?: LayoutInflater.from(context).inflate(R.layout.list_item, p2, false)

        val task = tasks[p0]

        val txtAsset = view.findViewById<TextView>(R.id.assets_name)
        val txtTaskName = view.findViewById<TextView>(R.id.task_name)
        val txtSchedule = view.findViewById<TextView>(R.id.schedule)
        val checkbox = view.findViewById<CheckBox>(R.id.is_favorite)

        txtAsset.text = "${task.assetName} SN:${task.assetSN}"
        txtTaskName.text = task.task
        txtSchedule.text = if (task.type == "By Milage") {
            "Mileage - at ${task.mileage} kilometer"
        } else {
            "${task.model} - at ${task.date}"
        }

        checkbox.isChecked = task.done

        checkbox.setOnClickListener {
            (context as MainActivity).toggleDone(task.id, !task.done)
        }

        return view
    }
}