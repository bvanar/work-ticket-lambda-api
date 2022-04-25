using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace well_project_api.Dto.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }
    }
}
