#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly IAppUOW _uow;

        public DiscountsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: Discounts
        public async Task<IActionResult> Index()
        {
            var vm = await _uow.DiscountRepository.AllAsync();
            return View(vm);
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _uow.DiscountRepository.FindAsync(id.Value);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description");
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discount discount)
        {
            if (ModelState.IsValid)
            {
                discount.Id = Guid.NewGuid();
                _uow.DiscountRepository.Add(discount);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", discount.ProductId);
            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _uow.DiscountRepository.FindAsync(id.Value);
            if (discount == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", discount.ProductId);
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Discount discount)
        {
            if (id != discount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.DiscountRepository.Update(discount);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.Id))
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
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", discount.ProductId);
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _uow.DiscountRepository.FindAsync(id.Value);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var discount = await _uow.DiscountRepository.FindAsync(id);
            if (discount != null)
            {
                _uow.DiscountRepository.Remove(discount);
            }
            
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(Guid id)
        {
          return (_uow.DiscountRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
