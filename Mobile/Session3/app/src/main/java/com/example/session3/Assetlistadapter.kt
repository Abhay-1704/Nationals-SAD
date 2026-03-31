package com.example.session3

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.ImageView
import android.widget.TextView

class AssetListAdapter(
    private val context: Context,
    private val assetIds: MutableList<Int>,
    private val onRemove: (Int) -> Unit
) : BaseAdapter() {

    var allAssets: List<AssetModel> = emptyList()

    override fun getCount() = assetIds.size
    override fun getItem(pos: Int): Any = assetIds[pos]
    override fun getItemId(pos: Int): Long = assetIds[pos].toLong()

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val view = convertView
            ?: LayoutInflater.from(context).inflate(R.layout.item_list, parent, false)

        val id   = assetIds[position]
        val name = allAssets.find { it.id == id }?.assetName ?: "Asset #$id"

        view.findViewById<TextView>(R.id.txt_asset_item).text = name
        view.findViewById<ImageView>(R.id.img_delete).setOnClickListener { onRemove(id) }

        return view
    }
}