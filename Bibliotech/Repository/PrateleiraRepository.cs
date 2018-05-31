using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class PrateleiraRepository : BaseRepository<Prateleira>
    {

        private static PrateleiraRepository instance;

        private PrateleiraRepository() { }

        public static PrateleiraRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(PrateleiraRepository))
                        if (instance == null)
                            instance = new PrateleiraRepository();

                return instance;
            }
        }

        public List<Prateleira> GetListPrateleiraByExample(Prateleira entity, bool lazyProperties = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Prateleira));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Descricao != null)
                    criteria.Add(Restrictions.InsensitiveLike("Descricao", "%" + entity.Descricao + "%"));

                var list = criteria.List<Prateleira>().ToList();

                if (lazyProperties && list != null)
                    foreach(var item in list)
                        LazyProperties(item);

                return list;
            }
        }

        public override void LazyProperties(Prateleira entity)
        {
            if (entity.Estante != null)
                entity.Estante.ToString();
        }
    }
}

