using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class PublicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ServiceInquiry query)
        {
            if (ModelState.IsValid)
            {
                query.SubmittedAt = DateTime.Now;

                _context.CustomerQueries.Add(query);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you. Your logistics query has been submitted successfully.";
                return RedirectToAction(nameof(Contact));
            }

            return View(query);
        }
    }
}