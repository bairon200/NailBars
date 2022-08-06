using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailBars.Vistas;
using Acr.UserDialogs;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.VistasModelo;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            //esto es para ver si ya ha iniciado sesion que lo mande a la pagina principal
          

        }


        private async void btncrearUser_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroClientes());
        }

        private async void btniniciar_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtUsuercorreo.Text))
            {
                if (!string.IsNullOrEmpty(txtUserPassword.Text))
                {
                    UserDialogs.Instance.ShowLoading("Validando Usuario...");

                    await validarDatos();
                }
                else
                {
                    await DisplayAlert("Alerta", "Ingrese su contraseña", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Ingrese su correo", "OK");
            }


        }

        private async Task validarDatos()
        {
            try
            {
                var funcion = new VMcrearcuenta();
                await funcion.ValidarCuenta(txtUsuercorreo.Text, txtUserPassword.Text);

                UserDialogs.Instance.HideLoading();

                //esto es pq se van a convertir las paginas de toolvar en navigationpage
                Application.Current.MainPage = new NavigationPage(new Contenedor());

            }
            catch (Exception)
            {

                UserDialogs.Instance.HideLoading();

                await DisplayAlert("Alerta", "Correo o Contraseña incorrectos", "OK");
                await Navigation.PushAsync(new Login());

            }



        }





    }
}