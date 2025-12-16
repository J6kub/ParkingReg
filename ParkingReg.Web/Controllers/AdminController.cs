using Microsoft.AspNetCore.Mvc;
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
