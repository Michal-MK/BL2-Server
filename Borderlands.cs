using Igor.TCP;
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
				if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].StartsWith("#")) {
					continue;
				}

				string[] splitLine = lines[i].Split(':');
				Quest q = new Quest {
					QuestID = int.Parse(splitLine[0].Trim()),
					QuestName = splitLine[1].Trim(),
					AcceptingLocation = splitLine[2].Trim(),
					GivenBy = splitLine[3].Trim(),
					AvailableSince = int.Parse(splitLine[4].Trim()),
					AcceptedBy = (PlayerCharacter)Enum.Parse(typeof(PlayerCharacter), splitLine[5].Trim()),
					QuestLevel = int.Parse(splitLine[6].Trim())
				};
				list.Add(q);
			}
		}

		private async void Server_OnConnectionEstablished(object sender, ClientConnectedEventArgs e) {
			TCPConnection connection = e.myServer.GetConnection(e.clientInfo.clientID);
			connection.dataIDs.DefineCustomDataTypeForID<Packet>(Constants.PACKET_ID, OnPacketReceived);
			await Task.Delay(400);
			connection.SendData(Constants.PROPERTY_SYNC, SimpleTCPHelper.GetBytesFromObject(list.ToArray()));
			Console.WriteLine(e.clientInfo.computerName + "(" + e.clientInfo.clientID + ")" + " " + e.clientInfo.clientAddress);
		}

		private void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
			Console.WriteLine(e.clientID + "Disconnected");
		}

		private void OnPacketReceived(Packet packet, byte senderID) {
			Quest q = list.Where(other => other.QuestID == packet.QuestID).First();
			if (q == null) return;

			Console.WriteLine($"{q.QuestName} change {q.Status.ToString()} -> {packet.Status.ToString()}");
			q.Status = packet.Status;
			server.SendToAll(Constants.PROPERTY_SYNC, list.ToArray());
		}

		internal void Restart() {
			list.Clear();
			ParseQuests();
			server.SendToAll(Constants.PROPERTY_SYNC, list.ToArray());
		}
	}
}