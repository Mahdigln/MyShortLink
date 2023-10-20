using Microsoft.AspNetCore.Mvc;
using ShortLink.Web.Models;
using System.Diagnostics;
using ShortLink.Application.DTOs.Link;
using ShortLink.Application.Interfaces;

namespace ShortLink.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constractor
        private readonly ILinkService _linkService;
        public HomeController(ILinkService linkService)
        {
            _linkService = linkService;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UrlRequestDTO urlRequest)
        {
            if (ModelState.IsValid)
            {
                if (urlRequest.OrginalUrl.Contains("https://") || urlRequest.OrginalUrl.Contains("http://"))
                {
                    var url = new Uri(urlRequest.OrginalUrl);
                    var shortUrl = _linkService.QuickShortUrl(url);

                    var result = await _linkService.AddLink(shortUrl);
                    switch (result)
                    {
                        case UrlRequestResult.Error:
                            TempData[ErrorMessage] = "مشکلی رخ داده است";
                            break;
                        case UrlRequestResult.Success:
                            TempData[SuccessMessage] = "لینک شما با موفقیت کوتاه شد";
                            ViewBag.isSuccess = true;
                            ViewBag.UserShortLink = shortUrl.Value.ToString();
                            break;
                    }

                }
                else
                {
                    TempData[ErrorMessage] = "لطفا لینک خود را با https یا http وارد نمایید";
                    return View(urlRequest);
                }

            }

            return View(urlRequest);
        }
    }
}