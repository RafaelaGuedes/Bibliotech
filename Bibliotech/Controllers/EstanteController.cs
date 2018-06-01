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
    public class EstanteController : Controller
    {
        public ActionResult Listar(Estante exemplo, int page = 1)
        {
            var list = EstanteRepository.Instance.GetListEstanteByExample(exemplo)
                .OrderBy(x => x.Descricao)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Estante>, Estante>(list, exemplo));
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
            Estante estante = EstanteRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View(estante);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult Salvar(Estante estante)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                if (ModelState.IsValid)
                {
                    if (!BEstante.Instance.ValidarSalvar(ref estante))
                    {
                        return Json(new { Status = BEstante.Instance.Status(), Message = BEstante.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    try
                    {
                        EstanteRepository.Instance.SaveOrUpdate(estante);
                        return Json(new { Status = BEstante.Instance.Status(), Message = BEstante.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
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
            Estante estante = EstanteRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                if (!BEstante.Instance.ValidarRemover(ref estante))
                {
                    return Json(new { Status = BEstante.Instance.Status(), Message = BEstante.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
                }

                EstanteRepository.Instance.Delete(estante);

                return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }
    }
}