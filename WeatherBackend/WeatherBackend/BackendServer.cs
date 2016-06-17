using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WeatherBackend
{
    internal class BackendServer
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);


        public static void StartListening()
        {
            // Data buffer for incoming data.
            var bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            var listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        AcceptCallback,
                        listener);

                    // Wait until a connection is made before continuing.
                    AllDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            AllDone.Set();

            // Get the socket that handles the client request.
            var listener = (Socket) ar.AsyncState;
            var handler = listener.EndAccept(ar);

            // Create the state object.
            var state = new StateObject {WorkSocket = handler};
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                ReadCallback, state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            var state = (StateObject) ar.AsyncState;
            var handler = state.WorkSocket;

            // Read data from the client socket. 
            var bytesRead = handler.EndReceive(ar);

            if (bytesRead <= 0) return;
            // There  might be more data, so store the data received so far.
            state.Sb.Append(Encoding.ASCII.GetString(
                state.Buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            var content = state.Sb.ToString();
            if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
            {
                var value = content;
                // All the data has been read from the 
                // client. Display it on the console.\

                // compare content 
                // get first char 
                var toint = int.Parse(value.Replace("<EOF>", ""));
                
                SendRequest(toint);

                Send(handler, content);
            }
            else
            {
                // Not all data received. Get more.
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    ReadCallback, state);
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            var byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                var handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public BackendServer()
        {
            StartListening();
        }

        //mikes shitty little function 
        private static void SendRequest(int rating)
        {
            if (rating < 8) return;
            var request = WebRequest.Create("http://localhost:27039/notification/potentialcat");
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            var byteArray = Encoding.UTF8.GetBytes(string.Empty);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            var dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            var response = request.GetResponse();
            // Display the status.
            // Get the stream containing content returned by the server.
            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(response.GetResponseStream());
            // Read the content.
            reader.ReadToEnd();
            // Display the content.
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}