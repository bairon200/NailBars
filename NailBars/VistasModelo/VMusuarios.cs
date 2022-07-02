using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Firebase.Storage;
using NailBars.Modelo;
using NailBars.Servicios;
using NailBars.VistasModelo;


namespace NailBars.VistasModelo
{
    public class VMusuarios
    {
        List<MusuariosClientes> Usuarios = new List<MusuariosClientes>();
        string rutafoto;
        string Idusuario;

        public async Task<List<MusuariosClientes>> mostrar_usuarios()
        {
            var data = await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>();
            foreach (var rdr in data)
            {
                MusuariosClientes parametros = new MusuariosClientes();
                parametros.Id_usuario = rdr.Key;              
                parametros.Nombres = rdr.Object.Nombres;              
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;               
                parametros.Correo = rdr.Object.Correo;
                Usuarios.Add(parametros);

            }
            return Usuarios;
        }

        public async Task<string> insertar_usuario(MusuariosClientes parametros)
        {
            //child agregar o poder utilizar una tabla y PostAsync es para insertat datos a la tabla

            var data = await Conexionfirebase.firebase
                  .Child("UsuariosClientes")
                  .PostAsync(new MusuariosClientes()
                  {
                      Nombres = parametros.Nombres,                     
                      Pass = parametros.Pass,
                      Icono = parametros.Icono,
                      Correo = parametros.Correo

                  });
            Idusuario = data.Key;
            return Idusuario;
        }

        
        public async Task<string> SubirImagenesStorage(Stream ImagenStream, string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("nailbars-9dde3.appspot.com")
                .Child("UsuariosClientes")
                .Child(Idusuarios + ".jpg")
                .PutAsync(ImagenStream);
            rutafoto = dataAlmacenamiento;
            return rutafoto;
        }

        public async Task EditarFoto(MusuariosClientes parametros)
        {
            var obtenerData = (await Conexionfirebase.firebase
                .Child("UsuariosClientes") //comparamos si es la misma key
                .OnceAsync<MusuariosClientes>()).Where(a => a.Key == parametros.Id_usuario).FirstOrDefault();

            await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .Child(obtenerData.Key)
                .PutAsync(new MusuariosClientes()
                {
                    Nombres = parametros.Nombres,                 
                    Pass = parametros.Pass,
                    Icono = parametros.Icono,
                    Correo = parametros.Correo

                });
        }

        public async Task EliminarUsuarios(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OnceAsync<MusuariosClientes>()).Where((a) => a.Key == parametros.Id_usuario).FirstOrDefault();
            //eliminar
            await Conexionfirebase.firebase.Child("UsuariosClientes").Child(data.Key).DeleteAsync();
        }
        //eliminar la img
        public async Task EliminarImagen(string nombre)
        {
            await new FirebaseStorage("nailbars-9dde3.appspot.com")
                .Child("UsuariosClientes")
                .Child(nombre)
                .DeleteAsync();
        }

        public async Task<List<MusuariosClientes>> ObtenerDatosUsuarios(MusuariosClientes parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("UsuariosClientes")
                .OrderByKey()
                .OnceAsync<MusuariosClientes>()).Where(a => a.Key == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                parametros.Nombres = rdr.Object.Nombres;              
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Correo = rdr.Object.Correo;

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

    }
}
