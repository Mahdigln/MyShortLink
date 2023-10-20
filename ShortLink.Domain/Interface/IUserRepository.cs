using ShortLink.Domain.Models.Account;
using ShortLink.Domain.ViewModels.Account;

namespace ShortLink.Domain.Interface;

public interface IUserRepository:IAsyncDisposable
{
    Task AddUser(User user);
    Task<bool> IsExistMobile(string mobile);
    Task<User> GetUserByMobile(string mobile);
    Task<List<UserForShowViewModel>> GetAllUserForShow();
    Task<UserForShowViewModels> GetUserForShow(string mobile);
    Task<User> GetUserById(long userId);
    Task<bool> CompareOldPassword(string oldPassword, string mobile);
    Task  ChangeUserPassword(string mobile, string newPassword);
    void UpdateUser(User user);
    Task SaveChange();
}