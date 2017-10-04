using Xamarin.Forms.Internals;

namespace FormsMqtt.Mqtt.Modelos
{
    /// <summary>
    /// Modelo que guarda as informações para conexão ao broker
    /// </summary>
    /// 
    [Preserve(AllMembers = true)]
    public static class InfoConexao
    {
        public static string ClienteId = "Bl_Iot";
        public static int Porta = 1883;
        public static string BrokerIp = "test.mosquitto.org";
        public static string Usuario = "";
        public static string Senha = "";
    }
}
