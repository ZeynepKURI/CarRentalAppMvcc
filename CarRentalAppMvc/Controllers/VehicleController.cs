using CarRentalAppMvc.Data;
using CarRentalAppMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMvc.Controllers
{
    public class VehicleController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleController(AppDbContext context)
        {
            _context = context;
        }

        // Index Action - Araçları listele
        public IActionResult Index()
        {
            var vehicles = _context.Vehicles.ToList();
            return View(vehicles);
        }

        // Create Action - Yeni araç eklemek için
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LicensePlate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // Edit Action - Aracı düzenlemek için
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LicensePlate")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(vehicle);
        }

        // Delete Action - Araç silme işlemi (ajax)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
