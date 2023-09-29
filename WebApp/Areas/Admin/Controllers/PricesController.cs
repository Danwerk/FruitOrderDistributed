#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    public class PricesController : Controller
    {
        private readonly IAppUOW _uow;

        public PricesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: Prices
        public async Task<IActionResult> Index()
        {
            var vm = await _uow.PriceRepository.AllAsync();
            return View(vm);
        }

        // GET: Prices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _uow.PriceRepository.FindAsync(id.Value);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // GET: Prices/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Price price)
        {
            if (ModelState.IsValid)
            {
                price.Id = Guid.NewGuid();
                _uow.PriceRepository.Add(price);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductName"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Name", price.ProductId);
            return View(price);
        }

        // GET: Prices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _uow.PriceRepository.FindAsync(id.Value);
            if (price == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", price.ProductId);
            return View(price);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Price price)
        {
            if (id != price.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PriceRepository.Update(price);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", price.ProductId);
            return View(price);
        }

        // GET: Prices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _uow.PriceRepository.FindAsync(id.Value);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var price = await _uow.PriceRepository.FindAsync(id);
            if (price != null)
            {
                _uow.PriceRepository.Remove(price);
            }
            
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceExists(Guid id)
        {
          return (_uow.PriceRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
