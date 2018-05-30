using FluentNHibernate.Mapping;
using System;

namespace Bibliotech.Models
{
    [Serializable]
    public class Reserva
    {
        public virtual Guid? Id { get; set; }

        public virtual DateTime? DataReserva { get; set; }

        public virtual DateTime? DataVencimento { get; set; }

        public virtual Exemplar Exemplar { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual byte[] Version { get; set; }

        public class ReservaMap : ClassMap<Reserva>
        {
            public ReservaMap()
            {
                Id(x => x.Id).GeneratedBy.GuidNative();
                Map(x => x.DataReserva).Not.Nullable();
                Map(x => x.DataVencimento).Not.Nullable();

                References<Exemplar>(x => x.Exemplar).Not.Nullable();
                References<Usuario>(x => x.Usuario).Not.Nullable();

                Version(x => x.Version)
                    .Nullable()
                    .CustomSqlType("timestamp")
                    .Generated.Always();
            }
        }
    }
}