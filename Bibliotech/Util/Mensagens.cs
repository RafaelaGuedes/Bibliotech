using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Util
{
    public class Mensagens
    {
        public static string ADICIONADO_SUCESSO = "Adicionado com sucesso.";
        public static string ALTERADO_SUCESSO = "Alterado com sucesso.";
        public static string REMOVIDO_SUCESSO = "Removido com sucesso.";

        public static string ERRO_GENERICO = "Ocorreu um erro.";
        public static string ERRO_CONCORRENCIA = "Este registro foi modificado recentemente. Atualize a página e tente novamente.";

        public static string USUARIO_SENHA_INVALIDOS = "Usuário ou senha inválidos.";
    }
}