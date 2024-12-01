using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarRentalAppMvc.Models;
using CarRentalAppMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRentalAppMvc.ViewModel;

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
        CreateModel customerCreateModel = new CreateModel
        {
            // Yeni VehicleWorkingTime nesnesi oluşturuluyor
            VehicleWorkingTime = new VehicleWorkingTime(),

            // Vehicle listesini veritabanından alıyoruz
            Vehicle = _context.Vehicles
           .OrderBy(v => v.Name) // Araçları isme göre sıralıyoruz
           .Select(v => new SelectListItem
           {
               Value = v.Id.ToString(),  // Vehicle Id'sini 'Value'ya, aracın adını 'Text'ye atıyoruz
               Text = v.Name
           }).ToList(),
        };
        return View(customerCreateModel);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

