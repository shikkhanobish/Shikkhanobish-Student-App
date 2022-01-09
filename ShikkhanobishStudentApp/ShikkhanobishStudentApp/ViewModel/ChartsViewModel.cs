using ShikkhanobishStudentApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;
using Microcharts;
using ShikkhanobishStudentApp.View;

namespace ShikkhanobishStudentApp.ViewModel
{
    class ChartsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChartsViewModel()
        {
            //Chart1.Chart = new RadialGaugeChart() { Entries = entries };
        }

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


    }
}
