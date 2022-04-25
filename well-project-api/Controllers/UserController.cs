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
        public async Task<JsonResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Json(ApiResponseDto.SuccessResponse(users));
            } catch(Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<JsonResult> RegisterUser(UserDto user)
        {
            try
            {
                var newUser = await _userService.RegisterUser(user);
                return Json(ApiResponseDto.SuccessResponse(newUser));
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
    }
}
