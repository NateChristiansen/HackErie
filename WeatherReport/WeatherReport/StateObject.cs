using System.Net.Sockets;
using System.Text;

namespace WeatherReport
{
    internal class StateObject
    { 
        public Socket WorkSocket = null;
        public const int BufferSize = 256;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder Sb = new StringBuilder();
    }
}
