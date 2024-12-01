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
                Value = v.Id.ToString(),
                Text = v.Name
            }),
            VehicleLicensePlates = vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.LicensePlate
            })
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(CreateModel vw)
    {
        // IdleTime'ı hesapla
        vw.VehicleWorkingTime.IdleTime = (7 * 24) - (vw.VehicleWorkingTime.ActiveWorkTime + vw.VehicleWorkingTime.MaintenanceTime);

        // VehicleWorkingTime nesnesini veritabanına ekle
        _context.Add(vw.VehicleWorkingTime);

        // Veritabanına kaydet
        _context.SaveChanges();

        // Başarıyla kaydedildikten sonra listeleme sayfasına yönlendir
        return RedirectToAction("List");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

