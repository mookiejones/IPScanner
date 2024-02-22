using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPScanner.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace IPScanner.ViewModels
{


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="https://www.codeproject.com/Articles/10190/Implement-a-basic-IP-Scanner-for-a-local-LAN-in-C"/>
    public partial class MainViewModel:ObservableValidator
    {

        [ObservableProperty]
        private string? _from="From";

        [ObservableProperty]
        private string? _to="To";

        [ObservableProperty]
        private string? _host = "Host";


        public ObservableCollection<string> Results { get; set; } =new ObservableCollection<string>();

        private void Add(string message)
        {

            App.Current.Dispatcher.BeginInvoke(() =>
            {
                Results.Add(message);

            });
        }

        [ObservableProperty]
        private NetworkInterface? _selectedInterface;
        public ObservableCollection<NetworkInterface> NetworkInterfaces { get; set; } = new ObservableCollection<NetworkInterface>();

        public MainViewModel()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces()
               .Where(o => o.IsValid());
//                .WhereInternetwork();
               
                 
            

            foreach(var adapter in adapters)
            {

                Trace.WriteLine(adapter.GetType().Name);
                NetworkInterfaces.Add(adapter);

            }

            SelectedInterface=NetworkInterfaces.FirstOrDefault  ();
        }

        

        [RelayCommand]
        private void Use()
        {

            var props = SelectedInterface?.GetIPProperties();

            var unitCastAddresses = props.UnicastAddresses.
                Where(o => o.Address.IsInternetWork())
                .ToList();

            IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());

            var address = ip.AddressList
                .Where(o => o.IsInternetWork())
                .FirstOrDefault(o => unitCastAddresses.Any(u => u.Address.Equals(o)));
          

            From = address?.ToString();
            To = address?.ToString();

        }

        Thread sercher;
        [RelayCommand]
        private void Start()
        {
            sercher = new Thread(new ThreadStart(Find));
            Results.Clear();
            try
            {
                IPAddress from = IPAddress.Parse(From);
                IPAddress to = IPAddress.Parse(To);
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message);
                return;
            }
            sercher.Name = "Network searching thread";
            sercher.Start();
            Add("-->> >>> Please wait while processing is done <<< <<--");
            Thread.Sleep(1);// pune la culcare Threadul principal
        }

        void Find()
        {
            Add("-- >>> >> >>> Found station <<< << <<< -- ");
            int lastF = From.LastIndexOf(".");
            int lastT =To.LastIndexOf(".");
            string frm = From.Substring(lastF + 1);
            string tto = To.Substring(lastT + 1);
            int result = 0;
            System.Diagnostics.Debug.WriteLine(frm + " " + tto);
            for (int i = int.Parse(frm); i <= int.Parse(tto); i++)
            {
                try
                {
                    string address = To.Substring(0, lastT + 1);
                    System.Diagnostics.Debug.WriteLine(To.Substring(0, lastT + 1) + i);
                    IPHostEntry he = Dns.GetHostByAddress(address + i);
                    Add(he.HostName);
                    result += 1;
                }

                catch (SocketException)
                {
                    // in cazul unei erori
                }
                catch (Exception)
                {
                    // previne bloacarea programului
                }
            }
        }

        [RelayCommand]
        private void ClearResults()
        {
            Results.Clear();
        }
    }
}
