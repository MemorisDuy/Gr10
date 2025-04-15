using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AsyncTcpClient
{
    public partial class AsyncTcpClient : Form
    {
        private Socket client;
        private byte[] data = new byte[1024];
        private int size = 1024;
        public AsyncTcpClient()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            conStatus.Text = "Connecting...";
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            newsock.BeginConnect(iep, new AsyncCallback(Connected), newsock);

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] message = Encoding.ASCII.GetBytes(newText.Text);
            newText.Clear();
            client.BeginSend(message, 0, message.Length, SocketFlags.None,new AsyncCallback(SendData), client);

        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            client.Close();
            conStatus.Text = "Disconnected";
        }
        void Connected(IAsyncResult iar)
        {
            client = (Socket)iar.AsyncState;
            try
            {
                client.EndConnect(iar);
                if (client.Connected)  // Kiểm tra kết nối thành công
                {
                    Invoke((MethodInvoker)delegate {
                        conStatus.Text = "Connected to: " + client.RemoteEndPoint.ToString();
                    });
                    client.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), client);
                }
                else
                {
                    Invoke((MethodInvoker)delegate {
                        conStatus.Text = "Connection failed";
                    });
                }
            }
            catch (SocketException ex)
            {
                Invoke((MethodInvoker)delegate {
                    conStatus.Text = "Error connecting: " + ex.Message;
                });
            }
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
                    Invoke((MethodInvoker)delegate {
                        conStatus.Text = "Disconnected";
                    });
                    return;
                }

                string stringData = Encoding.ASCII.GetString(data, 0, recv);

                // Sử dụng Invoke để cập nhật UI từ luồng chính
                Invoke((MethodInvoker)delegate {
                    results.Items.Add(stringData);
                });

                // Tiếp tục nhận dữ liệu mới
                remote.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
            }
            catch (ObjectDisposedException)
            {
                // Socket đã bị đóng, không cần xử lý thêm
            }
            catch (SocketException ex)
            {
                Invoke((MethodInvoker)delegate {
                    conStatus.Text = "Connection error: " + ex.Message;
                });
            }
        }

        void SendData(IAsyncResult iar)
        {
            Socket remote = (Socket)iar.AsyncState;
            int sent = remote.EndSend(iar);
            remote.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), remote);
        }

    }
}
