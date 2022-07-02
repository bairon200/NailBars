using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using NailBars.Servicios;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace NailBars.VistasModelo
{
    public class VMcrearcuenta
    {
       

        public async void crearcuenta(string correo, string contraseña)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
            await authProvider.CreateUserWithEmailAndPasswordAsync(correo, contraseña);

        }

        public async void ValidarCuenta(string correo, string contraseña)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Conexionfirebase.WebapyFirebase));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(correo, contraseña);

            var contenido = await auth.GetFreshAuthAsync(); 
            var serializacion = JsonConvert.SerializeObject(contenido);

            Preferences.Set("MyFirebaseRefreshToken", serializacion);


        }

        

    }
}
