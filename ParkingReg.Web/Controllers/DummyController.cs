using Microsoft.AspNetCore.Mvc;
using ParkingReg.Models;
using ParkingReg.Web.Models;
using ParkingReg.Web.Utils;
using System.Diagnostics;

namespace ParkingReg.Web.Controllers;

public class DummyController : Controller
{


    public IActionResult Index()
    {
        return View();
    }
    public IActionResult GetEmails()
    {
        try
        {
            List<EmailModel> emails = EmailHandler.GetEmails();

            return Json(new GeneralResponse(true, "emails", emails));
        } catch (Exception E)
        {
            return Json(new GeneralResponse(false, "emails", E.Message));
        }
        
    }


    
}
