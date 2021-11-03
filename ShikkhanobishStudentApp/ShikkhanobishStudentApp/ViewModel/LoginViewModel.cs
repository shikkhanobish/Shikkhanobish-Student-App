using ShikkhanobishStudentApp.Model;
using ShikkhanobishStudentApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShikkhanobishStudentApp.ViewModel
{
    public class LoginViewModel: BaseViewModel, INotifyPropertyChanged
    {
        public ICommand goRegisterView =>
             new Command(() =>
             {
                 Application.Current.MainPage.Navigation.PushAsync(new ResgisterView());
             });
    }
}
