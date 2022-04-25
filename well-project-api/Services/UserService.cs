using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using well_project_api.Dto.User;
using well_project_api.Models;

namespace well_project_api.Services
{
    public class UserService
    {
        private readonly WellDbContext _db;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;
        public UserService(WellDbContext db, IMapper mapper, PasswordService passwordService)
        {
            _db = db;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<UserDto> Login(UserRequestDto loginRequest)
        {
            if (loginRequest.UserName == string.Empty)
            {
                throw new Exception("Username is required");
            }

            // use the password service to hash new 
            // need to use a common salt
            var hashedPassword = _passwordService.HashPassword(loginRequest.Password);

            var user = await _db.User.Where(u => u.UserName == loginRequest.UserName && u.Password == hashedPassword).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            user.LastLogin = DateTime.UtcNow;
            _db.Update(user);
            await _db.SaveChangesAsync();            

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterUser(UserDto user)
        {
            await ValidateUsername(user.UserName);

            if (user.CompanyId == 0)
            {
                throw new Exception("Error Creating User: The User must be tied to a company");
            }

            var mappedUser = _mapper.Map<User>(user);

            mappedUser.Password = _passwordService.HashPassword(user.Password);
            var newUser = (await _db.AddAsync(mappedUser)).Entity;            

            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task ValidateUsername(string userName)
        {
            var validation = await _db.User.Where(u => u.UserName == userName).CountAsync();

            if (validation > 0)
            {
                throw new Exception($"Username: {userName} is already taken");
            }
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _db.User.Where(u => u.IsDeleted == false).ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task ResetPassword(UserRequestDto userRequest)
        {
            var user = await _db.User.Where(u => u.UserName == userRequest.UserName).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new Exception($"User ({userRequest.UserName}) does not exist");
            }

            user.Password = "";
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            var userFromDb = await _db.User.Where(x => x.UserId == user.UserId).SingleOrDefaultAsync();
            userFromDb.UserName = user.UserName;
            userFromDb.IsAdmin = user.IsAdmin;
            userFromDb.IsDeleted = user.IsDeleted;
            userFromDb.CompanyId = user.CompanyId;

            _db.Update(userFromDb);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(userFromDb);
        }
    }
}
