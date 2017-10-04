using FormsMqtt.Mqtt.Modelos;
using FormsMqtt.Mqtt.Servicos;
using FormsMqtt.Servicos.Interfaces;
using Plugin.Connectivity;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System;

namespace FormsMqtt.ViewModels
{
    /// <summary>
    /// ViewModel da Main
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MainViewModel : BaseViewModel
    {
        public ICommand AbrirPortaUmCommand { get; private set; }
        public ICommand AbrirPortaDoisCommand { get; private set; }
        public ICommand AbrirPortaTresCommand { get; private set; }
        public ICommand DesconectarBrokerCommand { get; private set; }

        readonly IMqttService MqttService;
        readonly IMessageService MessageService;

        RetornoMqtt _RetornoMqtt;
        bool _EstaDisponivel;

        #region propriedades

        string[] topicos = { "dispositivo/ip" };

        public RetornoMqtt RetornoMqtt
        {
            get { return _RetornoMqtt; }
            set
            {
                SetValue(ref _RetornoMqtt, value);
                OnPropertyChanged(nameof(_RetornoMqtt));
            }
        }

        public bool EstaDisponivel
        {
            get { return _EstaDisponivel; }
            set
            {
                SetValue(ref _EstaDisponivel, value);
                OnPropertyChanged(nameof(_EstaDisponivel));
            }
        }

        #endregion

        public ICommand RecuperarIpDeDispositivoCommand { get; private set; }

        public MainViewModel()
        {
            MqttService = DependencyService.Get<IMqttService>();
            MessageService = DependencyService.Get<IMessageService>();

            AbrirPortaUmCommand = new Command(AbrirPortaUm);
            AbrirPortaDoisCommand = new Command(AbrirPortaDois);
            AbrirPortaTresCommand = new Command(AbrirPortaTres);
            DesconectarBrokerCommand = new Command(Desconectar);

            Inicializar();
        }

        /// <summary>
        /// Efetua a desconexão com o broker
        /// </summary>
        private void Desconectar()=> MqttService.Desconectar();

        /// <summary>
        /// inicializa a conexão com o broker
        /// </summary>
        private async void Inicializar()
        {
            if (!MqttService.Conectar())
            {
                await MessageService.MostrarDialog("Atenção", "Conexão não efetuada, tente novamente mais tarde");
                return;
            }

            MessagingCenter.Subscribe<RetornoMqtt>(this, "dispositivo/ip", (t) => RecuperarMensagens(t));
            MqttService.SubscribeMensagem(topicos);
        }

        /// <summary>
        /// Comando do botao da interface
        /// </summary>
        private async void AbrirPortaUm()
        {
            VerificarSeDispositivoEstaDisponivel();

            if (!EstaDisponivel)
            {
                await MessageService.MostrarDialog("Atenção", "Dispositivo indisponível no momento");
                return;
            }

            MqttService.PublicarMensagem("sala/dispositivo", "ligado");
            await MessageService.MostrarDialog("Atenção", "Acesso efetuado");
        }

        /// <summary>
        /// Comando do botao da interface
        /// </summary>
        private async void AbrirPortaDois()
        {
            try
            {
                VerificarSeDispositivoEstaDisponivel();

                if (!EstaDisponivel)
                {
                    await MessageService.MostrarDialog("Atenção", "Dispositivo indisponível no momento");
                    return;
                }

                MqttService.PublicarMensagem("quarto/dispositivo", "ligado");
                await MessageService.MostrarDialog("Atenção", "Acesso efetuado");
            }
            catch (System.Exception ex)
            {
                var m = ex.Message;
            }
        }

        /// <summary>
        /// Comando do botao da interface
        /// </summary>
        private async void AbrirPortaTres()
        {
            VerificarSeDispositivoEstaDisponivel();

            if (!EstaDisponivel)
            {
                await MessageService.MostrarDialog("Atenção", "Dispositivo indisponível no momento");
                return;
            }

            MqttService.PublicarMensagem("cozinha/dispositivo", "ligado");
            await MessageService.MostrarDialog("Atenção", "Acesso efetuado");
        }

        /// <summary>
        /// Recupera as mensagens do topico inscrito
        /// </summary>
        /// <param name="retorno"></param>
        /// <returns></returns>
        private RetornoMqtt RecuperarMensagens(RetornoMqtt retorno)
        {
            var mensagem = Encoding.UTF8.GetString(retorno.Mensagem, 0, retorno.Mensagem.Length);

            retorno.MensagemRecuperada = mensagem;

            RetornoMqtt = retorno;

            return retorno;
        }

        /// <summary>
        /// Verifica se o dispositivo está disponível pelo ip informado
        /// </summary>
        /// <returns></returns>
        public async void VerificarSeDispositivoEstaDisponivel()
        {
            try
            {
                var conectividade = CrossConnectivity.Current;
                if (conectividade.IsConnected)
                {
                    EstaDisponivel = await conectividade.IsRemoteReachable(_RetornoMqtt.MensagemRecuperada);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                var m = ex.Message;
                EstaDisponivel = false;
            }
        }
    }
}
