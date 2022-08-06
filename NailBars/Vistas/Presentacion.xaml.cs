﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NailBars.Vistas.TutorialIntro;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Presentacion : ContentPage
    {
        public Presentacion()
        {
            InitializeComponent();
           Animacion();
            
        }

        public async Task Animacion()
        {
            logo.Opacity = 0;
            await logo.FadeTo(1, 2000);
            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
              
                Application.Current.MainPage = new NavigationPage(new Contenedor());
            }
            else
            {
                Application.Current.MainPage = new Intro1();

            }

        }

    }
}