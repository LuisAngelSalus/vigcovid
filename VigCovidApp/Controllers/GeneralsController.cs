using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VigCovidApp.Controllers.Base;

namespace VigCovidApp.Controllers
{
    public class GeneralsController : GenericController
    {
        // GET: Generals
        public ActionResult SessionExpired()
        {
            return View();
        }

        public ActionResult UserUnknown()
        {
            return View();
        }
    }
}