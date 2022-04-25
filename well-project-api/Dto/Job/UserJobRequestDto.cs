using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Dto.Job
{
    public class UserJobRequestDto
    {
        public int JobId { get; set; }        
        public int UserId { get; set; }
    }
}
