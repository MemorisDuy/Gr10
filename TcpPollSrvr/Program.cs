using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpPollSrvr
{
    public static void Main()
    {
        int recv;
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any,9050);

        Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        newsock.Bind(ipep);
        newsock.Listen(10);
        Console.WriteLine("Waiting for a client...");
        bool result;
        int i = 0;
        while (true)
        {
            i++;
            Console.WriteLine("polling for accept#{0}...", i);
            result = newsock.Poll(1000000, SelectMode.SelectRead);
            if (result)
            {
                break;
            }
        }
        Socket client = newsock.Accept();

        IPEndPoint newclient = (IPEndPoint) client.RemoteEndPoint;
        Console.WriteLine("Connected with {0} at port {1}", newclient.Address, newclient.Port);

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        client.Send(data, data.Length, SocketFlags.None);

        i = 0;
        while (true)
        {
            Console.WriteLine("polling for receive #{0}...", i);
            i++;
            result = client.Poll(3000000, SelectMode.SelectRead);
            if (result)
            {
                data = new byte[1024];
                i = 0;
                recv = client.Receive(data);
                if (recv == 0)
                    break;

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                client.Send(data, recv, 0);
            }
        }

        Console.WriteLine("Disconnected from {0}", newclient.Address);
        client.Close();
        newsock.Close();
    }
}