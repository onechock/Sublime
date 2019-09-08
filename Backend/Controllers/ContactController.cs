using Backend.Models;
using Backend.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backend.Controllers
{
    public class ContactController : Controller
    {
		private readonly Repository _repository;

		public ContactController()
		{
			_repository = new Repository();
		}

		// GET: Contact
		public ActionResult Index()
        {
			var contacts = _repository.GetContacts();

			return View(contacts);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
			ViewBag.Companies = _repository.GetCompanies();

			return View();
        }

        // POST: Contact/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
				_repository.CreateContact(
					collection["FirstName"],
					collection["LastName"],
					collection["Email"],
					collection["PhoneNumber"],
					int.Parse(collection["CompanyId"])
				);

				return RedirectToAction("Index");
            }
            catch
			{
				ViewBag.Companies = _repository.GetCompanies();

				@ViewBag.Error = true;

				return View();
            }
        }

		// GET: Contact/Edit/5
		public ActionResult Edit(int id)
		{
			ViewBag.Companies = _repository.GetCompanies();
			var model = _repository.GetContact(id);

			return View(model);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
				_repository.UpdateContact(
					id, 
					collection["FirstName"],
					collection["LastName"],
					collection["Email"],
					collection["PhoneNumber"],
					int.Parse(collection["CompanyId"])
				);

				return RedirectToAction("Index");
            }
            catch
			{
				ViewBag.Companies = _repository.GetCompanies();

				@ViewBag.Error = true;

				var model = _repository.GetContact(id);

				return View(model);
            }
        }
    }
}
