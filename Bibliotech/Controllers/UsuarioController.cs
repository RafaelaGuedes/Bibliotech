using Bibliotech.Base;
using Bibliotech.Business;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using NReco.ImageGenerator;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Bibliotech.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        public ActionResult Listar(Usuario exemplo, int page = 1)
        {
            var list = UsuarioRepository.Instance.GetListUsuarioByExample(exemplo)
                .OrderBy(x => x.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Usuario>, Usuario>(list, exemplo));
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
                if (!BUsuario.Instance.ValidarSalvar(ref usuario))
                {
                    return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    UsuarioRepository.Instance.SaveOrUpdate(usuario);
                    return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
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
            Usuario usuario = UsuarioRepository.Instance.GetById(id);
            if (!BUsuario.Instance.ValidarRemover(ref usuario))
            {
                return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
            }

            UsuarioRepository.Instance.Delete(usuario);

            return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarCartaoUsuario(Guid id)
        {
            string conteudo = FormatarCartaoUsuario(UsuarioRepository.Instance.GetById(id));

            MemoryStream ms = Functions.ConvertHtmlToImage(conteudo, ImageFormat.Png, "--width 500 --height 150");
            ViewBag.ImageSrc = Functions.GetPngImageSrc(ms);

            return View();
        }

        private string FormatarCartaoUsuario(Usuario usuario)
        {
            string conteudo =
                "<html>" +
                "<head>" +
                    "<meta charset='UTF-8'>" +
                "</head>" +
                "<body>" +
                    "<table border='1' width='500px' height='150px' style='margin-top: -8; margin-left: -8; border-collapse: collapse; font-family: Franklin Gothic Medium, Arial Narrow, Arial, sans-serif'>" +
                        "<tr>" +
                            "<td width='150px' rowspan='3' style='text-align: center'>" +
                                "<img src='[QRCODE]' width='130px' height='130px' />" +
                            "</td>" +
                            "<td>[NOME]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[LOGIN]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[DATA DE NASCIMENTO]</td>" +
                        "</tr>" +
                    "</table>" +
                "</body>" +
                "</html>";

            conteudo = conteudo.Replace("[NOME]", usuario.Nome);
            conteudo = conteudo.Replace("[LOGIN]", usuario.Login);
            conteudo = conteudo.Replace("[DATA DE NASCIMENTO]", usuario.DataNascimento.Value.ToString("dd/MM/yyyy"));
            conteudo = conteudo.Replace("[QRCODE]", Functions.GetPngImageSrc(Functions.GenerateQRCode(usuario.Id.ToString(), 130, 130)));

            return conteudo;
        }
    }


}
