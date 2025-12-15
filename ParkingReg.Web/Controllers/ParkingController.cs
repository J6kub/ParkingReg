using System.Diagnostics;
using ParkingReg.Utils;
using Microsoft.AspNetCore.Mvc;
using ParkingReg.Web.Models;
using ParkingReg.Models;
using ParkingReg.Web.Utils;

namespace ParkingReg.Web.Controllers;

public class ParkingController : Controller
{
    [HttpPost]
    public IActionResult HandleEmail([FromForm] HandleEmailModel HEM)
    {
        string email = HEM.Email;
        SequelWhitelistEmail seq = new SequelWhitelistEmail();
        if(seq.CheckWhitelistEmail(email))
        {
            int emailId = seq.GetWhitelistEmailId(email);
            SequelVtk seqvtk = new SequelVtk();
            seqvtk.GenerateVtk(emailId);
            return Json(new GeneralResponse(true, "Email sent"));
        }
        return Json(new GeneralResponse(false, "Failed, invalid email"));
    }
    public IActionResult HandlePark([FromForm] ParkReq PR)
    {
        SequelParkReg seq = new SequelParkReg();
        if (!seq.HasValidParking(PR.Regnr))
        {
            SequelVtk seqvtk = new SequelVtk();
            Vtk vtk = seqvtk.GetVtkByToken(PR.Token);

            PR.EmailId = vtk.EmailId;
            SequelWhitelistEmail seqWLE = new SequelWhitelistEmail();
            string email = seqWLE.GetWhitelistEmail(PR.EmailId);
            seq.SubmitParking(PR);
            Parking prk = seq.GetParking(PR.Regnr);
            EmailHandler.SendEmail(email, "Park registered", $"Yo parking fo car {PR.Regnr} was registered");
            seqvtk.InvalidateVtks(PR.EmailId);
            return View("ParkingRegistered", prk);
        }
        else
        {
            return Json(new GeneralResponse(false, "Parking already exists for this vehicle"));
        }
    }
    
    [HttpGet]
    public IActionResult Index(string t = null, string regnr = null)
    {
        if (!string.IsNullOrEmpty(regnr) && !string.IsNullOrEmpty(t))
        {
            SequelVtk seqvtk = new SequelVtk();
            Vtk vtk = seqvtk.GetVtkByToken(t);
            SequelParkReg seq = new SequelParkReg();
            if (!seq.HasValidParking(regnr))
            {
                vtk.Regnr = regnr;
                return View("ChoosePark",vtk);
            } else
            {
                Parking park = seq.GetParking(regnr);
                return Json(park);
            }


        } else if (!string.IsNullOrEmpty(t))
        {
            SequelVtk seqvtk = new SequelVtk();
            Vtk vtk = seqvtk.GetVtkByToken(t);
            if (vtk == null)
            {
                return View();
            }
            else
            {
                return View("EnterReg", vtk);
            }
        } else
        {
            return View();
        }
            

    }
}
