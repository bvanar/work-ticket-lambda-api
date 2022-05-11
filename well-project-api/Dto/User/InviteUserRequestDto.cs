using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace well_project_api.Dto.User
{
    public class InviteUserRequestDto
    {
        public string Email { get; set; }        
        public int CompanyId { get; set; }
    }
}
