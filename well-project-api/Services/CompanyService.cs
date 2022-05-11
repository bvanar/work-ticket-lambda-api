using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using well_project_api.Dto.Company;
using well_project_api.Models;

namespace well_project_api.Services
{
    public class CompanyService
    {
        private readonly WellDbContext _db;
        private readonly IMapper _mapper;
        public CompanyService(WellDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> CompanyList(int userId)
        {
            var userCompanyIds = await _db.UserCompany.Where(u => u.UserId == userId).Select(x => x.CompanyId).ToListAsync();
            var companies = await _db.Company.Where(z => userCompanyIds.Contains(z.CompanyId)).ToListAsync();
            companies.Add(new Company
            {
                CompanyId = 0,
                CompanyName = "My Created Jobs"
            });

            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            var companyFromDb = await GetCompanyById(company.CompanyId);

            companyFromDb.CompanyName = company.CompanyName;

            _db.Update(companyFromDb);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCompany(int id)
        {
            var company = await GetCompanyById(id);
            company.IsDeleted = true;

            _db.Update(company);
            await _db.SaveChangesAsync();
        }

        public async Task<CompanyDto> AddCompany(CompanyDto company)
        {
            await ValidateCompanyName(company.CompanyName);
            var newCompany = new Company()
            {
                CompanyName = company.CompanyName
            };

            var newCompanyFromDb = (await _db.AddAsync(newCompany)).Entity;
            await _db.SaveChangesAsync();

            return _mapper.Map<CompanyDto>(newCompanyFromDb);
        }

        private async Task<Company> GetCompanyById(int id)
        {
            var companyFromDb = await _db.Company.Where(x => x.CompanyId == id).SingleOrDefaultAsync();            
            if (companyFromDb == null)
            {
                throw new Exception("Error: Company not found");
            }

            return companyFromDb;
        }

        private async Task ValidateCompanyName(string companyName)
        {
            var count = await _db.Company.Where(x => x.CompanyName == companyName).CountAsync();
            if (count > 0)
            {
                throw new Exception($"Company: {companyName} already exists. Company names must be unique");
            }
        }
    }
}
