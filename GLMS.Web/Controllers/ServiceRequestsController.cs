using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrencyService _currencyService;
        private readonly IContractWorkflowService _workflowService;

        public ServiceRequestsController(
            ApplicationDbContext context,
            ICurrencyService currencyService,
            IContractWorkflowService workflowService)
        {
            _context = context;
            _currencyService = currencyService;
            _workflowService = workflowService;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> Details(int id)
        {
            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (serviceRequest == null)
                return NotFound();

            return View(serviceRequest);
        }

        public async Task<IActionResult> Create()
        {
            await LoadContractDropdownAsync();

            ViewBag.ExchangeRate = await _currencyService.GetUsdToZarRateAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Please select a valid contract.");
            }
            else if (!_workflowService.CanCreateServiceRequest(contract))
            {
                ModelState.AddModelError(
                    "",
                    "Service Request cannot be created because the selected contract is Expired or On Hold."
                );
            }

            try
            {
                serviceRequest.ExchangeRate = await _currencyService.GetUsdToZarRateAsync();
                serviceRequest.Cost = _currencyService.ConvertUsdToZar(
                    serviceRequest.UsdAmount,
                    serviceRequest.ExchangeRate
                );
                serviceRequest.Status = ServiceRequestStatus.Pending;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (ModelState.IsValid)
            {
                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Service request created successfully.";
                return RedirectToAction(nameof(Index));
            }

            await LoadContractDropdownAsync(serviceRequest.ContractId);
            ViewBag.ExchangeRate = serviceRequest.ExchangeRate;

            return View(serviceRequest);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (serviceRequest == null)
                return NotFound();

            return View(serviceRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);

            if (serviceRequest == null)
                return NotFound();

            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Service request deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadContractDropdownAsync(int? selectedContractId = null)
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .OrderBy(c => c.Client!.Name)
                .ThenBy(c => c.ServiceLevel)
                .ToListAsync();

            var contractList = contracts.Select(c => new
            {
                c.ContractId,
                DisplayName = $"{c.Client!.Name} - {c.ServiceLevel} ({c.Status})"
            });

            ViewBag.ContractId = new SelectList(
                contractList,
                "ContractId",
                "DisplayName",
                selectedContractId
            );
        }
    }
}