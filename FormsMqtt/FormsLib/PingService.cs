using System.Net.NetworkInformation;

namespace FormsLib
{
    public class PingService
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
                var m = pex.Message;
                return false;
            }
        }
    }
}
