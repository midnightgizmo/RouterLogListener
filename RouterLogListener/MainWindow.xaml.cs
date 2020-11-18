using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;
using System.Net.Sockets;
namespace RouterLogListener
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private udpListener.Client _Client;

        public MainWindow()
        {
            InitializeComponent();

            _Client = new udpListener.Client(this.Dispatcher);
            _Client.MessageRecieved += new udpListener.Client.MessageReceivedDelegate(_Client_MessageRecieved);
            //CreateUDPListener();
          
        }

        

        private void CreateUDPListener()
        {
            UdpClient client;
            IPEndPoint remoteIPEndPoint;
            byte[] byteMessage = null;

            client = new UdpClient(1524);
            remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                byteMessage = client.Receive(ref remoteIPEndPoint);
            }
            catch
            {
            }
            finally
            {
                client.Close();
            }


            string message = Encoding.ASCII.GetString(byteMessage);

            
        }

        private void cmdStartStop_Click(object sender, RoutedEventArgs e)
        {
            if ((string)cmdStartStop.Content == "Start")
            {
                cmdStartStop.Content = "Stop";
                _Client.StartListening();
            }
            else
            {
                cmdStartStop.Content = "Start";
                _Client.StopListening();
            }
        }

        void _Client_MessageRecieved(string message)
        {
            txtMessage.Text += "\r\n" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + message;

            
        }
        
    }
}
