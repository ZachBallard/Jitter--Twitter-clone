using System.Web.Mvc;
using System.Web.Security;

namespace Jitter.Web.Controllers
{
    public class SharedController : Controller
    {
        public void SignOut()
        {
            FormsAuthentication.SignOut();
            RedirectToRoute("Home", "Index");
        }
    }
}