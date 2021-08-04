using WooliesXTest.DTO;

namespace WooliesX.Services.Interfaces
{
    public interface IUserService
    {
        UserResponse GetUserAndToken();
    }
}
