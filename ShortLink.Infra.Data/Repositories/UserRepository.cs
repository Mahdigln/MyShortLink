using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShortLink.Application.Interfaces;
using ShortLink.Application.Services;
using ShortLink.Domain.Interface;
using ShortLink.Domain.Models.Account;
using ShortLink.Domain.ViewModels.Account;
using ShortLink.Infra.Data.Context;

namespace ShortLink.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    #region constructor

    private readonly ShortLinkContext _context;
    private IPasswordHelper _passwordHelper;
    public UserRepository(ShortLinkContext context, IPasswordHelper passwordHelper)
    {
        _context = context;
        _passwordHelper = passwordHelper;
    }

    #endregion

    #region dispose & save change
    public async ValueTask DisposeAsync()
    {
        if (_context != null) await _context.DisposeAsync();
    }

    public async Task SaveChange()
    {
        await _context.SaveChangesAsync();
    }
    #endregion


    #region Account

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> IsExistMobile(string mobile)
    {
        return await _context.Users.AnyAsync(u => u.Mobile == mobile);
    }
    public async Task<User> GetUserByMobile(string mobile)
    {
        return await _context.Users.AsQueryable().SingleOrDefaultAsync(u => u.Mobile == mobile);
    }

    public async Task<List<UserForShowViewModel>> GetAllUserForShow()
    {
        var allUser = await _context.Users.AsQueryable()
            .Select(u => new UserForShowViewModel
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Mobile = u.Mobile,
                IsAdmin = u.IsAdmin,
                IsBlocked = u.IsBlocked,
            }).ToListAsync();

        return allUser;
    }

    public async Task<UserForShowViewModels> GetUserForShow(string mobile)
    {
        var user = await GetUserByMobile(mobile);
        UserForShowViewModels showViewModel = new UserForShowViewModels()
        {
            Mobile = user.Mobile,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserId = user.Id,
            IsAdmin = user.IsAdmin,
            IsBlocked = user.IsBlocked
        };
        return  showViewModel;
    }

    public async Task<User> GetUserById(long userId)
    {
        return await _context.Users.AsQueryable()
            .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> CompareOldPassword(string oldPassword, string mobile)
    {
        string hashOldPassword = _passwordHelper.EncodePasswordMd5(oldPassword);
        return await _context.Users.AnyAsync(u => u.Mobile == mobile && u.Password == hashOldPassword);
    }
   
    public async Task ChangeUserPassword(string mobile, string newPassword)
    {
        var user =await GetUserByMobile(mobile);
        user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
       UpdateUser(user);
       await SaveChange();
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
    }

    #endregion


}