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
    public class LivroController : Controller
    {
        public ActionResult Listar(Livro exemplo, int page = 1)
        {
            var list = LivroRepository.Instance.GetListLivroByExample(exemplo)
                .OrderBy(x => x.Titulo)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Livro>,Livro>(list,exemplo));
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult Alterar(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);

            return View(livro);
        }

        [HttpPost]
        public JsonResult Salvar(Livro livro)
        {
            if (ModelState.IsValid)
            {
                if (!BLivro.Instance.ValidarSalvar(ref livro))
                {
                    return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    LivroRepository.Instance.SaveOrUpdate(livro);
                    return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                }

            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remover(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);
            if (!BLivro.Instance.ValidarRemover(ref livro))
            {
                return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
            }

            LivroRepository.Instance.Delete(livro);

            return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
        }
    }
}