using Firebase.Auth;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroClientes : ContentPage
    {
        public RegistroClientes()
        {
            InitializeComponent();
        }

        MediaFile file;
        
        string rutafoto;
        string Idusuario;
        string estado;
        string EstadoImagen;

        private async void btnCrearcuenta_Clicked(object sender, EventArgs e)
        {
            if (EstadoImagen != "usuario.png")
            {
                if (!string.IsNullOrEmpty(txtNombres.Text))
                {
                    if (!string.IsNullOrEmpty(txtCorreo.Text))
                    {
                        if (!string.IsNullOrEmpty(txtContraseña.Text))
                        {                         

                            await InsertarUsuarios();
                            await SubirImagenesStore();
                            await EditarFoto();


                            Crearcienta();
                            IniciarSesion();
                            // ObtenerIdusuario();
                        }
                        else
                        {
                            await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");
                }
            }
            else
            {

                await DisplayAlert("Imagen Obligatoria", "LLenar los campos", "Ok");


            }

            }

    


        private async void btnagregarimagen_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                //para poder acceder a la galeria y agregar la img que queramos  NOTA: NIVEL LOCAL aun no se sube la imagen
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    EstadoImagen = "usuario.png";
                    return;
                }
                else
                {
                    imagenCelular.Source = ImageSource.FromStream(() =>
                    {
                        //GetStream extrae toda la ruta de la imagen
                        var rutaImagen = file.GetStream();

                        return rutaImagen;
                    });
                    EstadoImagen = "LLENO";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task SubirImagenesStore()
        {
            VMusuarios funcion = new VMusuarios();
            rutafoto = await funcion.SubirImagenesStorage(file.GetStream(), Idusuario);
        }

        private async Task EditarFoto()
        {
            VMusuarios funcion = new VMusuarios();
            MusuariosClientes parametros = new MusuariosClientes();

            parametros.Nombres = txtNombres.Text;          
            parametros.Correo = txtCorreo.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Icono = rutafoto;
            parametros.Id_usuario = Idusuario;

            await funcion.EditarFoto(parametros);
            await DisplayAlert("Listo", "Usuario Agregado", "OK");


        }

        private async Task InsertarUsuarios()
        {

            VMusuarios funcion = new VMusuarios();

            MusuariosClientes parametros = new MusuariosClientes();

            parametros.Nombres = txtNombres.Text;          
            parametros.Correo = txtCorreo.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Icono = "-";

            Idusuario = await funcion.insertar_usuario(parametros);
         



        }


        private void Crearcienta()
        {
            var funcion = new VMcrearcuenta();
            funcion.crearcuenta(txtCorreo.Text, txtContraseña.Text);
        }

        private void IniciarSesion()
        {
            var funcion = new VMcrearcuenta();
            funcion.crearcuenta(txtCorreo.Text, txtContraseña.Text);
        }

        private async void ObtenerIdusuario()
        {
            try
            {

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));

                //validar si el usuario se ha validado o no dentro de la aplicacion
                var guardarId = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));

                var RefrescarContenido = await authProvider.RefreshAuthAsync(guardarId);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefrescarContenido));
                //el ID
                Idusuario = guardarId.User.LocalId;



            }
            catch (Exception ex)
            {

                await DisplayAlert("Alerta", "Sesion expirada", "OK");

            }


        }



    }
}