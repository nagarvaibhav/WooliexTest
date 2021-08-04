using System;
using WooliesX.Services.Interfaces;
using WooliesXTest.DTO;
using WooliesXTest.Utility;

namespace WooliesXTest.Services
{
    public class UserService : IUserService
    {
        public UserResponse GetUserAndToken()
        {
            return new UserResponse
            {
                Token = Guid.NewGuid().ToString(),
                Name = Constants.User
            };
        }
    }
}
