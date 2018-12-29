namespace PacketNS {
	/// <summary>
	/// Various constants used throughout the application
	/// </summary>
	public static class Constants {

		/// <summary>
		/// Packet ID for synchronizing the global quest container
		/// </summary>
		public const byte PROPERTY_SYNC = 6;

		/// <summary>
		/// Packet ID for relaying changes from connected clients about changes to the global quest container
		/// </summary>
		public const byte PACKET_ID = 7;

		/// <summary>
		/// The port this server is running on
		/// </summary>
		internal const ushort PORT = 4245;

		/// <summary>
		/// The path to quests definitions
		/// </summary>
		internal const string PATH = "QuestIndexBL2.txt";

	}
}
