using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Contexts;
using LoginApi.Business.Users.Dto;
using LoginApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApi.Business.Users
{
    public class UserManager : IUserService
    {
        private readonly LoginDbContext _context;

        public UserManager(LoginDbContext context)
        {
            _context = context;
        }

        public async Task<IDataResult<User>> Add(User user)
        {
            var newUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new SuccessDataResult<User>(newUser.Entity, "user added");
        }

        public async Task<IDataResult<List<UserDto>>> GetAll()
        {
            var users = await _context.Users.Select(u => new UserDto { Id = u.Id, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName }).ToListAsync();
            return new  SuccessDataResult<List<UserDto>>(users);
        }

        public async Task<User?> GetByMail(string email)
        {
            return await _context.Users.Where(p => p.Email == email).FirstOrDefaultAsync();

        }

        public async Task<List<OperationClaim>> GetClaims(User user)
        {
            var res = await _context
                 .UserOperationClaims
                 .Where(p => p.UserId == user.Id)
                 .Select(p => new OperationClaim
                 {
                     Id = p.OperationClaimId,
                     Name = p.OperationClaim.Name
                 })
                 .ToListAsync();
            return res;
        }
    }
}
