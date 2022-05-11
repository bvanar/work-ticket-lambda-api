using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
    }
}
