using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using well_project_api.Dto.Job;
using well_project_api.Dto.Tasks;
using well_project_api.Models;

namespace well_project_api.Services
{
    public class JobService
    {
        private readonly WellDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        public JobService(WellDbContext db, IMapper mapper, UserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<JobDto>> GetJobsByCompanyId(int companyId)
        {
            var jobs = await _db.Job.Where(c => c.CompanyId == companyId && c.IsDeleted == false).ToListAsync();
            if (jobs.Count == 0)
            {
                throw new Exception($"No jobs were found for the selected company. Id: {companyId}");
            }

            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<JobDto> AddJob(JobDto job)
        {
            if (job.CompanyId != 0)
            {
                var companyCheck = await _db.Company.Where(x => x.CompanyId == job.CompanyId).SingleOrDefaultAsync();
                if (companyCheck == null)
                {
                    throw new Exception($"No company found for Id: {job.CompanyId}");
                }
            }            

            var newJob = (await _db.AddAsync(_mapper.Map<Job>(job))).Entity;
            await _db.SaveChangesAsync();

            await AssignUserToJob(new UserJobRequestDto { JobId = newJob.JobId, UserId = newJob.OwnerId });
            // create userXjobs record
            

            return _mapper.Map<JobDto>(newJob);
        }

        public async Task UpdateJob(JobDto job)
        {
            var jobFromDb = await GetJobFromDb(job.JobId);

            jobFromDb.JobName = job.JobName;
            _db.Update(jobFromDb);
            await _db.SaveChangesAsync();
        }

        public async Task<List<JobDto>> GetUserJobs(int userId, int companyId)
        {
            List<int> jobIds = await _db.UserJob.Where(u => u.UserId == userId).Select(z => z.JobId).ToListAsync();
            if (jobIds.Count == 0)
            {
                return new List<JobDto>();
            }

            var jobs = await _db.Job.Where(z => jobIds.Contains(z.JobId) && z.CompanyId == companyId && z.IsDeleted == false).ToListAsync();            
            var mappedJobs = _mapper.Map<List<JobDto>>(jobs);

            // get the tasks associated with these jobs
            foreach (var job in mappedJobs)
            {
                var tasks = await _db.JobTasks.Where(t => t.JobId == job.JobId).ToListAsync();
                job.Tasks = _mapper.Map<List<TaskDto>>(tasks);

                foreach(var task in job.Tasks)
                {
                    if (task.CompletedByUserId == null) continue;
                    var user = await _userService.GetUser((int)task.CompletedByUserId);
                    task.CompletedByUserName = user.UserName;                    
                }
            }

            return mappedJobs;
        }

        public async Task DeleteJob(int jobId)
        {
            var job = await GetJobFromDb(jobId);
            job.IsDeleted = true;            

            _db.Update(job);            

            var jobTasks = await _db.JobTasks.Where(j => j.JobId == jobId).ToListAsync();            

            // need to delete all tasks as well
            foreach (JobTasks task in jobTasks)
            {
                task.IsDeleted = true;
            }

            var userXjobs = await _db.UserJob.Where(j => j.JobId == jobId).ToListAsync();
            _db.RemoveRange(userXjobs);

            _db.UpdateRange(jobTasks);
            await _db.SaveChangesAsync();
        }

        private async Task<Job> GetJobFromDb(int jobId)
        {
            var jobFromDb = await _db.Job.Where(j => j.JobId == jobId).FirstOrDefaultAsync();
            if (jobFromDb == null)
            {
                throw new Exception($"Unable to update job. Job: {jobId} was not found.");
            }
            return jobFromDb;
        }

        public async Task AssignUserToJob(UserJobRequestDto userJob)
        {
            //await ValidateUserJob(userJob);

            var newUserJob = new UserJob();
            newUserJob.JobId = userJob.JobId;
            newUserJob.UserId = userJob.UserId;

            await _db.AddAsync(newUserJob);
            await _db.SaveChangesAsync();
        }

        public async Task AssignUsersToJob(List<UserJobRequestDto> userJob)
        {
            var jobId = userJob.Select(z => z.JobId).FirstOrDefault();
            var existingUserJobs = await _db.UserJob.Where(j => j.JobId == jobId).ToListAsync();
            var jobOwner = await _db.Job.Where(z => z.JobId == jobId).Select(u => u.OwnerId).SingleOrDefaultAsync();

            var removeEntries = existingUserJobs.Where(z => z.UserId != jobOwner);
            _db.RemoveRange(removeEntries);
            await _db.AddRangeAsync(_mapper.Map<List<UserJob>>(userJob));
            await _db.SaveChangesAsync();
        }

        public async Task RemoveUsersFromJob(UserJobRequestDto userJob)
        {            
            //await ValidateUserJob(userJob);

            var userJobFromDb = await _db.UserJob.Where(x => x.UserId == userJob.UserId && x.JobId == userJob.JobId).FirstOrDefaultAsync();
            if (userJobFromDb == null)
            {
                throw new Exception("Unable to delete Job Assignment. Job was not assigned to the user.");
            }

            _db.Remove(userJobFromDb);
            await _db.SaveChangesAsync();
        }

        //private async Task ValidateUserJob(UserJobRequestDto userJob)
        //{
        //    // need to make sure the job is tied to the same company as the user is
        //    var userCompanyId = await _db.User.Where(u => u.UserId == userJob.UserId).Select(x => x.CompanyId).SingleOrDefaultAsync();
        //    var jobCompanyId = await _db.Job.Where(j => j.JobId == userJob.JobId).Select(x => x.CompanyId).SingleOrDefaultAsync();

        //    if (userCompanyId != jobCompanyId)
        //    {
        //        throw new Exception($"The User (id: {userJob.UserId}) cannot be assigned to this job (id: {userJob.JobId}) since they do not reside in the same company");
        //    }
        //}
    }
}
