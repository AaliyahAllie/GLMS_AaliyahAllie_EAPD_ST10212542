using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ClientCount = await _context.Clients.CountAsync();
            ViewBag.ContractCount = await _context.Contracts.CountAsync();
            ViewBag.ServiceRequestCount = await _context.ServiceRequests.CountAsync();

            ViewBag.ActiveContracts = await _context.Contracts
                .CountAsync(c => c.Status == ContractStatus.Active);

            return View();
        }
    }
}