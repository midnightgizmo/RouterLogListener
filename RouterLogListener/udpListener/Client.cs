using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace RouterLogListener.udpListener
{
    class Client
    {

        private UdpClient client;
        private bool CanListen = false;
        private System.Windows.Threading.Dispatcher _Dispatcher;

        public delegate void MessageReceivedDelegate(string message);
        public event MessageReceivedDelegate MessageRecieved;


        public Client(System.Windows.Threading.Dispatcher dispatcher)
        {
            _Dispatcher = dispatcher;
        }

        public void StartListening()
        {
            System.Threading.Thread ListenThread = new System.Threading.Thread(new System.Threading.ThreadStart(ListenLoop));
            CanListen = true;
            ListenThread.Start();

        }

        public void StopListening()
        {
            CanListen = false;
        }

        private void ListenLoop()
        {
            IPEndPoint remoteIPEndPoint;
            byte[] byteMessage = null;
            
            client = new UdpClient(1524);
            client.Client.ReceiveTimeout = 10000;
            
            remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (CanListen)
            {
                byteMessage = null;
                try
                {
                    byteMessage = client.Receive(ref remoteIPEndPoint);
                }
                catch
                {
                    int i = 0;
                }

                myObject holder = new myObject();
                holder.array = byteMessage;
                if (byteMessage != null && byteMessage.Length > 0)
                    SendMessage(holder);
            }
            try
            {
                client.Close();
            }
            catch
            { }
        }

        private delegate void DispatcherSwitcherDelegate(object MessageInBytes);
        private void SendMessage(object TheMessage)
        {
            if (!_Dispatcher.CheckAccess())
            {
                try
                {
                    _Dispatcher.Invoke(new DispatcherSwitcherDelegate(SendMessage), TheMessage);
                }
                catch(System.Exception e)
                {
                    int i = 0;
                }
                return;
            }

            string message = Encoding.ASCII.GetString(((myObject)TheMessage).array);

            if (MessageRecieved != null)
                MessageRecieved(message);
        }

        
    }

    public class myObject
    {
        public byte[] array;
    }
}
