using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
