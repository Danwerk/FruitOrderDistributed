#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    public class CartProductsController : Controller
    {
        private readonly IAppUOW _uow;


        public CartProductsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: CartProducts
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _uow.CartProductRepository.AllAsync();
                return View(vm);
            }
            else
            {
                var vm = await _uow.CartProductRepository.AllAsync(User.GetUserId());
                return View(vm);
            }
        }

        // GET: CartProducts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartProduct = await _uow.CartProductRepository.FindAsync(id.Value);
            if (cartProduct == null)
            {
                return NotFound();
            }

            return View(cartProduct);
        }

        // GET: CartProducts/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_uow.CartRepository.AllAsync().Result, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description");
            return View();
        }

        // POST: CartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartProduct cartProduct)
        {
            if (ModelState.IsValid)
            {
                cartProduct.Id = Guid.NewGuid();
                _uow.CartProductRepository.Add(cartProduct);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CartId"] = new SelectList(_uow.CartRepository.AllAsync().Result, "Id", "Id", cartProduct.CartId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description",
                cartProduct.ProductId);
            return View(cartProduct);
        }

        // GET: CartProducts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartProduct = await _uow.CartProductRepository.FindAsync(id.Value);
            if (cartProduct == null)
            {
                return NotFound();
            }

            ViewData["CartId"] = new SelectList(_uow.CartRepository.AllAsync().Result, "Id", "Id", cartProduct.CartId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description",
                cartProduct.ProductId);
            return View(cartProduct);
        }

        // POST: CartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CartProduct cartProduct)
        {
            if (id != cartProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.CartProductRepository.Update(cartProduct);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartProductExists(cartProduct.Id))
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

            ViewData["CartId"] = new SelectList(_uow.CartRepository.AllAsync().Result, "Id", "Id", cartProduct.CartId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description",
                cartProduct.ProductId);
            return View(cartProduct);
        }

        // GET: CartProducts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartProduct = await _uow.CartProductRepository.FindAsync(id.Value);
            if (cartProduct == null)
            {
                return NotFound();
            }

            return View(cartProduct);
        }

        // POST: CartProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cartProduct = await _uow.CartProductRepository.FindAsync(id);
            if (cartProduct != null)
            {
                _uow.CartProductRepository.Remove(cartProduct);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartProductExists(Guid id)
        {
            return (_uow.CartProductRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}