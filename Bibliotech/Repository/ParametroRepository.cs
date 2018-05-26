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
    public class ParametroRepository : BaseRepository<Parametro>
    {
        private static ParametroRepository instance;

        private ParametroRepository() { }

        public static ParametroRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(ParametroRepository))
                        if (instance == null)
                            instance = new ParametroRepository();

                return instance;
            }
        }

        public override void BeforeCommitSaveOrUpdate(ISession session, ref Parametro entity)
        {
            entity.Senha = CriptografiaHelper.Encriptar(entity.Senha);
        }

        protected override void DoAfterGet(Parametro entity)
        {
            if (entity != null)
                entity.Senha = CriptografiaHelper.Decriptar(entity.Senha);
        }

        public List<Parametro> GetListUsuarioByExample(Parametro entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Parametro));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.DiasAlteracaoSenha != null)
                    criteria.Add(Restrictions.Eq("Dias de Alteração de Senha", entity.DiasAlteracaoSenha));

                if (entity.DiasPrazoDevolucao != null)
                    criteria.Add(Restrictions.Eq("Prazo de Devolução", entity.DiasPrazoDevolucao));

                if (entity.EmailRemetente != null)
                    criteria.Add(Restrictions.InsensitiveLike("Email", "%" + entity.EmailRemetente + "%"));

                if (entity.DiasPrazoReserva!= null)
                    criteria.Add(Restrictions.InsensitiveLike("Prazo de Reserva", "%" + entity.DiasPrazoReserva + "%"));

                if (entity.QuantidadeMaximaEmprestimo != null)
                    criteria.Add(Restrictions.InsensitiveLike("Quantidade de Emprestimo", "%" + entity.QuantidadeMaximaEmprestimo + "%"));

                return criteria.List<Parametro>().ToList();
            }
        }


        internal Parametro GetFirst(Parametro parametro)
        {
            throw new NotImplementedException();
        }

        public override void LazyProperties(Parametro entity)
        {
            
        }

        public virtual Parametro GetParametro()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Parametro));
                criteria.SetMaxResults(1);
                var result = criteria.List<Parametro>();

                Parametro obj = null;

                if (result.Count > 0)
                    obj = result[0];

                DoAfterGet(obj);

                return obj;
            }
        }
    }
}
