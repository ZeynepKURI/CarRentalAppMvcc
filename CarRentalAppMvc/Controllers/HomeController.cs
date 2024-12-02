using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarRentalAppMvc.Models;
using CarRentalAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRentalAppMvc.ViewModel;
using System;

namespace CarRentalAppMvc.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }


    // Create Action
    public IActionResult VehicleCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VehicleCreate([Bind("Id,Name,LicensePlate")] Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Success redirect to Index or List page
        }
        return View(vehicle);
    }





[HttpGet]
    public IActionResult List()
    {
        // VehicleWorkingTime'ları ve ilişkili Vehicle verilerini alıyoruz
        List<VehicleWorkingTime> vehicleWorkingTimes = _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle) // Vehicle bilgilerini dahil et
            .ToList();

        return View(vehicleWorkingTimes);
    }


    [HttpGet]
    public ActionResult Create()
    {
        var vehicles = _context.Vehicles.ToList();

        var model = new CreateModel
        {
            VehicleWorkingTime = new VehicleWorkingTime(),
            VehicleNames = vehicles.Select(v => new SelectListItem
            {
                Value = v.Name,
                Text = v.Name
            }),
            VehicleLicensePlates = vehicles.Select(v => new SelectListItem
            {
                Value = v.LicensePlate,
                Text = v.LicensePlate
            })
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(CreateModel vw)
    {
        var idleTime = (7 * 24) - (vw.VehicleWorkingTime.ActiveWorkTime + vw.VehicleWorkingTime.MaintenanceTime);

        // Hesaplanan idleTime değerini VehicleWorkingTime objesine ekliyoruz
       vw.VehicleWorkingTime.IdleTime = idleTime;
        // VehicleWorkingTime nesnesini veritabanına ekle
        _context.Add(vw.VehicleWorkingTime);

        // Veritabanına kaydet
        _context.SaveChanges();

        // Başarıyla kaydedildikten sonra listeleme sayfasına yönlendir
        return RedirectToAction("List");
    }




    public async Task<IActionResult> ViewVehicles()
    {
        // VehicleWorkingTime ile ilişkili Vehicle verisini de yükle
        var vehicleWorkingTimes = await _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle)  // Vehicle navigasyon özelliğini dahil et
            .ToListAsync();

        var activeWorkTimeData = new List<float>();
        var idleTimeData = new List<float>();
        var vehicleNames = new List<string>();

        foreach (var vehicle in vehicleWorkingTimes)
        {
            if (vehicle.Vehicle != null) // Vehicle verisi null değilse
            {
                // Araç ismini al
                vehicleNames.Add(vehicle.Vehicle.Name);

                // Aktif çalışma süresi ve boşta bekleme süresi hesapla
                activeWorkTimeData.Add(vehicle.ActiveWorkTime);
                idleTimeData.Add(vehicle.IdleTime);
            }
            else
            {
                // Eğer Vehicle null ise, hata mesajı ekle veya farklı işlem yap
                vehicleNames.Add("Bilinmeyen Araç");
                activeWorkTimeData.Add(0);
                idleTimeData.Add(0);
            }
        }

        // Bu verileri View'a gönder
        ViewBag.VehicleNames = vehicleNames;
        ViewBag.ActiveWorkTimeData = activeWorkTimeData;
        ViewBag.IdleTimeData = idleTimeData;

        return View();
    }










    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

