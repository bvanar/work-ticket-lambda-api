using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using well_project_api.Dto.Company;
using well_project_api.Dto.Shared;
using well_project_api.Services;

namespace well_project_api.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;
        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]        
        public async Task<JsonResult> List()
        {
            try
            {
                var companies = await _companyService.CompanyList();
                return Json(ApiResponseDto.SuccessResponse(companies));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<JsonResult> Update(CompanyDto company)
        {
            try
            {
                await _companyService.UpdateCompany(company);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<JsonResult> Add(CompanyDto company)
        {
            try
            {
                var newCompany = await _companyService.AddCompany(company);
                return Json(ApiResponseDto.SuccessResponse(newCompany));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]        
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                await _companyService.DeleteCompany(id);
                return Json(ApiResponseDto.SuccessResponse(true));
            }
            catch (Exception ex)
            {
                return Json(ApiResponseDto.FailureResponse(ex.Message));
            }
        }
    }
}