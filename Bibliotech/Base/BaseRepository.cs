using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Base
{
    public class BaseRepository<T> where T : class
    {
        public virtual T GetById(object id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.SetMaxResults(1);
                criteria.Add(Restrictions.Eq("Id", id));
                var result = criteria.List<T>();

                T obj = null;

                if (result.Count > 0)
                    obj = result[0];

                return obj;
            }
        }

        public T GetByExample(T example)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<T> listExample = session.CreateCriteria(typeof(T)).Add(Example.Create(example))
                    .SetResultTransformer(new DistinctRootEntityResultTransformer()).List<T>().ToList();

                if (listExample.Count == 1)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        object value1 = property.GetValue(example);
                        object value2 = property.GetValue(listExample[0]);

                        if (value1 != null && value2 != null && value1.ToString().ToLower() == value2.ToString().ToLower())
                        {
                            return listExample[0];
                        }
                    }
                }
            }
            return default(T);
        }

        public virtual List<T> GetList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                List<T> list = criteria.List<T>().ToList();

                return list;
            }
        }

        public virtual List<T> GetPagedList(int? page, Int32 quantidade)
        {
            int pagina = page == null ? 1 : (int)page;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(T));
                List<T> list = criteria.List<T>().ToList();

                criteria.SetFirstResult(pagina).SetMaxResults(quantidade);

                return list;
            }
        }

        //public void Inserir(T entidade)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transacao = session.BeginTransaction())
        //        {
        //            try
        //            {
        //                session.Save(entidade);
        //                transacao.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (!transacao.WasCommitted)
        //                {
        //                    transacao.Rollback();
        //                }
        //                throw new Exception("Erro ao inserir Cliente : " + ex.Message);
        //            }
        //        }
        //    }
        //}

        //public void Alterar(T entidade)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transacao = session.BeginTransaction())
        //        {
        //            try
        //            {
        //                session.Update(entidade);
        //                transacao.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (!transacao.WasCommitted)
        //                {
        //                    transacao.Rollback();
        //                }
        //                throw new Exception("Erro ao Alterar Cliente : " + ex.Message);
        //            }
        //        }
        //    }
        //}

        //public void Excluir(T entidade)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transacao = session.BeginTransaction())
        //        {
        //            try
        //            {
        //                session.Delete(entidade);
        //                transacao.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (!transacao.WasCommitted)
        //                {
        //                    transacao.Rollback();
        //                }
        //                throw new Exception("Erro ao Excluir Cliente : " + ex.Message);
        //            }
        //        }
        //    }
        //}

        public virtual void SaveOrUpdate(T entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                BeforeCommitSaveOrUpdate(session, ref entity);
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public virtual void SaveOrUpdate(List<T> listEntity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                BeforeCommitSaveOrUpdate(session, ref listEntity);
                foreach (T entity in listEntity)
                {
                    T entityAux = entity;
                    BeforeCommitSaveOrUpdate(session, ref entityAux);
                    session.SaveOrUpdate(entityAux);
                }
                transaction.Commit();
            }
        }

        public virtual void BeforeCommitSaveOrUpdate(ISession session, ref List<T> list)
        {

        }

        public virtual void BeforeCommitSaveOrUpdate(ISession session, ref T entity)
        {

        }

        public virtual void BeforeCommitDelete(ISession session, ref T entity)
        {

        }

        public virtual void BeforeCommitDelete(ISession session, ref List<T> list)
        {

        }

        public virtual void Delete(T entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                BeforeCommitDelete(session, ref entity);
                session.Delete(entity);
                transaction.Commit();
            }
        }

        public void Delete(List<T> listEntity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (T entity in listEntity)
                {
                    session.Delete(entity);
                }

                transaction.Commit();
            }
        }

        public IList<T> Consultar()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return (from c in session.Query<T>() select c).ToList();
            }
        }
    }
}