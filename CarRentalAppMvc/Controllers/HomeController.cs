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

    public IActionResult List()
    {
        // VehicleWorkingTime'ları ve ilişkili Vehicle verilerini alıyoruz
        List<VehicleWorkingTime> vehicleWorkingTimes = _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle) // Vehicle bilgilerini dahil et
            .ToList();

        return View(vehicleWorkingTimes);
    }
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

    [HttpGet]
    public JsonResult GetLicensePlates(string Name)
    {
        if (!string.IsNullOrWhiteSpace(Name))
        {
            // Veritabanından araç adına göre plaka bilgilerini getiren sorgu
            List<SelectListItem> licensePlates = _context.Vehicles
                .Where(v => v.Name == Name) // Araç adına göre filtreleme
                .OrderBy(v => v.LicensePlate)     // Plaka sırasına göre sıralama
                .Select(v => new SelectListItem
                {
                    Value = v.LicensePlate,     // Dropdown Value
                    Text = v.LicensePlate        // Dropdown Text
                })
                .ToList();

            return Json(licensePlates); // JSON formatında döndür
        }
        return Json(null);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

