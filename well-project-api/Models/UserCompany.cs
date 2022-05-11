using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class UserCompany
    {
        public int UserCompanyId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }                             
    }
}
