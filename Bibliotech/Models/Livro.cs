using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Livro
    {
        public virtual Guid? Id { get; set; }

        public virtual string Titulo { get; set; }

        public virtual string Isbn { get; set; }

        public virtual int? AnoPublicao { get; set; }

        public virtual int? Edicao { get; set; }

        public virtual int? NumeroPaginas { get; set; }

        public virtual string Assunto { get; set; }

        public virtual string NomeArquivo { get; set; }

        public virtual byte[] Version { get; set; }
    }

    public class LivroMap : ClassMap<Livro>
    {
        public LivroMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.Titulo).Length(200).Not.Nullable();
            Map(x => x.Isbn).Length(15).Not.Nullable();
            Map(x => x.AnoPublicao);
            Map(x => x.Edicao);
            Map(x => x.NumeroPaginas);
            Map(x => x.Assunto).Length(200);
            Map(x => x.NomeArquivo).Length(300);

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }

    }
}