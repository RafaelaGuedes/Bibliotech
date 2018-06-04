﻿using Bibliotech.Base;
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
    public class ParametroController : Controller
    {
        
        public ActionResult Manter()
        {
            Parametro parametro = ParametroRepository.Instance.GetParametro() ?? new Parametro();
            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View(parametro);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult Salvar(Parametro parametro)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {

                if (ModelState.IsValid)
                {
                    if (!BParametro.Instance.ValidarSalvar(ref parametro))
                    {
                        return Json(new { Status = BParametro.Instance.Status(), Message = BParametro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    try
                    {
                        ParametroRepository.Instance.SaveOrUpdate(parametro);
                        return Json(new { Status = BParametro.Instance.Status(), Message = BParametro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
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
    }
}