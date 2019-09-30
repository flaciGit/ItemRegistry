using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ItemRegistry.Models;
using PagedList;

namespace ItemRegistry.Controllers
{
    public class ItemController : Controller
    {

        public bool userIsAdmin() {

            if (Session["userIsAdmin"] != null && (bool)Session["userIsAdmin"] == true)
                return true;

            return false;
        }

        [Route("Items")]
        [AllowAnonymous]
        public ActionResult Index(FormCollection form, string sortOrder, string searchString, string currentFilter, int? page)
        {
            using (DBModelEntity dbModel = new DBModelEntity()) {

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.ValueSortParm = sortOrder == "Value" ? "Value_desc" : "Value";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                
                ViewBag.CurrentFilter = searchString;
                
                var items = from s in dbModel.Items
                               select s;

                if (!String.IsNullOrEmpty(searchString)) {
                    int result = 0;
                    if(int.TryParse(searchString, out result))
                    {
                        items = items.Where(s => s.Name.Contains(searchString) || s.Value.ToString().Contains(searchString));
                    }
                    else
                    {
                        items = items.Where(s => s.Name.Contains(searchString));
                    }
                }

                switch (sortOrder)
                {
                    
                    case "name_desc":
                        items = items.OrderByDescending(s => s.Name);
                        break;
                    case "Value":
                        items = items.OrderBy(s => s.Value);
                        break;
                    case "Value_desc":
                        items = items.OrderByDescending(s => s.Value);
                        break;
                    default:
                        items = items.OrderBy(s => s.Id);
                        break;  
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(items.ToPagedList(pageNumber, pageSize));

                //return View(items.ToList());
            }
        }

        // GET: Item/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            using (DBModelEntity dbModel = new DBModelEntity())
            {

                return View(dbModel.Items.Where(x => x.Id == id).FirstOrDefault());

            }
        }

        [Authorize]
        public ActionResult Create()
        {
            if (!userIsAdmin())
                return RedirectToAction("Login", "Login");

            return View();
        }

        // GET: Item/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Items item)
        {
            if (!userIsAdmin())
                return RedirectToAction("Login","Login");

            try
            {
                using (DBModelEntity dbModel = new DBModelEntity())
                {

                    dbModel.Items.Add(item);
                    dbModel.SaveChanges();

                }

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }


        // GET: Item/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (!userIsAdmin())
                return RedirectToAction("Login", "Login");

            using (DBModelEntity dbModel = new DBModelEntity())
            {

                return View(dbModel.Items.Where(x => x.Id == id).FirstOrDefault());

            }
        }

        // POST: Item/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Items item)
        {
            if (!userIsAdmin())
                return RedirectToAction("Login", "Login");

            try
            {
                // TODO: Add update logic here
                using (DBModelEntity dbModel = new DBModelEntity())
                {

                    dbModel.Entry(item).State = EntityState.Modified;
                    dbModel.SaveChanges();
                    
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            if (!userIsAdmin())
                return RedirectToAction("Login", "Login");

            using (DBModelEntity dbModel = new DBModelEntity())
            {

                return View(dbModel.Items.Where(x => x.Id == id).FirstOrDefault());

            }
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (!userIsAdmin())
                return RedirectToAction("Login", "Login");

            try
            {
                using (DBModelEntity dbModel = new DBModelEntity()) {

                    Items item = dbModel.Items.Where(x => x.Id == id).FirstOrDefault();
                    dbModel.Items.Remove(item);
                    dbModel.SaveChanges();

                    
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
        }
    }
}
