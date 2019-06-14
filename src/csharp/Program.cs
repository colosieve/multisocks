using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace csharp
{
    class Program
    {
        static private string ExeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        // Constructor sets it
        private string[] args = null;

        // PaseOptions sets it
        private IPEndPoint ipEndpoint = null;

        // StartServer sets it
        private Socket listener = null;

        private void ParseOptions()
        {
            bool showHelp = false;
            int exitCode = 0;
            if (args.Length == 0)
                showHelp = true;

            if (args.Length == 1 && args[0] == "-h")
                showHelp = true;
            
            if (args.Length > 0 && (args.Length != 2 || args[0] != "-p"))
            {
                Console.WriteLine("Unexpected options: {0}", args[0]);
                exitCode = 1;
                showHelp = true;
            }

            if (showHelp)
            {
                Console.WriteLine("Usage: {0} -p [port]", ExeName);
                Environment.Exit(exitCode);
            }

            int port;
            if (!int.TryParse(args[1], out port) || port < 0 || port >= 65536)
            {
                Console.WriteLine("Wrong port: {0}", args[1]);
                Environment.Exit(2);
            }

            IPAddress ipAny = IPAddress.Any;
            this.ipEndpoint = new IPEndPoint(ipAny, port);
        }

        private void StartServer()
        {
            this.listener = new Socket(this.ipEndpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try {
                this.listener.Bind(this.ipEndpoint);
                this.listener.Listen(100);
                Console.WriteLine("Listening on port {0}", this.ipEndpoint.Port);
                Thread.Sleep(60000);
            }
            catch (Exception ex) {
                Console.WriteLine("ERROR: {0}", ex.ToString());
                Environment.Exit(2);
            }
        }

        public void Run()
        {
            this.ParseOptions();
            this.StartServer();
        }

        public Program(string[] args)
        {
            this.args = args;
        }

        static void Main(string[] args)
        {
            Program prog = new Program(args);
            prog.Run();
        }
    }
}
