using MVC_Validation.Models.BaseModel;
using MVC_Validation.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Validation.Controllers
{
    public class HomeController : Controller
    {
        ItemFactory itemFac = new ItemFactory();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddItemSubmit(Item item)
        {
            if (item.Name.Length < 3)
            {
                TempData["NameError"] = "Name must be atleast 4 characters long.";
            }


            return RedirectToAction("Index");
        }
    }
}