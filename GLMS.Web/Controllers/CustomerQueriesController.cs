using GLMS.Web.Data;
using GLMS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    [AdminAuthorize]
    public class CustomerQueriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerQueriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var queries = await _context.CustomerQueries
                .OrderByDescending(q => q.SubmittedAt)
                .ToListAsync();

            return View(queries);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var query = await _context.CustomerQueries.FindAsync(id);

            if (query == null)
                return NotFound();

            _context.CustomerQueries.Remove(query);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Customer query deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}