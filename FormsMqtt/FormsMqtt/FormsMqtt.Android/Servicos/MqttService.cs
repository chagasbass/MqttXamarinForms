using Android.Runtime;
using FormsMqtt.Droid.Servicos;
using FormsMqtt.Mqtt.Modelos;
using FormsMqtt.Mqtt.Servicos;
using System.Linq;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

[assembly: Xamarin.Forms.Dependency(typeof(MqttService))]
namespace FormsMqtt.Droid.Servicos
{
    /// <summary>
    /// implementação da interface do Servico de Mqtt
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MqttService : IMqttService
    {
        MqttClient Cliente { get; set; }
        RetornoMqtt Retorno { get; set; }

        /// <summary>
        /// Efetua a conexao com o broker informado
        /// </summary>
        /// <returns></returns>
        public bool Conectar()
        {
            Cliente = new MqttClient(InfoConexao.BrokerIp);
            Cliente.Connect(InfoConexao.ClienteId, InfoConexao.Usuario, InfoConexao.Senha);

            InicializarEventos();

            return Cliente.IsConnected;
        }


        /// <summary>
        /// desconecta do broker
        /// </summary>
        public void Desconectar() => Cliente.Disconnect();

        /// <summary>
        /// Efetua a publicacao de uma mensagem no broker
        /// </summary>
        /// <param name="topico">topico da publicacao</param>
        /// <param name="mensagem">payload da mensagem</param>
        public void PublicarMensagem(string topico, string mensagem)
        {
            var _mensagem = Encoding.UTF8.GetBytes(mensagem);

            Retorno.Topico = topico;
            Retorno.Mensagem = _mensagem;

            if (Cliente.IsConnected)
            {
                Retorno.MsgId = Cliente.Publish(topico,
                             _mensagem,
                             MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                             false);
            }
        }

        /// <summary>
        /// Efetua a inscrição em um determinado tópido do broker
        /// </summary>
        /// <param name="topicos">topicos a serem inscritos</param>
        public void SubscribeMensagem(string[] topicos)
        {
            Cliente.Subscribe(topicos,
                new byte[] {
                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
                });
        }

        /// <summary>
        /// Inicializa os eventos do mqtt
        /// </summary>
        private void InicializarEventos()
        {
            Cliente.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
            Cliente.MqttMsgSubscribed += MqttClient_MqttMsgSubscribed;
            Cliente.MqttMsgUnsubscribed += MqttClient_MqttMsgUnsubscribed;
            Cliente.MqttMsgPublished += MqttClient_MqttMsgPublished;

            Retorno = new RetornoMqtt();
        }

        #region Eventos

        /// <summary>
        /// Evento efetuado após publicação de mensagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (Retorno.MsgId == e.MessageId && e.IsPublished)
                Retorno.Publicado = true;
        }

        /// <summary>
        /// Evento efetuado apos a desassinatura de um topico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {

        }

        /// <summary>
        /// Evento efetuado após inscrição em um topico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {

        }


        /// <summary>
        /// Evento efetuado após o recebimento de uma mensagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Message.Any())
            {
                Retorno.Topico = e.Topic;
                Retorno.Mensagem = e.Message;
                Retorno.Publicado = true;
            }

            Xamarin.Forms.MessagingCenter.Send<RetornoMqtt>(Retorno, e.Topic);
        }

        #endregion
    }
}