using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsDeleted { get; set; }
        public int OwnerId { get; set; }
    }
}
