using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VigCovid.Security;
using VigCovidApp.Models;
using VigCovidApp.ViewModels;

namespace VigCovidApp.Controllers
{
    public class AccessSystemController : Controller
    {
        // GET: AccessSystem
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ListarEmpresas();
            return View();
        }

        public void ListarEmpresas()
        {
            ViewBag.EMPRESAS = new Empresas().ListarEmpresas();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            try {
                var empresasAsignadas = ValidateUser(model.Username.Trim().ToLower(), model.Password).ToList();
                    if (empresasAsignadas.Count() > 1)
                    {
                        var sessionModel = new SessionModel();
                        sessionModel.UserName = model.Username;
                        //sessionModel.EmpresasAsignadas = empresasAsignadas;
                        FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);

                        Security.HttpSessionContext.SetAccount(sessionModel);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                    ListarEmpresas();
                    ModelState.AddModelError("", "Contraseña o identificador de usuario incorrectos. Escriba la contraseña y el identificador de usuario correctos e inténtelo de nuevo.");
                        return View(model);
                    }
            }
            catch (Exception ex)
            {
                ListarEmpresas();
                ModelState.AddModelError("", "Error al procesar la solicitud.");
                return RedirectToAction("Login", "AccessSystem");
            }
        }

        private List<EmpresaSedeViewModel> ValidateUser(string username, string password)
        {
            var oAccessBL = new AccessBL();
            var usuario = oAccessBL.ValidarUsuario(username, password);
            if (usuario)
            {
                //OBETENER LISTADO DE EMPRESAS CON SEDES
            }
            else
            {
                return null;
            }
            return null;
            //return new Usuarios(username, password).ValidarUsuario();

        }

        [AllowAnonymous]
        public ActionResult UserUnknown()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            return SignOff();
        }

        private ActionResult SignOff()
        {
            var rolUsuario = string.Empty;
            var userData = Security.HttpSessionContext.CurrentAccount();
            if (userData != null)
            {
                rolUsuario = userData.UserName;
            }

            var urlSignOut = string.Empty;

            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Session.Clear();


            urlSignOut = string.Format("{0}", FormsAuthentication.LoginUrl);


            return Redirect(urlSignOut);
        }


        [AllowAnonymous]
        public ActionResult SessionExpired(string returnUrl)
        {
            AsignarUrlRetorno(returnUrl);
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Generals");
        }

        protected virtual void AsignarUrlRetorno(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }
        }
    }
}