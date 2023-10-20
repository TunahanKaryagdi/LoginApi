
using Core.Entities;
using Core.Utilities.Results;
using LoginApi.Business.Auths;
using LoginApi.Business.Auths.Dto;
using LoginApi.Core.Utilities.Security.Jwt;

namespace LoginApi.Business.Auths
{
    public interface IAuthService
    {
        Task<IDataResult<User>> Login(LoginDto loginDto);
        Task<IDataResult<User>> Register(RegisterDto registerDto);
        Task<IResult> UserExists(string email);
        Task<IDataResult<AccessToken>> CreateAccessToken(User user);
    }
}
