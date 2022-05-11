using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Dto.Tasks
{
    public class CompleteTaskDto
    {
        public int TaskId { get; set; }                
        public bool Completed { get; set; }                
        public int? CompletedByUserId { get; set; }       
    }
}
