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
    public class EstanteRepository : BaseRepository<Estante>
    {
        private static EstanteRepository instance;

        private EstanteRepository() { }

        public static EstanteRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(EstanteRepository))
                        if (instance == null)
                            instance = new EstanteRepository();

                return instance;
            }
        }
        public List<Estante> GetListEstanteByExample(Estante entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Estante));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Descricao != null)
                    criteria.Add(Restrictions.InsensitiveLike("Descricao", "%" + entity.Descricao + "%"));

               
              

                return criteria.List<Estante>().ToList();
            }
        }
    }
}