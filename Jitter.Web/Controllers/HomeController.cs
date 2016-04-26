using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Jitter.Web.Models;
using Microsoft.AspNet.Identity;
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
            var userid = User.Identity.GetUserId();
            var model = new AboutVM
            {
                Tweaks = userInfo.Tweaks.Select(
                    t =>
                        new TweakVM()
                        {
                            Body = t.Body,
                            TweakDate = t.TweakDate,
                            TweakId = t.Id,
                            UserHandle = t.JitterUser.Handle
                        }).ToList(),
                Following =
                    userInfo.Followers.Select(x => new { Key = x.FollowingId, Value = x.Following.Handle }).ToDictionary(t => t.Key, t => t.Value),
                FollowerCount = userInfo.Followers.Count,
                Followers =
                    userInfo.Following.Select(x => new { Key = x.FollowerId, Value = x.Follower.Handle }).ToDictionary(t => t.Key, t => t.Value),
                FollowingCount = userInfo.Following.Count,
                TweaksCount = userInfo.Tweaks.Count,
                PhotoURL = userInfo.PhotoURL,
                CanDelete = userInfo.Id == userid
            };


            return View(model);
        }

        //POST: Home/Create
        [HttpPost]
        public ActionResult Create(string body)
        {
            if (ModelState.IsValid)
            {
                var tweak = new Tweak() { Body = body };
                var currentUser = User.Identity;
                var userInfo = db.Users.FirstOrDefault(x => x.Email == currentUser.Name);
                userInfo.Tweaks.Add(tweak);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        // POST: Home/ManageFollowing/5
        [HttpPost, ActionName("ManageFollowing")]
        public ActionResult ManageFollowing(string id)
        {
            var userid = User.Identity.GetUserId();

            var userInfo = db.Users.Find(userid);

            if (userInfo.Following.Any(x => x.FollowerId == userid && x.FollowingId == id))
            {
                userInfo.Following.Remove(new JitterFollowers() { FollowerId = userid, FollowingId = id });
            }
            else
            {
                userInfo.Following.Add(new JitterFollowers() { FollowerId = userid, FollowingId = id });
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var userid = User.Identity.GetUserId();

            Tweak tweak = db.Tweaks.Find(id);

            if (userid != tweak.JitterUser.Id)
            {
                return new HttpUnauthorizedResult("Can't delete tweaks you don't own.");
            }
            db.Tweaks.Remove(tweak);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}