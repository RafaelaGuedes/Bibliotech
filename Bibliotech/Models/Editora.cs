using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bibliotech.Models
{
    [Serializable]
    public class Editora
    {
        public virtual Guid? Id { get; set; }

        public virtual string Nome { get; set; }

        public virtual byte[] Version { get; set; }

        [XmlIgnore]
        public virtual IList<Livro> Livros { get; set; }
    }

    public class EditoraMap : ClassMap<Editora>
    {
        public EditoraMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();
            Map(x => x.Nome).Length(200).Not.Nullable();

            HasMany<Livro>(x => x.Livros).Cascade.All().Inverse().LazyLoad();

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }
    }
}