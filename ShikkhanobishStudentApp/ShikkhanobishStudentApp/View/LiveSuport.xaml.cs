using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveSuport : ContentPage
    {
        public LiveSuport()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        
    }
}