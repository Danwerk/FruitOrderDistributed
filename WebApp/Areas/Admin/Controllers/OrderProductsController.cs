#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using Base.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    public class OrderProductsController : Controller
    {
        private readonly IAppUOW _uow;

        public OrderProductsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _uow.OrderProductRepository.AllAsync();
                return View(vm);
            }
            else
            {
                var vm = await _uow.OrderProductRepository.AllAsync(User.GetUserId());
                return View(vm);
            }
            
            
            
        }

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _uow.OrderProductRepository.FindAsync(id.Value);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // GET: OrderProducts/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_uow.OrderRepository.AllAsync().Result, "Id", "OrderNr");
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description");
            return View();
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                orderProduct.Id = Guid.NewGuid();
                _uow.OrderProductRepository.Add(orderProduct);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_uow.OrderRepository.AllAsync().Result, "Id", "OrderNr", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _uow.OrderProductRepository.FindAsync(id.Value);
            if (orderProduct == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_uow.OrderRepository.AllAsync().Result, "Id", "OrderNr", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderProduct orderProduct)
        {
            if (id != orderProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.OrderProductRepository.Update(orderProduct);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProductExists(orderProduct.Id))
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
            ViewData["OrderId"] = new SelectList(_uow.OrderRepository.AllAsync().Result, "Id", "OrderNr", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_uow.ProductRepository.AllAsync().Result, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderProduct = await _uow.OrderProductRepository.FindAsync(id.Value);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // POST: OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orderProduct = await _uow.OrderProductRepository.FindAsync(id);
            if (orderProduct != null)
            {
                _uow.OrderProductRepository.Remove(orderProduct);
            }
            
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderProductExists(Guid id)
        {
          return (_uow.OrderProductRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
