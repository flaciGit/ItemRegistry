using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ItemRegistry.Models;

namespace ItemRegistry.Controllers
{
    public class UserController : Controller
    {
        // GET: Customer

        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Authorize() {

            return View();
        }


        [HttpGet]
        [Route("Registration")]
        public ActionResult AddOrEdit(int id=0)
        {
            Users customerModel = new Users();


            return View(customerModel);
        }

        [HttpPost]
        [Route("Registration")]
        public ActionResult AddOrEdit(Users user)
        {
            using (DBModelEntity dbmodel = new DBModelEntity())
            {

                if(dbmodel.Users.Any(x => x.Name == user.Name))
                {
                    ViewBag.DuplicateMessage = "Username already exists";
                    return View("AddOrEdit", user);
                }


                dbmodel.Users.Add(user);
                dbmodel.SaveChanges();

            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";

            return View("AddOrEdit", new Users());
        }
    }
}