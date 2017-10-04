using Xamarin.Forms.Internals;

namespace FormsMqtt.Mqtt.Modelos
{
    /// <summary>
    /// Modelo responsável pelas informações de comunicação com o broker
    /// </summary>
    /// 
    [Preserve(AllMembers = true)]
    public class RetornoMqtt
    {
        public byte[] Mensagem { get; set; }
        public bool Publicado { get; set; }
        public string Topico { get; set; }
        public ushort MsgId { get; set; }
        public string MensagemRecuperada { get; set; }

        public RetornoMqtt()
        {

        }
    }
}