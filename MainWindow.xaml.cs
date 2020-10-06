using System.Windows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using WPFCore.Models;


namespace WPFCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var values = new Dictionary<string, string>
                {
                    { "email", "ricky@solvengo.be" },
                    { "password", "Ricky2020" }
                };
            
            /* var values = new Dictionary<string, string>
                {
                    { "email", emailBox.Text},
                    { "password", passwordBox.Password}
                };
            */
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://localhost:4100/api/v1/auth.login", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseString);
            Console.WriteLine(authResponse);
            Console.WriteLine(response);
            Console.WriteLine(responseString);
            if(authResponse == null)
            {
                fouteLoginText.Text = "Uw email en/of wachtwoord is fout, probeer opnieuw.";
            } 
            else
            {
                var ipWindow = new IpWindow(authResponse);
                ipWindow.Show();
                this.Close();
            }
        }
    }
}
