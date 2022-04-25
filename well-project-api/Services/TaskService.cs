using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using well_project_api.Dto.Tasks;
using well_project_api.Models;

namespace well_project_api.Services
{
    public class TaskService
    {
        private readonly WellDbContext _db;
        private readonly IMapper _mapper;
        public TaskService(WellDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<TaskDto>> GetTasksByJobId(int jobId)
        {
            var tasks = await _db.JobTasks.Where(x => x.JobId == jobId && x.IsDeleted == false).ToListAsync();

            if (tasks.Count == 0)
            {
                throw new Exception($"The selected job (id: {jobId}) has no tasks associatied.");
            }

            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto> AddTask(TaskDto task)
        {
            // validate the selected job exists
            var jobVerify = await _db.Job.Where(j => j.JobId == task.JobId).FirstOrDefaultAsync();

            if (jobVerify == null)
            {
                throw new Exception("Error Adding Task: Job does not exist");
            }

            var mappedTask = _mapper.Map<JobTasks>(task);
            mappedTask.TaskId = 0;

            var newTask = (await _db.AddAsync(mappedTask)).Entity;
            await _db.SaveChangesAsync();

            return _mapper.Map<TaskDto>(newTask);
        }

        public async Task DeleteTask(int taskId)
        {
            var task = await GetTask(taskId);

            task.IsDeleted = true;
            _db.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTask(TaskDto task)
        {
            var taskFromDb = await GetTask(task.TaskId);
            taskFromDb.TaskName = task.TaskName;
            taskFromDb.TaskOrder = task.TaskOrder;

            _db.Update(taskFromDb);
            await _db.SaveChangesAsync();
        }

        public async Task<TaskDto> CompleteTask(int taskId, bool isComplete)
        {
            var task = await GetTask(taskId);
            task.Completed = isComplete;            

            if (isComplete == true)
            {
                task.CompletedDate = DateTime.UtcNow;
            }
            else
            {
                task.CompletedDate = null;
            }

            var completedTask = (_db.Update(task)).Entity;
            await _db.SaveChangesAsync();

            return _mapper.Map<TaskDto>(completedTask);
        }

        private async Task<JobTasks> GetTask(int taskId)
        {
            var task = await _db.JobTasks.Where(z => z.TaskId == taskId).FirstOrDefaultAsync();

            if (task == null)
            {
                throw new Exception("Error: Task was not found");
            }

            return task;
        }
    }
}
