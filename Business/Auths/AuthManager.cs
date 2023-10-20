using Core.Entities;
using Core.Utilities.Results;
using LoginApi.Business.Auths;
using LoginApi.Business.Auths.Dto;
using LoginApi.Business.Users;
using LoginApi.Core.Utilities.Security.Hashing;
using LoginApi.Core.Utilities.Security.Jwt;


namespace LoginApi.Business.Auth
{
    public class AuthManager : IAuthService
    {

        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            var claims = await _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, "Access token successfully created!");
        }

        public async Task<IDataResult<User>> Login(LoginDto loginDto)
        {
            var userToCheck = await _userService.GetByMail(loginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("User not found!");
            }

            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Wrong password!");
            }

            return new SuccessDataResult<User>(userToCheck, "Successfully login!");
        }

        public async Task<IDataResult<User>> Register(RegisterDto registerDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);
            var user = new User {
                Email = registerDto.Email,
                FirstName = registerDto.Email,
                LastName = registerDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            var createdUser = await _userService.Add(user);
            return new SuccessDataResult<User>(createdUser.Data,"register successfully");
        }

        public async Task<IResult> UserExists(string email)
        {
            if (await _userService.GetByMail(email) != null)
            {
                return new ErrorResult("User already exists!");
            }
            return new SuccessResult();
        }
    }
}
