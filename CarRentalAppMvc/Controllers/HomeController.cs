using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarRentalAppMvc.Models;
using CarRentalAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRentalAppMvc.ViewModel;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;

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
       
        List<VehicleWorkingTime> vehicleWorkingTimes = _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle) 
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
    
        var existingVehicle = _context.Vehicles
            .FirstOrDefault(v => v.Name == vw.VehicleWorkingTime.Vehicle.Name && v.LicensePlate == vw.VehicleWorkingTime.Vehicle.LicensePlate);

        if (existingVehicle != null)
        {
           
            vw.VehicleWorkingTime.Vehicle = existingVehicle; 
                                                            
            var totalWeekTime = 7 * 24;

          
            var idleTime = totalWeekTime - (vw.VehicleWorkingTime.ActiveWorkTime + vw.VehicleWorkingTime.MaintenanceTime);


         
            _context.vehicleWorkingTimes.Add(vw.VehicleWorkingTime);
            _context.SaveChanges();

            return RedirectToAction();
        }
        else
        {
           
            ModelState.AddModelError("", "Bu araç veritabanında mevcut.");
            return View(vw);  
        }
    }



    public async Task<IActionResult> ViewVehicles()
    {
     
        var vehicleWorkingTimes = await _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle) 
            .ToListAsync();

        var activeWorkTimeData = new List<float>();
        var idleTimeData = new List<float>();
        var vehicleNames = new List<string>();

        foreach (var vehicle in vehicleWorkingTimes)
        {
            if (vehicle.Vehicle != null) 
            {
              
                vehicleNames.Add(vehicle.Vehicle.Name);

                activeWorkTimeData.Add(vehicle.ActiveWorkTime);
                idleTimeData.Add(vehicle.IdleTime);
            }
            else
            {
                
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

    [HttpGet]
    public IActionResult Delete(int id)
    {
     
        var vehicleWorkingTime = _context.vehicleWorkingTimes
            .Include(vwt => vwt.Vehicle) 
            .FirstOrDefault(vwt => vwt.Id == id);

        if (vehicleWorkingTime == null)
        {
            return NotFound();  
        }

        return View(vehicleWorkingTime);  
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        // VehicleWorkingTime'ı id'ye göre alıyoruz
        var vehicleWorkingTime = _context.vehicleWorkingTimes
            .FirstOrDefault(vwt => vwt.Id == id);

        if (vehicleWorkingTime == null)
        {
            return NotFound(); 
        }

      
        _context.vehicleWorkingTimes.Remove(vehicleWorkingTime);
        _context.SaveChanges();

      
        return RedirectToAction(nameof(List));
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

