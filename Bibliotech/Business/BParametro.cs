using Bibliotech.Base;
using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Bibliotech.Business
{
    public class BParametro : BaseBusiness<Parametro>
    {

        private static BParametro instance;

        private BParametro()
        {

        }

        public static BParametro Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(BParametro))
                        if (instance == null)
                            instance = new BParametro();

                return instance;
            }
        }

        public override bool ValidarRemover(ref Parametro parametro)
        {
            return true;
        }

        public override bool ValidarSalvar(ref Parametro parametro)
        {
            SetMensagemSalvar(parametro);
            SetStatusSucesso();
            return true;
        }

        public void EnvioEmail(Emprestimo emprestimo)
        {
            Parametro parametro = ParametroRepository.Instance.GetParametro();

            string conteudo = "";

           
            conteudo += "Livro: " + livro.Titulo + " < br /> ";
            conteudo += "Seu empréstimo foi realizado" + emprestimo.DataInicio + " <br />";
            conteudo += "Data de entrega: " + emprestimo.DataFimPrevisao + " < br /> ";


            //Envia o e-mail
            MailMessage mail = new MailMessage(parametro.EmailRemetente, usuario.Email);
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            mail.Subject = "Comprovante de Empréstimo";
            mail.Body = conteudo;
            mail.IsBodyHtml = true;

            client.Credentials = new System.Net.NetworkCredential(parametro.EmailRemetente, parametro.Senha);
            client.Send(mail);


        }
    }
}