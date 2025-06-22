using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_project.Controllers
{
    public class SelectController : Controller
    {
        // GET: SelectController
        public ActionResult Select()
        {
            return View();
        }

        // GET: SelectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SelectController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SelectController/Create
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

        // GET: SelectController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SelectController/Edit/5
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

        // GET: SelectController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SelectController/Delete/5
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
