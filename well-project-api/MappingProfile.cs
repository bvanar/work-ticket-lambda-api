using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using well_project_api.Dto.Company;
using well_project_api.Dto.Job;
using well_project_api.Dto.Tasks;
using well_project_api.Dto.User;
using well_project_api.Models;

namespace well_project_api
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<JobTasks, TaskDto>().ReverseMap();
        }
    }
}
