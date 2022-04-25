using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using well_project_api.Models;

namespace well_project_api.Services
{
    public class PasswordService
    {
        private readonly IConfiguration _config;
        public PasswordService(IConfiguration config)
        {
            _config = config;
        }

        public string HashPassword(string password)
        {
            var salt = Encoding.ASCII.GetBytes(_config["Salt"]);            

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
