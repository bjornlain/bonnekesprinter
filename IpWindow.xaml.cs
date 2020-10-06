using ESCPOS_NET;

using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using WPFCore.Models;

namespace WPFCore
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class IpWindow : Window
    {
        private AuthResponse _authResponse;
        private static BasePrinter printer;
        private string ipAdres;
        private static ICommandEmitter epson;
        private static readonly HttpClient client = new HttpClient();
        private static System.Timers.Timer aTimer;
        private static List<string> printedCustomerIds ;
        private static Restaurant thisRestaurant;
        public IpWindow(AuthResponse authResponse)
        {
            printedCustomerIds = new List<string>();
            _authResponse = authResponse;
            getPrinterUrl();
            InitializeComponent();
        }

        private async void getPrinterUrl()
        {
            var networkPort = 9100;

            epson = new EPSON();
            // printer.StartMonitoring();
            //Setup(true);


            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost:4100/api/v1/restaurants.getById");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authResponse.token.access_token);
            client.DefaultRequestHeaders.Add("restaurantid", _authResponse.restaurantid);

            var response = await client.SendAsync(httpRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            var restaurantResponse = JsonConvert.DeserializeObject<Restaurant>(responseString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            thisRestaurant = restaurantResponse;
            thisRestaurant._id = _authResponse.restaurantid;
            ipAdresBlock.Text = "Het ip-adres van uw printer is :" + restaurantResponse.printerUrl;
            ipAdresBox.Text = restaurantResponse.printerUrl;

            
        }
        private void StartPrinter(object sender, RoutedEventArgs e)
        {
            printer = new NetworkPrinter(ipAddress: ipAdresBox.Text, port: 9100, reconnectOnTimeout: true);
            SetTimer();
            printerStatus.Text = "Online";
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#008000");
            printerStatus.Foreground = brush;
            MessageBox.Show("Printer is gestart");
        }
            private async void Button_Click(object sender, RoutedEventArgs e)
        {
     
            thisRestaurant.printerUrl = ipAdresBox.Text;
            var JsonRestaurant = JsonConvert.SerializeObject(thisRestaurant);
            var content = new StringContent(JsonRestaurant, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("http://localhost:4100/api/v1/restaurants.update", content);


            var responseString = await result.Content.ReadAsStringAsync();
            var restaurantResponse = JsonConvert.DeserializeObject<Restaurant>(responseString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            ipAdresBlock.Text = "Het ip-adres van uw printer is :" + restaurantResponse.printerUrl;
            ipAdresBox.Text = restaurantResponse.printerUrl;
           // printer = new NetworkPrinter(ipAddress: restaurantResponse.printerUrl, port: networkPort, reconnectOnTimeout: true);
           
            MessageBox.Show("Opgeslaan");
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(5000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost:4100/api/v1/customers.getAllCustomers");

            var response = await client.SendAsync(httpRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<Customer[]>(responseString, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            List<Customer> customerArray = new List<Customer>();
          

            for (var i = 0; i < authResponse.Length; i++)
            {

                if (authResponse[i].orders.Length > 0)
                {
                    if (!printedCustomerIds.Contains(authResponse[i]._id))
                    {
                        customerArray.Add(authResponse[i]);
                    }
                   
                }
             }
            for (var i = 0; i < customerArray.Count; i++)
            {
                printedCustomerIds.Add(customerArray[i]._id);
                printer.Write(Printing(epson, customerArray[i]));
                for (var j = 0; j < customerArray[i].orders.Length; j++)
                {
                    var fixedLength = 21;
                    var productName = customerArray[i].orders[j].product.name;
                    if (productName.Length < 21)
                    {
                        for (var k = productName.Length; k < fixedLength; k++)
                        {
                            customerArray[i].orders[j].product.name = customerArray[i].orders[j].product.name.Insert(k, " ");
                        }
                    } else
                    {
                        var indexOfLastSpace = customerArray[i].orders[j].product.name.IndexOf(" ", 14);
                        var firstLine = customerArray[i].orders[j].product.name.Substring(0,indexOfLastSpace);
                        

                          for (var l = firstLine.Length; l < fixedLength; l++)
                            {
                            firstLine = firstLine.Insert(l, " ");
                            }
                          //first normaal gezien oke
                        var secondLine = customerArray[i].orders[j].product.name.Substring(indexOfLastSpace + 1);
                        var lol = 0;
                    }
                    
                    printer.Write(PrintingOrder(epson, customerArray[i].orders[j]));
                    for(var y = 0; y < customerArray[i].orders[j].chosenExtra.Length; y++)
                    {
                        if(customerArray[i].orders[j].chosenExtra[y].price > 0)
                        {
                            printer.Write(PrintingExtra(epson, customerArray[i].orders[j].chosenExtra[y]));
                        } else
                        {
                            printer.Write(PrintingExtraWithoutPrice(epson, customerArray[i].orders[j].chosenExtra[y]));
                        }
                       
                    }
               
                }
                printer.Write(PrintingCut(epson));
            }

           
            Console.WriteLine(authResponse);
            Console.WriteLine(response);
            Console.WriteLine(responseString);


        }
        private static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            Console.WriteLine($"Printer Online Status: {status.IsCoverOpen}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }

        private static bool _hasEnabledStatusMonitoring = false;
        private static void Setup(bool enableStatusBackMonitoring)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(epson.Initialize());
                printer?.Write(epson.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(epson.EnableAutomaticStatusBack());
                }
            }
        }


        public static byte[] Printing(ICommandEmitter e, Customer customer) =>

               ByteSplicer.Combine(
                   e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Bold | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth),
                   e.CenterAlign(),
                   e.Print(customer.restaurantid.name + "\n"),
                   e.SetStyles(PrintStyle.FontB),
                   e.Print(customer.restaurantid.street + " " + customer.restaurantid.streetnumber + "\n"),
                   e.Print(customer.restaurantid.city + "  " + customer.restaurantid.website + "\n"),
                   e.PrintLine(" "),
                   //TODO: sanitize test.
                   e.SetStyles(PrintStyle.None),
                   e.Print("Tafelnummer " + customer.table.tablenumber + " | datum:" + customer.time.Date.ToShortDateString() + " | uur: " + customer.time.Hour + ":" + customer.time.Minute),
                   e.PrintLine(" ")
                   );

        public static byte[] PrintingOrder(ICommandEmitter e, Order order) =>
             ByteSplicer.Combine(
                   e.LeftAlign(),
                   e.PrintLine(""),
                   e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth),
                   e.PrintLine(order.amount + "X" + "  " + order.product.name + " " + (char)164 + " " + order.product.price + "\n")
                    
                );

        public static byte[] PrintingOrderMultipleLines(ICommandEmitter e, Order order, string firstLine, string secondLine) =>
            ByteSplicer.Combine(
                  e.LeftAlign(),
                  e.PrintLine(""),
                  e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth),
                  e.PrintLine(order.amount + "X" + "  " + order.product.name + " " + (char)164 + " " + order.product.price + "\n")

               );

        public static byte[] PrintingTest(ICommandEmitter e, int count) =>
             ByteSplicer.Combine(
                   e.LeftAlign(),
                   e.PrintLine((char)count + " " + count)
                );
        public static byte[] PrintingExtra(ICommandEmitter e, ChosenExtra chosenExtra) =>
            ByteSplicer.Combine(
                  e.LeftAlign(),
                  e.SetStyles(PrintStyle.None),
                  e.Print( "   " + chosenExtra.name + " : " + chosenExtra.option.name + " " + (char)164 + " " +  chosenExtra.option.price + "\n" )
               );

        public static byte[] PrintingExtraWithoutPrice(ICommandEmitter e, ChosenExtra chosenExtra) =>
           ByteSplicer.Combine(
                 e.LeftAlign(),
                 e.SetStyles(PrintStyle.None),
                 e.Print("   " + chosenExtra.name + " : " + chosenExtra.option.name + "\n")
              );
        public static byte[] PrintingCut(ICommandEmitter e) =>
            ByteSplicer.Combine(
                  e.CenterAlign(),
                  e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight),
                  e.FeedLines(3),
                  e.PrintLine("DIT IS GEEN GELDIG BTW-KASTICKET"),
                  e.FeedLines(3),
                  e.FullCut()
               );
    }
}
