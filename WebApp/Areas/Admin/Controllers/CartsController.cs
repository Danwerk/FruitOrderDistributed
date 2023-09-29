#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly IAppUOW _uow;
        private readonly UserManager<AppUser> _userManager;

        public CartsController(UserManager<AppUser> userManager, IAppUOW uow)
        {
            _userManager = userManager;
            _uow = uow;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            // Lets use our CartRepository
            if (User.IsInRole("admin"))
            {
                var vm = await _uow.CartRepository.AllAsync();
                return View(vm);
            }
            else
            {
                var vm = await _uow.CartRepository.AllAsync(User.GetUserId());
                return View(vm);
            }

        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lets use our CartRepository
            var cart = await _uow.CartRepository.FindAsync(id.Value);

            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            // ViewData["AppUserId"] = new SelectList(_userManager.Users,
            //     nameof(AppUser.Id),
            //     nameof(AppUser.Email));
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart)
        {

            cart.AppUserId = User.GetUserId();
            if (ModelState.IsValid)
            {
                cart.Id = Guid.NewGuid();
                _uow.CartRepository.Add(cart);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id), nameof(AppUser.Email),
            //     cart.AppUserId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _uow.CartRepository.FindAsync(id.Value);
            if (cart == null)
            {
                return NotFound();
            }

            // ViewData["AppUserId"] = new SelectList(_userManager.Users, "Id", "Id", cart.AppUserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cart cart)
        {
            // todo: check the ownership before edit!!!! 
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                cart.AppUserId = User.GetUserId();
                _uow.CartRepository.Update(cart);
                await _uow.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            // ViewData["AppUserId"] = new SelectList(_userManager.Users, nameof(AppUser.Id),
                // nameof(AppUser.Email), cart.AppUserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _uow.CartRepository.FindAsync(id.Value);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cart = await _uow.CartRepository.FindAsync(id);
            if (cart != null)
            {
                _uow.CartRepository.Remove(cart);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(Guid id)
        {
          return (_uow.CartRepository.AllAsync().Result?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}