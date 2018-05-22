using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Prateleira
    {
        public virtual Guid? Id { get; set; }

        public virtual string Descricao { get; set; }

        public virtual Estante Estante { get; set; }
        
    }
    public class PrateleiraMap : ClassMap<Prateleira>
    {
        public PrateleiraMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.Descricao).Length(200).Not.Nullable();

            References<Estante>(x => x.Estante).Not.Nullable();
        }
    }
}