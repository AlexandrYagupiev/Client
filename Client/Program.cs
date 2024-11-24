using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Communicate("localhost", 8888);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void Communicate(string hostname, int port)
        {
            //буфер для входящих данных
            byte[] bytes = new byte[1024];

            //соединение с удаленным сервером

            //установка удаленной точки (сервера) для сокета
            IPHostEntry iPHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddr = iPHost.AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(ipAddr, port);

            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //подключение к серверу
            sock.Connect(iPEndPoint);

            Console.Write("Введите сообщение: ");
            string message = Console.ReadLine();

            Console.WriteLine("Подключаемся к порту {0} ",sock.RemoteEndPoint.ToString());

            byte[] data = Encoding.UTF8.GetBytes(message);

            //получаем кол-во отправленных байтов
            int bytesSent= sock.Send(data);

            //получаем ответ от сервера
            int bytesRec= sock.Receive(bytes);

            Console.WriteLine("\nОтвет сервера: {0}\n\n",Encoding.UTF8.GetString(bytes,0,bytesRec));

            if (message.IndexOf("<TheEnd>")==-1)
            {
                Communicate(hostname, port);
            }

            //освобождаем сокет
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}
