using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using well_project_api.Dto.Shared;
using well_project_api.Dto.User;
using well_project_api.Services;

namespace well_project_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }        
        
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<JsonResult> GetUsersByCompanyId(int companyId)
        {
            try
            {
                var users = await _userService.GetUsersByCompanyId(companyId);
                return Json(ApiResponseDto.SuccessResponse(users));
            } catch(Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpGet]
        [Route("job/{jobId}")]
        public async Task<JsonResult> GetUsersByJobId(int jobId)
        {
            try
            {
                var users = await _userService.GetUsersByJobId(jobId);
                return Json(ApiResponseDto.SuccessResponse(users));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPut]
        [Route("invite")]
        public async Task<JsonResult> InviteUser(InviteUserRequestDto invite)
        {
            try
            {
                await _userService.InviteUser(invite);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpGet]
        [Route("validate/{userName}")]
        public async Task<JsonResult> ValidateUsername(string userName)
        {
            try
            {
                await _userService.ValidateUsername(userName);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> Login(UserRequestDto request)
        {
            try
            {
                var user = await _userService.Login(request);
                return Json(ApiResponseDto.SuccessResponse(user));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<JsonResult> ResetPassword(UserRequestDto request)
        {
            try
            {
                await _userService.ResetPassword(request);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPatch]        
        public async Task<JsonResult> UpdateUser(UserDto user)
        {
            try
            {
                var updatedUser = await _userService.UpdateUser(user);
                return Json(ApiResponseDto.SuccessResponse(updatedUser));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }
        
        [HttpPut]
        public async Task<JsonResult> RegisterUser(RegisterUserDto registerUser)
        {
            try
            {
                await _userService.RegisterUser(registerUser);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }
    }
}
