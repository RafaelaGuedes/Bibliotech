using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Listar(int? page)
        {
            return View(UsuarioRepository.Instance.GetPagedList(page, 10));
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult Alterar(Guid id)
        {
            Usuario usuario = UsuarioRepository.Instance.GetById(id);

            return View(usuario);
        }

        [HttpPost]
        public JsonResult Salvar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioRepository.Instance.SaveOrUpdate(usuario);
                    return Json(new { Status = 0, Message = "salvo com sucesso" }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = 0, Message = "erro." }, JsonRequestBehavior.AllowGet);
                }

            }
            else
                return Json(new { Status = 0, Message = "erro." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remover(Guid id)
        {
            Usuario usuario = UsuarioRepository.Instance.GetById(id);
             
            UsuarioRepository.Instance.Delete(usuario);

            return Json(new { Status = 0, Message = "removido." }, JsonRequestBehavior.AllowGet);
        }
    }
}
