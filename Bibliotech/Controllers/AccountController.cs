using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bibliotech.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignIn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            return View(new Tuple<Usuario, string>(null, Request.QueryString["ReturnUrl"]));
        }

        [HttpPost]
        public ActionResult SignIn(Usuario usuario)
        {
            Usuario teste = UsuarioRepository.Instance.RetornarPorId(new Guid("4E379975-EF88-411D-9AC6-87D6B883DEE3"));
            return null;
        }
    }
}