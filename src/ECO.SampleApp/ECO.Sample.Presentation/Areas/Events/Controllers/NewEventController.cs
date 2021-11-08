using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Areas.Events.Controllers
{
    public class NewEventController : Controller
    {
        // GET: NewEventController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NewEventController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewEventController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewEventController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewEventController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewEventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewEventController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewEventController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
