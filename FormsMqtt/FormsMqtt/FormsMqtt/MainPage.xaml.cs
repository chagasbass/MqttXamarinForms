using FormsMqtt.ViewModels;
using Xamarin.Forms;

namespace FormsMqtt
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel ViewModel
        {
            get { return BindingContext as MainViewModel; }
            set { BindingContext = value; }
        }

        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            BindingContext = ViewModel;
        }
    }
}
