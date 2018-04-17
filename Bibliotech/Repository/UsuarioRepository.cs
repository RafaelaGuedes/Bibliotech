using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        private static UsuarioRepository instance;

        private UsuarioRepository() { }

        public static UsuarioRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(UsuarioRepository))
                        if (instance == null)
                            instance = new UsuarioRepository();

                return instance;
            }
        }

        public bool ValidarLogin(string login)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return (from e in session.Query<Usuario>() where e.Login.Equals(login) select e).Count() > 0;
            }
        }
        public bool ValidarAcesso(string login, string senha)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return (from e in session.Query<Usuario>() where e.Login.Equals(login) && e.Senha.Equals(senha) select e).Count() > 0;
            }
        }
    }
}