using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Application.DTOs.Account;
using ShortLink.Application.Interfaces;

namespace ShortLink.Web.Areas.UserPanel.Controllers;

public class AccountController : UserPanelBaseController
{
    private IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("UserPanel")]
    public async Task<IActionResult> Index()
    {
        return View(await _userService.GetUserForShow(User.Identity.Name));
    }

    [HttpGet("UserPanel/ChangePassword")]
    public async Task<IActionResult> ChangePassword()
    {
        return View();
    }

    [HttpPost("UserPanel/ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO change)
    {
        if (!ModelState.IsValid)
            return View(change);

        string mobile = User.Identity.Name;

      var currentPasswordResult= await _userService.CompareOldPassword(change.OldPassword, mobile);
      switch (currentPasswordResult)
      {
            case ChangePasswordResult.WrongPassword:
              TempData[ErrorMessage] = "رمز عبور فعلی صحیح نمی باشد";
              ModelState.AddModelError("Mobile", "رمز عبور فعلی صحیح نمی باشد");
              return View(change);
            
      }
        var newPasswordResult= await _userService.ChangeUserPassword(mobile, change.Password);

        switch (newPasswordResult)
        {
            case ChangePasswordResult.Success:
                TempData[SuccessMessage] = "رمز عبور با موفقیت تغییر یافت";
                break;
        }

        return View(change);
    }



}

