using Igor.TCP;
using PacketNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BL2Quests_Server {
	/// <summary>
	/// Main class of the program
	/// </summary>
	internal class Borderlands {

		/// <summary>
		/// Reference to the server
		/// </summary>
		public TCPServer server;

		/// <summary>
		/// The structure holding the global state of all interesting quests in the game
		/// </summary>
		public List<Quest> list { get; set; } = new List<Quest>();

		/// <summary>
		/// Default constructor
		/// </summary>
		public Borderlands() {
			server = new TCPServer(new ServerConfiguration());
			server.OnConnectionEstablished += Server_OnConnectionEstablished;
			server.OnClientDisconnected += Server_OnClientDisconnected;
			ParseQuests();
			server.Start(Constants.PORT);
			
		}

		

		private void ParseQuests() {
			if (!File.Exists(Constants.PATH)) {
				throw new Exception("Quest definition file is missing!");
			}

			string[] lines = File.ReadAllLines(Constants.PATH);

			for (int i = 0; i < lines.Length; i++) {
				if (string.IsNullOrWhiteSpace(lines[i])) {
					continue;
				}
				string[] splitLine = lines[i].Split(':');
				Quest q = new Quest();
				q.questID = int.Parse(splitLine[0].Trim());
				q.questName = splitLine[1].Trim();
				q.acceptingLocation = splitLine[2].Trim();
				q.givenBy = splitLine[3].Trim();
				q.availableSince = int.Parse(splitLine[4].Trim());
				q.acceptedBy = (Player)Enum.Parse(typeof(Player),splitLine[5].Trim());
				q.questLevel = int.Parse(splitLine[6].Trim()); 
				list.Add(q);
			}
		}

		private async void Server_OnConnectionEstablished(object sender, ClientConnectedEventArgs e) {
			e.myServer.GetConnection(e.clientInfo.clientID).dataIDs.DefineCustomDataTypeForID<Packet>(Constants.PACKET_ID, OnPacketReceived);
			e.myServer.GetConnection(e.clientInfo.clientID).dataIDs.DefineCustomDataTypeForID<Quest[]>(Constants.PROPERTY_SYNC, OnDataReceived);
			await Task.Delay(400);
			e.myServer.GetConnection(e.clientInfo.clientID).SendData(Constants.PROPERTY_SYNC, SimpleTCPHelper.GetBytesFromObject(list.ToArray()));
			Console.WriteLine(e.clientInfo.computerName + "(" + e.clientInfo.clientID + ")" + " " + e.clientInfo.clientAddress);
		}

		private void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
			Console.WriteLine(e.clientID + "Disconnected");
		}

		private void OnDataReceived(Quest[] arg1, byte arg2) {
			throw new NotImplementedException("Unexpected message!");
		}

		private void OnPacketReceived(Packet packet, byte senderID) {
			Quest q = list.Where((Quest other) => { return other.questID == packet.questID; }).First();
			if (q == null) return;

			Console.WriteLine("Received packet for modification of " + q.questName + "." +
				" Changing status from " + q.status.ToString() + " to " + packet.status.ToString() + "!");

			q.status = packet.status;
			server.SendToAll(Constants.PROPERTY_SYNC, list.ToArray());
		}
	}
}