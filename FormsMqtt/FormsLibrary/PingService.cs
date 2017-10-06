using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace FormsLibrary
{
   public  class PingService
    {
        public bool VerificarSeDispositivoEstaAtivo(string ip)
        {
            try
            {
                Ping ping = new Ping();

                PingReply reply = ping.Send(ip);

                return reply.Status == IPStatus.Success;
            }
            catch (PingException pex)
            {
                GerenciadorDeNotificacoes.InserirNotificacoes(null, "Problemas ao verificar a conexão com o Dispositivo, tente novamente mais tarde");
                return false;
            }
        }
    }
}
