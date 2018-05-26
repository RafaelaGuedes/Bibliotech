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


        public override void LazyProperties(Parametro entity)
        {
            
        }

        
    }
}
