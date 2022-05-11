using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace well_project_api.Dto.User
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }        
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string AccountType { get; set; }
        public string Email { get; set; }
    }
}
