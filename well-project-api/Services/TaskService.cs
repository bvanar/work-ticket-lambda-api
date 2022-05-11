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
        private readonly UserService _userService;
        public TaskService(WellDbContext db, IMapper mapper, UserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<TaskDto>> GetTasksByJobId(int jobId)
        {
            var tasks = await _db.JobTasks.Where(x => x.JobId == jobId && x.IsDeleted == false).ToListAsync();

            if (tasks.Count == 0)
            {
                throw new Exception($"The selected job (id: {jobId}) has no tasks associatied.");
            }

            var mappedTasks = _mapper.Map<List<TaskDto>>(tasks);
            foreach(var task in mappedTasks)
            {
                if (task.CompletedByUserId == null) continue;
                var user = await _userService.GetUser((int)task.CompletedByUserId);
                task.CompletedByUserName = user.UserName;
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

        public async Task<TaskDto> CompleteTask(CompleteTaskDto task)
        {
            var taskFromDb = await GetTask(task.TaskId);
            taskFromDb.Completed = task.Completed;            

            if (task.Completed == true)
            {
                taskFromDb.CompletedDate = DateTime.UtcNow;
                taskFromDb.CompletedByUserId = task.CompletedByUserId;
            }
            else
            {
                taskFromDb.CompletedDate = null;
                taskFromDb.CompletedByUserId = null;
            }

            var completedTask = (_db.Update(taskFromDb)).Entity;

            

            await _db.SaveChangesAsync();

            var mappedTask = _mapper.Map<TaskDto>(completedTask);

            if (mappedTask.CompletedByUserId != null)
            {
                var user = await _userService.GetUser((int)mappedTask.CompletedByUserId);
                mappedTask.CompletedByUserName = user.UserName;
            }

            return mappedTask;
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
