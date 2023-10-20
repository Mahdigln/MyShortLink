using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Application.DTOs.Account;
using ShortLink.Application.Interfaces;

namespace ShortLink.Web.Controllers;

public class AccountController : SiteBaseController
{
    #region constractor
    private readonly IUserService _userService;
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    #endregion

    #region user register
    [HttpGet("Registerr")]
    public async Task<IActionResult> Register()
    {

        return View();
    }

    [HttpPost("Registerr"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserDTO registerUser)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUser(registerUser);
            switch (result)
            {
                case RegisterUserResult.IsMobileExist:
                    TempData[ErrorMessage] = "این شماره تلفن قبلا ثبت نام کرده";
                    ModelState.AddModelError("Mobile", "این شماره تلفن قبلا ثبت نام کرده");
                    break;
                case RegisterUserResult.Success:
                    TempData[SuccessMessage] = "ثبت نام شما موفقیت آمیز بود";
                    return Redirect("/");
            }
        }
        return View(registerUser);
    }
    #endregion

    #region user login
    [HttpGet("Login")]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost("Login"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserDTO loginUser)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.LoginUser(loginUser);
            switch (result)
            {
                case LoginUserResult.NotFound:
                    TempData[ErrorMessage] = "User Not Found";
                    break;
                case LoginUserResult.NotActivate:
                    TempData[WarningMessage] = "This User Is Not Active";
                    break;
                case LoginUserResult.Success:
                    var user = await _userService.GetUserByMobile(loginUser.Mobile);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Mobile.ToString()),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim("IsAdmin",user.IsAdmin.ToString())

                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principle = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = loginUser.RememberMe
                    };
                    await HttpContext.SignInAsync(principle, properties);
                    TempData[SuccessMessage] = "شما وارد حساب کاربری شدید";
                    return Redirect("/");
            }
        }
        return View(loginUser);
    }
    #endregion

    #region user logout
    [HttpGet("log-Out")]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        TempData[SuccessMessage] = "شما از حساب کاربری خارج شدید";
        return Redirect("/");
    }
    #endregion
}
