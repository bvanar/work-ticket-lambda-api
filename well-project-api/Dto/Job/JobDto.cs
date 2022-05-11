using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using well_project_api.Dto.Tasks;

namespace well_project_api.Dto.Job
{
    public class JobDto
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int CompanyId { get; set; }
        public int OwnerId { get; set; }        
        public List<TaskDto> Tasks { get; set; }
    }
}
