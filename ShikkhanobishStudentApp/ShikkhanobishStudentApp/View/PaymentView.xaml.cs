using Microsoft.AspNetCore.SignalR.Client;
using ShikkhanobishStudentApp.Model;
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
    public partial class PaymentView : ContentPage
    {
        HubConnection _connection = null;
        string url = "https://shikkhanobishrealtimeapi.shikkhanobish.com/ShikkhanobishHub";
        public PaymentView(string url)
        {
            InitializeComponent();
            webview.Source = url;
            ConnectToRealTimeApiServer();
        }
        public async Task ConnectToRealTimeApiServer()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl(url)
                 .Build();
            await _connection.StartAsync();


            _connection.Closed += async (s) =>
            {
                await _connection.StartAsync();
            };

            
            _connection.On<int, bool, string, string,string,string,string,string>("StudentPaymentStatus", async (studentID, successFullPayment, amount, response, paymentID, trxID, cardID, cardType) =>
            {
                if (studentID == StaticPageToPassData.thisStudentInfo.studentID && StaticPageToPassData.LastPaymentRequestID != paymentID)
                {
                    StaticPageToPassData.LastPaymentRequestID = paymentID;
                    await _connection.StopAsync();
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                   
                }
            });

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}