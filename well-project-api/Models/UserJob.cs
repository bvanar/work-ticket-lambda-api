using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class UserJob
    {
        public int UserJobId { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
    }
}
