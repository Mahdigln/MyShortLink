using Microsoft.AspNetCore.Mvc;

namespace ShortLink.Web.Areas.UserPanel.Controllers;
[Area("UserPanel")]
public class UserPanelBaseController : Controller
{
    protected string ErrorMessage = "ErrorMessage";
    protected string InfoMessage = "InfoMessage";
    protected string SuccessMessage = "SuccessMessage";
    protected string WarningMessage = "WarningMessage";
}
