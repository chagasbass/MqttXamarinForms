using FormsMqtt.Servicos.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace FormsMqtt.Servicos.Implemetacoes
{
    [Preserve(AllMembers = true)]
    public class MessageService : IMessageService
    {
        public async Task MostrarDialog(string titulo, string mensagem) => await App.Current.MainPage.DisplayAlert(titulo, mensagem, "OK");

        public async Task<bool> MostrarDialogComBotoes(string titulo, string mensagem)
        {
            return await App.Current.MainPage.DisplayAlert(titulo, mensagem, "SIM", "NÃO");
        }
    }
}