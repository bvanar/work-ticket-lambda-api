using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Dto.Shared
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public object Data { get; set; } = null;
        public string Message { get; set; } = "";

        static public ApiResponseDto SuccessResponse(object data, string message = "")
        {
            return new ApiResponseDto()
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        static public ApiResponseDto FailureResponse(object data, string message = "")
        {
            return new ApiResponseDto()
            {
                Success = false,
                Data = data,
                Message = message
            };
        }

        static public ApiResponseDto FailureResponse(string message = "")
        {
            return new ApiResponseDto()
            {
                Success = false,
                Message = message
            };
        }
    }
}
