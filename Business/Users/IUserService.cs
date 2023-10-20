using Core.Entities;
using Core.Utilities.Results;
using LoginApi.Business.Users.Dto;
using LoginApi.Core.Entities;

namespace LoginApi.Business.Users
{

    public interface IUserService
    {
        Task<IDataResult<User>> Add(User user);
        Task<IDataResult<List<UserDto>>> GetAll();
        Task<List<OperationClaim>> GetClaims(User user);
        Task<User?> GetByMail(string email);
    }
}
