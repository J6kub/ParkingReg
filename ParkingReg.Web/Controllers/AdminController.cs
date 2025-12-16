using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ParkingReg.Models;
using ParkingReg.Utils;
using ParkingReg.Web.Models;
using ParkingReg.Web.Utils;
using System.Diagnostics;

namespace ParkingReg.Web.Controllers;

public class AdminController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult EmailManager() { return View(); }

    [HttpGet]
    public IActionResult Parkings() { return View(); }
    [HttpGet]
    public IActionResult ParkingCheck() { return View("ParkingLookup"); }

    [HttpGet]
    public IActionResult GetParkings() { return View(); }
    [HttpGet]
    public IActionResult GetActiveParkings()
    {
        List<ParkingExtended> Parkings = new List<ParkingExtended>();
        SequelAdmin seq = new SequelAdmin();
        try
        {
            Parkings = seq.GetActiveParkings();
            return Json(new GeneralResponse(true, "good", Parkings));
        }
        catch (Exception e)
        {
            return Json(new GeneralResponse(false, e.Message));
        }

    }
    [HttpGet]
    public IActionResult ParkingLookup(string regnr)
    {
        SequelAdmin seq = new SequelAdmin();
        try
        {
            ParkingExtended Parking = seq.LookUpParking(regnr);
            if (Parking == null)
            {
                return Json(new GeneralResponse(false, "No valid parking"));
            }
            return Json(new GeneralResponse(true, "good", Parking));
        }
        catch (Exception e)
        {
            return Json(new GeneralResponse(false, e.Message));
        }

    }

    [HttpPost]
    public IActionResult AddEmail([FromBody] WhiteListEmailModel WLEM) 
    {
        SequelAdmin seq = new SequelAdmin();
        try
        {
            seq.AddWhiteListedMail(WLEM);
            return Json(new GeneralResponse(true, "Email added"));
        }
        catch (Exception ex)
        {
            return Json(new GeneralResponse(false, ex.Message));
        }
    }


    
}
