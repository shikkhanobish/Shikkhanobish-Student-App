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
        /*
         * //Create actions
var actions = new string[]{ "Open in new tab", "Open in new window", "Copy link address", "Download link" };

//Show simple dialog
var result = await MaterialDialog.Instance.SelectActionAsync(actions: actions);

//Show simple dialog with title
var result = await MaterialDialog.Instance.SelectActionAsync(title: "Select an action", 
                                                             actions: actions);
        */
    }
}