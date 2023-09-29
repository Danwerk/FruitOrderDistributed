#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    // [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly IAppUOW _uow;

        public ProductsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var vm = await _uow.ProductRepository.AllAsync();
            if (User.IsInRole("admin"))
            {
                ViewData["ShowCreateLink"] = true;
            }
            else
            {
                ViewData["ShowCreateLink"] = false;
            }
            return View(vm);
            
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _uow.ProductRepository.FindAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_uow.ProductTypeRepository.AllAsync().Result, "Id", "Name");
            ViewData["UnitId"] = new SelectList(_uow.UnitRepository.AllAsync().Result, "Id", "UnitName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                _uow.ProductRepository.Add(product);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_uow.ProductTypeRepository.AllAsync().Result, "Id", "Name", product.ProductTypeId);
            ViewData["UnitId"] = new SelectList(_uow.UnitRepository.AllAsync().Result, "Id", "UnitName", product.UnitId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _uow.ProductRepository.FindAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_uow.ProductTypeRepository.AllAsync().Result, "Id", "Name", product.ProductTypeId);
            ViewData["UnitId"] = new SelectList(_uow.UnitRepository.AllAsync().Result, "Id", "UnitName", product.UnitId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.ProductRepository.Update(product);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(_uow.ProductTypeRepository.AllAsync().Result, "Id", "Name", product.ProductTypeId);
            ViewData["UnitId"] = new SelectList(_uow.UnitRepository.AllAsync().Result, "Id", "UnitName", product.UnitId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _uow.ProductRepository.FindAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _uow.ProductRepository.FindAsync(id);
            if (product != null)
            {
                _uow.ProductRepository.Remove(product);
            }
            
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
          return (_uow.ProductRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
