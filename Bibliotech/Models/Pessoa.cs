using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Pessoa
    {
        public virtual Guid? Id { get; set; }

        public virtual string Nome { get; set; }

        public virtual DateTime? DataNascimento { get; set; }

        public virtual string Telefone { get; set; }

        public virtual Usuario Usuario { get; set; }
    }

    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Telefone).Length(15);
            Map(x => x.DataNascimento);

            HasOne<Usuario>(x => x.Usuario).Cascade.All().PropertyRef("Pessoa");
        }

    }
}