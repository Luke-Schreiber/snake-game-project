﻿using NetworkUtil;
using Newtonsoft.Json;
using Server;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace SnakeGame
{
    class Server
    {
        private Dictionary<long, SocketState> clients;
        private World serverWorld;
        private GameSettings? settings;
        private string wallSettings;

        public static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();

            Console.Read();
        }

        public Server()
        {
            clients = new Dictionary<long, SocketState>();
            serverWorld = new World();
        }

        public void StartServer()
        {
            // This begins an "event loop"
            Networking.StartServer(NewPlayerConnected, 11000);

            DataContractSerializer ser = new DataContractSerializer(typeof(GameSettings));
            XmlReader reader;
            try
            {
                reader = XmlReader.Create("GameSettings.xml");
            }
            catch(Exception)
            {
                reader = XmlReader.Create("../../../GameSettings.xml");
            }
            settings = (GameSettings)ser.ReadObject(reader);

            StringBuilder buildWalls = new StringBuilder();

            foreach (Wall w in settings.Walls)
            {
                buildWalls.Append(JsonConvert.SerializeObject(w) + "\n");
            }
            wallSettings = buildWalls.ToString();

            Console.WriteLine("Server is running");
        }

        private void NewPlayerConnected(SocketState state)
        {
            if (state.ErrorOccurred)
                return;


            // change the state's network action to the 
            // receive handler so we can process data when something
            // happens on the network
            state.OnNetworkAction = Handshake;

            Networking.GetData(state);
        }

        /*
         * Receive player name - this is a delegate that implements the server's part of the initial handshake.
         * Make a new Snake with the given name and a new unique ID (recommend using the SocketState's ID).
         * Then change the callback to a method that handles command requests from the client.
         * Then send the startup info to the client. Then add the client's socket to a list of all clients.
         * Then ask the client for data.
         * Note: it is important that the server sends the startup info before adding the client to the list of all clients.
         * This guarantees that the startup info is sent before any world info.
         * Remember that the server is running a loop on a separate thread that may send world info to the list of clients at any time.
         */
        private void Handshake(SocketState state)
        {
            string playerName = state.GetData().Replace("\n", "");

            serverWorld.addSnake(new Snake(playerName, state.ID));

            state.OnNetworkAction = CommandRequest;

            string startUp = "" + state.ID + "\n" + settings.UniverseSize + "\n" + wallSettings;


            state.TheSocket.Send(Encoding.ASCII.GetBytes(startUp.ToString()));

            // Save the client state
            // Need to lock here because clients can disconnect at any time
            lock (clients)
            {
                clients[state.ID] = state;
            }

            Console.WriteLine("Player: " + playerName + " has connected");

            Networking.GetData(state);
        }

        private void CommandRequest(SocketState state)
        {

        }


    }
}