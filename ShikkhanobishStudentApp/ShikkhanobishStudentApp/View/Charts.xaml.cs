using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;
using Microcharts;
using ShikkhanobishStudentApp.ViewModel;

namespace ShikkhanobishStudentApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Charts : ContentPage
    {
        List<Entry> entries = new List<Entry>
        {
            new Entry(5)
            {
                Color=SKColor.Parse("#F0140E"),
                Label="January",
                ValueLabel="5"
            },
            new Entry(10)
            {
                Color=SKColor.Parse("#D0E218"),
                Label="February",
                ValueLabel="10"
            },
            new Entry(-4)
            {
                Color=SKColor.Parse("#18E245"),
                Label="March",
                ValueLabel="-4"
            }
        };

        List<Entry> entries2 = new List<Entry>
        {
            new Entry(-5)
            {
                Color=SKColor.Parse("#F0140E"),
                Label="January",
                ValueLabel="-5"
            },
            new Entry(10)
            {
                Color=SKColor.Parse("#D0E218"),
                Label="February",
                ValueLabel="10"
            },
            new Entry(4)
            {
                Color=SKColor.Parse("#18E245"),
                Label="March",
                ValueLabel="4"
            }
        };
        public Charts()
        {
            InitializeComponent();
            BindingContext = new ChartsViewModel();
            NavigationPage.SetHasNavigationBar(this, false);

            Chart1.Chart = new RadialGaugeChart() { Entries = entries };
            Chart2.Chart = new LineChart() { Entries = entries };
            Chart3.Chart = new LineChart() { Entries = entries2 };
            Chart4.Chart = new PieChart() { Entries = entries };
        }
    }
}