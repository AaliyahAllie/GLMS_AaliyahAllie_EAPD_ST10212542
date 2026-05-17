using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileValidationService _fileValidationService;
        private readonly IContractWorkflowService _workflowService;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(
            ApplicationDbContext context,
            IFileValidationService fileValidationService,
            IContractWorkflowService workflowService,
            IWebHostEnvironment environment)
        {
            _context = context;
            _fileValidationService = fileValidationService;
            _workflowService = workflowService;
            _environment = environment;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (startDate.HasValue)
                contracts = contracts.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                contracts = contracts.Where(c => c.EndDate <= endDate.Value);

            if (status.HasValue)
                contracts = contracts.Where(c => c.Status == status.Value);

            return View(await contracts.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        public IActionResult Create()
        {
            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (!_workflowService.IsValidContractDateRange(contract.StartDate, contract.EndDate))
            {
                ModelState.AddModelError("", "End date must be after start date.");
            }

            if (contract.SignedAgreementUpload != null)
            {
                try
                {
                    _fileValidationService.ValidatePdf(contract.SignedAgreementUpload);
                    contract.SignedAgreementPath = await SaveFile(contract.SignedAgreementUpload);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Contract created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.ContractId)
                return NotFound();

            if (!_workflowService.IsValidContractDateRange(contract.StartDate, contract.EndDate))
            {
                ModelState.AddModelError("", "End date must be after start date.");
            }

            var existingContract = await _context.Contracts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (existingContract == null)
                return NotFound();

            if (contract.SignedAgreementUpload != null)
            {
                try
                {
                    _fileValidationService.ValidatePdf(contract.SignedAgreementUpload);
                    contract.SignedAgreementPath = await SaveFile(contract.SignedAgreementUpload);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                contract.SignedAgreementPath = existingContract.SignedAgreementPath;
            }

            if (ModelState.IsValid)
            {
                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Contract updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClientId = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            if (contract.ServiceRequests.Any())
            {
                _context.ServiceRequests.RemoveRange(contract.ServiceRequests);
            }

            if (!string.IsNullOrEmpty(contract.SignedAgreementPath))
            {
                DeleteAgreementFile(contract.SignedAgreementPath);
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Contract and linked service requests deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound();

            var filePath = Path.Combine(_environment.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "contracts");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + ".pdf";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/contracts/" + uniqueFileName;
        }

        private void DeleteAgreementFile(string filePathFromDatabase)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePathFromDatabase.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}