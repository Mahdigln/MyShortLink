using Microsoft.AspNetCore.Mvc;
using ShortLink.Application.Interfaces;
using System.Threading.Tasks;
using ShortLink.Application.DTOs.Account;

namespace ShortLink.Web.Areas.Admin.Controllers;
public class UserController : AdminBaseController
    {
        #region constractor
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region user list
        [HttpGet("Admin/User")]
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllUserForShow());
        }
        #endregion

        #region edit user
        [HttpGet("Admin/EditUser/{userId}")]
        public async Task<IActionResult> EditUser(long userId)
        {
            return View(await _userService.GetEditUserByAdmin(userId));
        }

        [HttpPost("Admin/EditUser/{userId}")]
        public async Task<IActionResult> EditUser(EditUserDTO editUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUser(editUser);
                switch (result)
                {
                    case EditUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserResult.Success:
                        TempData[SuccessMessage] = $"کاربر {editUser.LastName} با موفقیت ویرایش شد  ";
                        return RedirectToAction("Index");
                }
            }

            return View(editUser);
        }
        #endregion

        #region create user
        [HttpGet("Admin/CreateUser")]

        public async Task<IActionResult> CreateUser()
        {
            return View();
        }

       [HttpPost("Admin/CreateUser"), ValidateAntiForgeryToken]
      public async Task<IActionResult> CreateUser(CreateUserDTO createUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddUser(createUser);
                switch (result)
                {
                    case CreateUserResult.Error:
                        TempData[ErrorMessage] = "شماره تلفن همراه وارد شده تکراری میباشد";
                        break;
                    case CreateUserResult.Success:
                        TempData[SuccessMessage] = "عملیات افزودن کاربر با موفقیت انجام شد";
                        return RedirectToAction("Index");
                }
            }

            return View(createUser);
        }
        #endregion
}

