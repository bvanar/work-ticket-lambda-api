using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using well_project_api.Dto.Shared;
using well_project_api.Dto.Tasks;
using well_project_api.Services;

namespace well_project_api.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{jobId}")]
        public async Task<JsonResult> GetTasks(int jobId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByJobId(jobId);
                return Json(ApiResponseDto.SuccessResponse(tasks));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddTask(TaskDto task)
        {
            try
            {
                var newTask = await _taskService.AddTask(task);
                return Json(ApiResponseDto.SuccessResponse(newTask));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpDelete("{taskId}")]
        public async Task<JsonResult> DeleteTask(int taskId)
        {
            try
            {
                await _taskService.DeleteTask(taskId);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<JsonResult> UpdateTask(TaskDto task)
        {
            try
            {
                await _taskService.UpdateTask(task);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPatch]
        [Route("complete")]
        public async Task<JsonResult> CompleteTask(CompleteTaskDto task)
        {
            try
            {
                var completedTask = await _taskService.CompleteTask(task);
                return Json(ApiResponseDto.SuccessResponse(completedTask));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }
    }
}
