using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iGrow.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
