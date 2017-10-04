using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace FormsMqtt.Servicos.Interfaces
{
    [Preserve(AllMembers = true)]
    public interface IMessageService
    {
        Task MostrarDialog(string titulo, string mensagem);
        Task<bool> MostrarDialogComBotoes(string titulo, string mensagem);
    }
}
