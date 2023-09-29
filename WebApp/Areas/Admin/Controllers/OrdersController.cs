#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUOW _uow;
        

        public OrdersController(UserManager<AppUser> userManager, IAppUOW uow)
        {
            _userManager = userManager;
            _uow = uow;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var vm = await _uow.OrderRepository.AllAsync();
                return View(vm);
            }
            else
            {
                var vm = await _uow.OrderRepository.AllAsync(User.GetUserId());
                return View(vm);
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var order = await _context.Orders
            //     .Include(o => o.AppUser)
            //     .Include(o => o.Payment)
            //     .FirstOrDefaultAsync(m => m.Id == id);
            
            var order = await _uow.OrderRepository.FindAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id), nameof(AppUser.Email));
            ViewData["PaymentId"] = new SelectList(_uow.PaymentRepository.AllAsync().Result, "Id", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            order.AppUserId = User.GetUserId();
            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid();
                _uow.OrderRepository.Add(order);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id), nameof(AppUser.Email), order.AppUserId);
            ViewData["PaymentId"] = new SelectList(_uow.PaymentRepository.AllAsync().Result, "Id", "Address", order.PaymentId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _uow.OrderRepository.FindAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id), nameof(AppUser.Email), order.AppUserId);
            ViewData["PaymentId"] = new SelectList(_uow.PaymentRepository.AllAsync().Result, "Id", "Address", order.PaymentId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.OrderRepository.Update(order);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id), nameof(AppUser.Email), order.AppUserId);
            ViewData["PaymentId"] = new SelectList(_uow.PaymentRepository.AllAsync().Result, "Id", "Address", order.PaymentId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _uow.OrderRepository.FindAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _uow.OrderRepository.FindAsync(id);
            if (order != null)
            {
                _uow.OrderRepository.Remove(order);
            }
            
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(Guid id)
        {
          return (_uow.OrderRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
