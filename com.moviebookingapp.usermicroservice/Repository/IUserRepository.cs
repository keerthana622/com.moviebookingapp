using com.moviebookingapp.usermicroservice.Collection;

namespace com.moviebookingapp.usermicroservice.Repository
{
    public interface IUserRepository
    {
        Task<Users> ValidateUser(string userEmail,string userLoginId);
        Task<Users> ValidateLoginUser(string userEmail, string userLoginId);
        Task Register(Users users);
        Task<Users> ForgotPassword(string userEmail);
        Task UpdatePassword(Users users);

    }
}
