using GLMS.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    [Authorize]
    public class ServiceInquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceInquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var queries = await _context.ServiceInquiries
                .OrderByDescending(q => q.SubmittedAt)
                .ToListAsync();

            return View(queries);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var query = await _context.ServiceInquiries.FindAsync(id);

            if (query == null)
                return NotFound();

            _context.ServiceInquiries.Remove(query);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Customer query deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}