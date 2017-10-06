using FormsMqtt.Mqtt.Servicos;
using FormsMqtt.Servicos.Implemetacoes;
using FormsMqtt.Servicos.Interfaces;
using System;
using Xamarin.Forms;

namespace FormsMqtt
{
    public partial class App : Application
    {
        IMqttService Service;

        public App()
        {
            InitializeComponent();

            #region Registro de dependencias
            DependencyService.Register<IMessageService, MessageService>();
            Service = DependencyService.Get<IMqttService>();
            #endregion

            MainPage = new FormsMqtt.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }


        /// <summary>
        /// Quando o app for para background verifica se está conectado ao broker
        /// 
        /// </summary>
        protected override void OnSleep()
        {
            var minutes = TimeSpan.FromSeconds(10);

            Device.StartTimer(minutes, () => {

                if (!Service.VerificarConexao())
                {
                    Service.Conectar();
                }

                return true;
            });
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}