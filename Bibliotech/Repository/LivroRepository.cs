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
    public class LivroRepository : BaseRepository<Livro>
    {
        private static LivroRepository instance;

        private LivroRepository() { }

        public static LivroRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(LivroRepository))
                        if (instance == null)
                            instance = new LivroRepository();

                return instance;
            }
        }

        public List<Livro> GetListLivroByExample(Livro entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Livro));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Titulo != null)
                    criteria.Add(Restrictions.InsensitiveLike("Titulo", "%" + entity.Titulo + "%"));

                if (entity.Isbn != null)
                    criteria.Add(Restrictions.InsensitiveLike("Isbn", "%" + entity.Isbn + "%"));

                //Mudar consulta tratando períodos
                if (entity.AnoPublicao != null)
                    criteria.Add(Restrictions.Eq("AnoPublicacao", entity.AnoPublicao));

                if (entity.Edicao != null)
                    criteria.Add(Restrictions.Eq("Edicao", entity.Edicao));

                if (entity.Assunto != null)
                    criteria.Add(Restrictions.InsensitiveLike("Assunto", "%" + entity.Assunto + "%"));

                return criteria.List<Livro>().ToList();
            }
        }
    }
}