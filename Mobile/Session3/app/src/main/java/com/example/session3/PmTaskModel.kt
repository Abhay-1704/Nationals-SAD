package com.example.session3

data class PmTaskModel (
    val id : Int,
    val assetName : String,
    val assetSN : String,
    val task : String,
    val type : String,
    val model : String,
    val date : String?,
    val mileage : Long?,
    val done : Boolean
)

data class AssetModel (
    val id : Int,
    val assetName : String
)

data class TaskNameModel (
    val id : Int,
    val taskName : String
)

// New: returned by GET /api/task/schedulemodels
data class ScheduleModelItem(
    val id: Int,
    val name: String,
    val scheduleTypeName: String
)

// New: body sent to POST /api/task
data class CreatePmTaskRequest(
    val assetId: Int,
    val taskId: Int,
    val pmScheduleTypeId: Int,
    val startDate: String?,
    val endDate: String?,
    val startKilometer: Long?,
    val endKilometer: Long?,
    val intervalKilometer: Long?
)