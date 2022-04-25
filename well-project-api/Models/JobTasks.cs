using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class JobTasks
    {
        public int TaskId { get; set; }
        public int JobId { get; set; }
        public string TaskName { get; set; }
        public int TaskOrder { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; } = null;
        public bool IsDeleted { get; set; }
    }
}
