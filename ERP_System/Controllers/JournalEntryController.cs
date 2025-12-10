using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP_System.Services.Interfaces;
using ERP_System.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_System.Controllers
{
    [Authorize]
    public class JournalEntryController : Controller
    {
        private readonly IJournalEntryService _journalService;

        public JournalEntryController(IJournalEntryService journalService)
        {
            _journalService = journalService;
        }

        public async Task<IActionResult> List()
        {
            var entries = await _journalService.GetAllAsync();
            return View(entries);
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Accounts = await _journalService.GetLeafAccountsAsync();
            return View(new JournalEntryVm());
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] JournalEntryVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vm.Details.Sum(d => d.Debit) != vm.Details.Sum(d => d.Credit))
            {
                return BadRequest("إجمالي المدين يجب أن يساوي إجمالي الدائن.");
            }

            // Implement user ID fetching if available, e.g. from Claims
            // int? userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 

            var id = await _journalService.AddAsync(vm);

            return Ok(new { id = id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var entry = await _journalService.GetByIdAsync(id);
            if (entry == null) return NotFound();
            return View(entry);
        }
    }
}
