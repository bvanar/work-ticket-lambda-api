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

        public async Task RegisterUser(RegisterUserDto user)
        {
            await ValidateUsername(user.UserName);

            var existingUser = _db.User.Where(u => u.Email == user.Email).FirstOrDefault();            
            var userId = 0;
            if (existingUser != null)
            {
                if (!string.IsNullOrEmpty(existingUser.UserName))
                {
                    throw new Exception("Account already exists with this Email");
                }
                existingUser.UserName = user.UserName;
                existingUser.Password = _passwordService.HashPassword(user.Password);
                userId = existingUser.UserId;
                await _db.SaveChangesAsync();
                _db.Update(existingUser);
            }
            else
            {
                var mappedUser = _mapper.Map<User>(user);
                mappedUser.Password = _passwordService.HashPassword(user.Password);
                var newUser = (await _db.AddAsync(mappedUser)).Entity;
                await _db.SaveChangesAsync();
                userId = newUser.UserId;
            }

            if (user.AccountType == "team")
            {
                var company = new Company();
                company.CompanyName = user.CompanyName;
                var userFromDb = await _db.User.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();
                company.OwnerId = userFromDb.UserId;

                var newCompany = (await _db.AddAsync(company)).Entity;                
                await _db.SaveChangesAsync();

                await AssignUserToCompany(userId, newCompany.CompanyId);
            }            
        }

        public async Task InviteUser(InviteUserRequestDto invite)
        {
            if (invite.Email == "" || invite.CompanyId == 0)
            {
                throw new Exception("Failed to invite User: Email and company are required");
            }

            var userId = 0;

            var userExists = await _db.User.Where(z => z.Email == invite.Email).FirstOrDefaultAsync();
            if (userExists == null)
            {
                var invitedUser = (await _db.User.AddAsync(new User
                {
                    UserName = "",
                    Password = "",
                    Email = invite.Email,
                    IsAdmin = false,
                    IsDeleted = false,
                })).Entity;

                await _db.SaveChangesAsync();
                userId = invitedUser.UserId;
            } else
            {
                userId = userExists.UserId;
            }            

            await AssignUserToCompany(userId, invite.CompanyId);
        }

        public async Task ValidateUsername(string userName)
        {
            var validation = await _db.User.Where(u => u.UserName == userName).CountAsync();

            if (validation > 0)
            {
                throw new Exception($"Username: {userName} is already taken");
            }
        }

        public async Task<List<UserDto>> GetUsersByCompanyId(int companyId)
        {
            var userIds = await _db.UserCompany.Where(z => z.CompanyId == companyId).Select(z => z.UserId).ToListAsync();
            var users = await _db.User.Where(u => u.IsDeleted == false && userIds.Contains(u.UserId)).ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<UserDto>> GetUsersByJobId(int jobId)
        {
            var userIds = await _db.UserJob.Where(z => z.JobId == jobId).Select(z => z.UserId).ToListAsync();
            var users = await _db.User.Where(u => u.IsDeleted == false && userIds.Contains(u.UserId)).ToListAsync();
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

            _db.Update(userFromDb);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDto>(userFromDb);
        }

        public async Task AssignUserToCompany(int userId, int companyId)
        {
            if (companyId == 0 || userId == 0)
            {
                throw new Exception("Company ID or User ID was not present");
            }

            var existingUser = await _db.UserCompany.Where(z => z.UserId == userId && z.CompanyId == companyId).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User is already assigned to this company");
            }

            await _db.UserCompany.AddAsync(new UserCompany
            {
                UserId = userId,
                CompanyId = companyId
            });

            await _db.SaveChangesAsync();
        }  
        
        public async Task<UserDto> GetUser(int userId)
        {
            var user = await _db.User.Where(u => u.UserId == userId).FirstOrDefaultAsync();            
            return _mapper.Map<UserDto>(user);
        }
    }
}
