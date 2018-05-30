using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    [Authorize]
    public class PrateleiraController : Controller
    {
        [HttpPost]
        public JsonResult Buscar(Prateleira prateleira)
        {
            List<Prateleira> list = PrateleiraRepository.Instance.GetListPrateleiraByExample(prateleira).OrderBy(x => x.Descricao).ToList();
            return Json(new { Lista = list.Select(x => new { Id = x.Id, Descricao = x.Descricao, EstanteDescricao = x.Estante.Descricao }) });
        }
    }
}