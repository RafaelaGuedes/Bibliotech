﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Parametro
    {
        public virtual Guid? Id { get; set; }

        [Display(Name = "Valor da Multa")]
        public virtual decimal ValorMultaAtraso { get; set; }

        [Display(Name = "Dias de Alteração de Senha")]
        public virtual int? DiasAlteracaoSenha { get; set; }

        [Display(Name = "Prazo de devolução")]
        public virtual int? DiasPrazoDevolucao { get; set; }

        [Display(Name = "Prazo de Reserva")]
        public virtual int? DiasPrazoReserva { get; set; }

        [Display(Name = "Quantidade de Empréstimo")]
        public virtual int? QuantidadeMaximaEmprestimo { get; set; }

        public virtual byte[] Version { get; set; }

    }

    public class ParametroMap : ClassMap<Parametro>
    {
        public ParametroMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.ValorMultaAtraso);
            Map(x => x.DiasAlteracaoSenha);
            Map(x => x.DiasPrazoDevolucao);
            Map(x => x.DiasPrazoReserva);
            Map(x => x.QuantidadeMaximaEmprestimo);

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }

    }
}