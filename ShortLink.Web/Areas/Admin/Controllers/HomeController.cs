using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShortLink.Application.Interfaces;

namespace ShortLink.Web.Areas.Admin.Controllers;

public class HomeController : AdminBaseController
{
    #region constractor

    private readonly ILinkService _linkService;

    public HomeController(ILinkService linkService)
    {
        _linkService = linkService;
    }

    #endregion

    public async Task<IActionResult> Index(int pageId = 1, string filter = "", string orderBy = "Descending",
        int take = 0)
    {
        ViewBag.pageId = pageId;

        return View(await _linkService.GetAllLink(pageId, filter, orderBy, 5));

    }
}



