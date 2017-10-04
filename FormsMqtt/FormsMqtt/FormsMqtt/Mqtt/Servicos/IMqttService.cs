using Xamarin.Forms.Internals;

namespace FormsMqtt.Mqtt.Servicos
{
    /// <summary>
    /// Interface de servicos para Acesso ao broker Mqtt
    /// </summary>
    /// 
    [Preserve(AllMembers = true)]
    public interface IMqttService
    {
        bool Conectar();
        void Desconectar();
        void PublicarMensagem(string topico, string mensagem);
        void SubscribeMensagem(string[] topicos);
    }
}