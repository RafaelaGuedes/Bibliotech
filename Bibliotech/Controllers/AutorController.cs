using Bibliotech.Base;
using Bibliotech.Business;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    [Authorize]
    public class AutorController : Controller
    {
        public ActionResult Listar(Autor exemplo, int page = 1)
        {
            var list = AutorRepository.Instance.GetListAutorByExample(exemplo)
                .OrderBy(x => x.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Autor>, Autor>(list, exemplo));
        }

        public ActionResult Adicionar()
        {
            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Alterar(Guid id)
        {
            Autor Autor = AutorRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View(Autor);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult Salvar(Autor autor)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                if (ModelState.IsValid)
                {
                    if (!BAutor.Instance.ValidarSalvar(ref autor))
                    {
                        return Json(new { Status = BAutor.Instance.Status(), Message = BAutor.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    try
                    {
                        AutorRepository.Instance.SaveOrUpdate(autor);
                        return Json(new { Status = BAutor.Instance.Status(), Message = BAutor.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    catch (NHibernate.StaleStateException dbcx)
                    {
                        return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Remover(Guid id)
        {
            Autor autor = AutorRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
            {

                if (!BAutor.Instance.ValidarRemover(ref autor))
                {
                    return Json(new { Status = BAutor.Instance.Status(), Message = BAutor.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
                }

                AutorRepository.Instance.Delete(autor);
                return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Buscar(Autor autor)
        {
            List<Autor> list = AutorRepository.Instance.GetListAutorByExample(autor).OrderBy(x => x.Nome).ToList();
            return Json(new { Lista = list.Select(x => new { Id = x.Id, Nome = x.Nome }) });
        }
    }

}
