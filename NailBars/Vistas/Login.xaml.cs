using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailBars.Vistas;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

       

        private async void btncrearUser_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroClientes());
        }
    }
}