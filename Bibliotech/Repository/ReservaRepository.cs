using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class ReservaRepository : BaseRepository<Reserva>
    {
        private static ReservaRepository instance;

        private ReservaRepository() { }

        public static ReservaRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(ReservaRepository))
                        if (instance == null)
                            instance = new ReservaRepository();

                return instance;
            }
        }

        public override void LazyProperties(Reserva entity)
        {
            if (entity.Exemplar != null)
            {
                entity.Exemplar.ToString();

                if (entity.Exemplar.Livro != null)
                {
                    entity.Exemplar.Livro.ToString();

                    if (entity.Exemplar.Livro.Autor != null)
                        entity.Exemplar.Livro.Autor.ToString();

                    if (entity.Exemplar.Livro.Editora != null)
                        entity.Exemplar.Livro.Editora.ToString();

                    if (entity.Exemplar.Livro.Prateleira != null)
                        entity.Exemplar.Livro.Prateleira.ToString();
                }
            }
        }

        public List<Reserva> GetListReservasAtivasByExemplo(Reserva reserva, bool lazyProperties = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Reserva))
                    .Add(Restrictions.Gt("DataVencimento", DateTime.Now));

                if (!Functions.IsNull(reserva.Usuario))
                    criteria.Add(Restrictions.Eq("Usuario.Id", reserva.Usuario.Id));

                var list = criteria.List<Reserva>().ToList();

                if (lazyProperties && list != null)
                    foreach (var item in list)
                        LazyProperties(item);

                return list;
            }
        }
    }
}