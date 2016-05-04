using MVC_Validation.Models.BaseModels;
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
        PersonFactory personFac = new PersonFactory();
        // GET: Home
        public ActionResult Index()
        {
            if (TempData["Item"] != null)
            {
                return View((TempData["Item"] as Item));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddItemSubmit(Item item)
        {
            bool canSubmit = true;
            if (item.Name == null || item.Name.Length < 3)
            {
                TempData["NameError"] = "Name must be atleast 4 characters long.";
                canSubmit = false;
            }

            int n;
            if (item.Price == null || !int.TryParse(item.Price, out n))
            {
                TempData["PriceError"] = "Price must be a number.";
                canSubmit = false;
            }

            if (item.Description == null || item.Description.Length < 1)
            {
                TempData["DescriptionError"] = "You must type in a description.";
                canSubmit = false;
            }

            if (item.Description != null && item.Description.Length > 250)
            {
                TempData["DescriptionError"] = "The description can only contain 250 characters.";
                canSubmit = false;
            }

            if (canSubmit == true)
            {
                itemFac.Add(item);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Item"] = item;
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddPerson()
        {
            if (TempData["Person"] != null)
            {
                return View((TempData["Person"] as Person));
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddPersonSubmit(Person person)
        {
            bool canSubmit = true;

            if (person.Firstname == null || person.Firstname.Length < 2)
            {
                TempData["FirstnameError"] = "Firstname must contain atleast 2 characters";
                canSubmit = false;
            }

            if (person.Lastname == null || person.Lastname.Length < 2)
            {
                TempData["LastnameError"] = "Lastname must contain atleast2 characters";
                canSubmit = false;
            }

            if (person.Age < 1)
            {
                TempData["AgeError"] = "You are not JESUS CHRIST SUPERSTAR";
                canSubmit = false;
            }

            if (person.Address == null || person.Address.Length < 2)
            {
                TempData["AddressError"] = "Address must contain atleast 2 characters";
                canSubmit = false;
            }

            if (person.Address != null && person.Address.Length > 255)
            {
                TempData["AddressError"] = "Address can only contain 255 characters";
                canSubmit = false;
            }

            if (canSubmit)
            {
                personFac.Add(person);
            }
            else
            {
                TempData["Person"] = person;
            }

            return RedirectToAction("AddPerson");
        }
    }
}