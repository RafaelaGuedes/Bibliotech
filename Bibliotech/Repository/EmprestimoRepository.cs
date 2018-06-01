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
    public class EmprestimoRepository : BaseRepository<Emprestimo>
    {
        private static EmprestimoRepository instance;

        private EmprestimoRepository() { }

        public static EmprestimoRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(EmprestimoRepository))
                        if (instance == null)
                            instance = new EmprestimoRepository();

                return instance;
            }
        }

        public override void LazyProperties(Emprestimo entity)
        {
            if(entity.Exemplar != null)
            {
                entity.Exemplar.ToString();

                if (entity.Exemplar.Livro != null)
                    entity.Exemplar.Livro.ToString();
            }

            if (entity.Usuario != null)
                entity.Usuario.ToString();
        }

        //Salva novo empréstimo e atualiza exemplar na mesma transação
        public virtual void SaveOrUpdateEmprestimoUpdateExemplar(Emprestimo emprestimo, Exemplar exemplar)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Clear();

                BeforeCommitSaveOrUpdate(session, ref emprestimo);
                session.SaveOrUpdate(emprestimo);

                ExemplarRepository.Instance.BeforeCommitSaveOrUpdate(session, ref exemplar);
                session.SaveOrUpdate(exemplar);

                transaction.Commit();
            }
        }

        public List<Emprestimo> GetListEmprestimosAtivosByExemplo(Emprestimo emprestimo, bool lazyProperties = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Emprestimo))
                    .Add(Restrictions.IsNull("DataFim"));

                if (!Functions.IsNull(emprestimo.Usuario))
                    criteria.Add(Restrictions.Eq("Usuario.Id", emprestimo.Usuario.Id));

                var list = criteria.List<Emprestimo>().ToList();

                if (lazyProperties && list != null)
                    foreach (var item in list)
                        LazyProperties(item);

                return list;
            }
        }

        public List<Emprestimo> GetListEmprestimosAtrasadosByExemplo(Emprestimo emprestimo, bool lazyProperties = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Emprestimo))
                    .Add(Restrictions.IsNull("DataFim"))
                    .Add(Restrictions.Lt("DataFimPrevisao", DateTime.Now));

                if (!Functions.IsNull(emprestimo.Usuario))
                    criteria.Add(Restrictions.Eq("Usuario.Id", emprestimo.Usuario.Id));

                var list = criteria.List<Emprestimo>().ToList();

                if (lazyProperties && list != null)
                    foreach (var item in list)
                        LazyProperties(item);

                return list;
            }
        }

        public List<Emprestimo> GetListEmprestimosAnterioresByExemplo(Emprestimo emprestimo, bool lazyProperties = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Emprestimo))
                    .Add(Restrictions.IsNotNull("DataFim"));

                if (!Functions.IsNull(emprestimo.Usuario))
                    criteria.Add(Restrictions.Eq("Usuario.Id", emprestimo.Usuario.Id));

                var list = criteria.List<Emprestimo>().ToList();

                if (lazyProperties && list != null)
                    foreach (var item in list)
                        LazyProperties(item);

                return list;
            }
        }
    }
}