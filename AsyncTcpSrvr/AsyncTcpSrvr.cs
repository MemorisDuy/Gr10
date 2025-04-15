using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AsyncTcpSrvr
{
    public partial class AsyncTcpSrvr : Form
    {
        private byte[] data = new byte[1024];
        private int size = 1024;
        private Socket server;
        public AsyncTcpSrvr()
        {
            InitializeComponent();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9050);
            server.Bind(iep);
            server.Listen(5);
            server.BeginAccept(new AsyncCallback(AcceptConn), server);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Close();

        }
        void AcceptConn(IAsyncResult iar)
        {
            Socket oldserver = (Socket)iar.AsyncState;
            Socket client = oldserver.EndAccept(iar);

            // Tiếp tục lắng nghe kết nối mới
            oldserver.BeginAccept(new AsyncCallback(AcceptConn), oldserver);

            // Xử lý client hiện tại
            if (client.Connected)
            {
                Invoke((MethodInvoker)delegate {
                    conStatus.Text = "Connected to: " + client.RemoteEndPoint.ToString();
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate {
                    conStatus.Text = "Connection failed";
                });
            }

            string stringData = "Welcome to my server";
            byte[] message1 = Encoding.ASCII.GetBytes(stringData);
            client.BeginSend(message1, 0, message1.Length, SocketFlags.None,
                        new AsyncCallback(SendData), client);
        }

        void SendData(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            int sent = client.EndSend(iar);
            client.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), client);
        }
        void ReceiveData(IAsyncResult iar)
        {
            Socket remote = (Socket)iar.AsyncState;
            try
            {
                int recv = remote.EndReceive(iar);
                if (recv == 0)
                {
                    // Kết nối đã đóng
                    return;
                }

                string stringData = Encoding.ASCII.GetString(data, 0, recv);
                Invoke((MethodInvoker)delegate {
                    results.Items.Add(stringData);
                });

                // Tiếp tục nhận dữ liệu mới
                remote.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
            }
        }
    }
}
