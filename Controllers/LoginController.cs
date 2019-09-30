using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ItemRegistry.Models;

using Microsoft.AspNetCore.Identity;

//using Microsoft.AspNetCore.Identity.EntityFramework;

namespace ItemRegistry.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        [Route("Profile")]
        [AllowAnonymous]
        public ActionResult UserProfile()
        {
            if(Session["userName"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            using (DBModelEntity dbModel = new DBModelEntity())
            {
                string userName =  this.HttpContext.Session["userName"].ToString();
                return View(dbModel.Users.Where(x => x.Name == userName).FirstOrDefault());

            }
        }

        [Route("Logout")]
        [Authorize]
        public ActionResult Logout()
        {
            //QUESTIONABLE
            
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/Home");

            return RedirectToAction("Index","Home");
        }

        //private readonly SignInManager<IdentityUser>

        [HttpPost]
        [AllowAnonymous]
        // do with async? it should speed up the login process
        //public async Task<ActionResult> Authorize(Customers customer)
        public ActionResult Authorize(Users customer)
        {
            using (DBModelEntity db = new DBModelEntity())
            {

                //var result = await _signInManager.Pass

                var userDetails = db.Users.Where(x => x.Name == customer.Name && x.Password == customer.Password).FirstOrDefault();

                if (userDetails == null)
                {

                    ViewBag.ErrorMessage = "Login error";
                    return View("Login", customer);
                }
                else
                {
                    Session["userId"] = userDetails.Id;
                    Session["userName"] = userDetails.Name;
                    Session["userIsAdmin"] = userDetails.IsAdmin;

                    HttpCookie UserCookie = new HttpCookie("user", userDetails.Id.ToString());

                    UserCookie.Expires.AddDays(10);

                    HttpContext.Response.SetCookie(UserCookie);

                    //HttpCookie NewCookie = Request.Cookies["user"];

                    //return NewCookie.Value;

                    //rememberme always set to true for now
                    FormsAuthentication.SetAuthCookie(userDetails.Name, true);
                    return RedirectToAction("Index", "Home");
                }
            }
            //return View();
        }



        //[HttpPost]
        //public ActionResult Authorize(Customers customer)
        //{
        //    using (DBModelEntity db = new DBModelEntity()) {

        //        var userDetails = db.Customers.Where(x => x.CustomerName == customer.CustomerName && x.CustomerPassword == customer.CustomerPassword).FirstOrDefault();

        //        if (userDetails == null) {

        //            ViewBag.ErrorMessage = "Login error";
        //            return View("Login", customer);
        //        }
        //        else
        //        {
        //            Session["userId"] = userDetails.Id;
        //            Session["userName"] = userDetails.CustomerName;
        //            Session["userIsAdmin"] = userDetails.CustomerIdAdmin;

        //            HttpCookie UserCookie = new HttpCookie("user", userDetails.Id.ToString());

        //            UserCookie.Expires.AddDays(10);

        //            HttpContext.Response.SetCookie(UserCookie);

        //            HttpCookie NewCookie = Request.Cookies["user"];

        //            //return NewCookie.Value;

        //            return RedirectToAction("Index","Home");
        //        }
        //    }
        //    //return View();
        //}




    }
}