using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Jitter.Web.Models;
using DbContext = Jitter.Web.Models.DbContext;

namespace Jitter.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();

        //GET: Home/Index
        public ActionResult Index()
        {
            var currentUser = User.Identity;
            var userInfo = db.Users.FirstOrDefault(x => x.Email == currentUser.Name);
            return View(userInfo);
        }


        //GET: Home/About/{id}
        public ActionResult About(string userHandle)
        {
            var userInfo = db.Users.FirstOrDefault(x => x.Handle == userHandle);
            return View(userInfo);
        }

        //POST: Home/Create
        [HttpPost]
        public ActionResult Create(string body)
        {
            if (ModelState.IsValid)
            {
                var tweak = new Tweak() {Body = body};
                var currentUser = User.Identity;
                var userInfo = db.Users.FirstOrDefault(x => x.Email == currentUser.Name);
                userInfo.Tweaks.Add(tweak);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            Tweak tweak = db.Tweaks.Find(id);
            db.Tweaks.Remove(tweak);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}