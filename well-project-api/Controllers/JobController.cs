using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using well_project_api.Dto.Job;
using well_project_api.Dto.Shared;
using well_project_api.Services;

namespace well_project_api.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class JobController : Controller
    {
        private readonly JobService _jobService;
        public JobController(JobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("{companyId}")]
        public async Task<JsonResult> List(int companyId)
        {
            try
            {
                var jobs = await _jobService.GetJobsByCompanyId(companyId);
                return Json(ApiResponseDto.SuccessResponse(jobs));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpGet]
        [Route("user/{userId}/company/{companyId}")]
        public async Task<JsonResult> GetUserJobs(int userId, int companyId)
        {
            try
            {
                var jobs = await _jobService.GetUserJobs(userId, companyId);
                return Json(ApiResponseDto.SuccessResponse(jobs));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddJob(JobDto job)
        {
            try
            {
                var newJob = await _jobService.AddJob(job);
                return Json(ApiResponseDto.SuccessResponse(newJob));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> DeleteJob(int id)
        {
            try
            {
                await _jobService.DeleteJob(id);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<JsonResult> UpdateJob(JobDto job)
        {
            try
            {
                await _jobService.UpdateJob(job);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        [Route("assign-user")]
        public async Task<JsonResult> AssignUser(List<UserJobRequestDto> userJobs)
        {
            try
            {
                await _jobService.AssignUsersToJob(userJobs);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpDelete]
        [Route("remove-user")]
        public async Task<JsonResult> RemoveUser(UserJobRequestDto userJob)
        {
            try
            {
                await _jobService.RemoveUsersFromJob(userJob);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }
    }
}
