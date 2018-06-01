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
    public class EditoraController : Controller
    {
        public ActionResult Listar(Editora exemplo, int page = 1)
        {
            var list = EditoraRepository.Instance.GetListEditoraByExample(exemplo)
                .OrderBy(x => x.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Editora>, Editora>(list, exemplo));
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
            Editora Editora = EditoraRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View(Editora);
            else
                return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public JsonResult Salvar(Editora editora)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {            
                if (ModelState.IsValid)

                {
                    if (!BEditora.Instance.ValidarSalvar(ref editora))
                    {
                        return Json(new { Status = BEditora.Instance.Status(), Message = BEditora.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    try
                    {
                        EditoraRepository.Instance.SaveOrUpdate(editora);
                        return Json(new { Status = BEditora.Instance.Status(), Message = BEditora.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
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
            Editora editora = EditoraRepository.Instance.GetById(id);
            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
            {

                if (!BEditora.Instance.ValidarRemover(ref editora))
                {
                    return Json(new { Status = BEditora.Instance.Status(), Message = BEditora.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
                }

                EditoraRepository.Instance.Delete(editora);

                return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
            } 
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Buscar(Editora editora)
        {
            List<Editora> list = EditoraRepository.Instance.GetListEditoraByExample(editora).OrderBy(x => x.Nome).ToList();
            return Json(new { Lista = list.Select(x => new { Id = x.Id, Nome = x.Nome }) });
        }
    }
}