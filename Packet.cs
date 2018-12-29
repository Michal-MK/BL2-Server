using System;

namespace PacketNS {

	/// <summary>
	/// Enum for differentiating from playable character
	/// </summary>
	public enum Player {
		Gaige,
		Axton
	}

	/// <summary>
	/// All possible states of a quest in the run
	/// </summary>
	public enum QuestStatus {
		NotAccepted,
		Hidden,
		Accepted,
		Triggered
	}

	/// <summary>
	/// Packet data structure for relaying information to the server
	/// </summary>
	[Serializable]
	public struct Packet {
		/// <summary>
		/// The ID of the quest to modify
		/// </summary>
		public int questID { get; set; }

		/// <summary>
		/// The state to switch to
		/// </summary>
		public QuestStatus status { get; set; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public Packet(int questID, QuestStatus status) {
			this.questID = questID;
			this.status = status;
		}
	}

	/// <summary>
	/// Structure for an in-game quest
	/// </summary>
	[Serializable]
	public class Quest {

		/// <summary>
		/// The unique ID of this quest
		/// </summary>
		public int questID;

		/// <summary>
		/// The in-game name of the quest
		/// </summary>
		public string questName;

		/// <summary>
		/// The location where this quest can be obtained
		/// </summary>
		public string acceptingLocation;

		/// <summary>
		/// The person/thing that gives you this quest
		/// </summary>
		public string givenBy;

		/// <summary>
		/// Integer signifying since when the quest is available, in relation to <see cref="questID"/>
		/// </summary>
		public int availableSince;

		/// <summary>
		/// The location where this quest is usually needed
		/// </summary>
		public string triggerLocation;

		/// <summary>
		/// Is the quest available only while we do not **** up
		/// </summary>
		public bool isOptional;

		/// <summary>
		/// The usual quest level based on the average run
		/// </summary>
		public int questLevel;

		/// <summary>
		/// The current state of this quest
		/// </summary>
		public QuestStatus status;

		/// <summary>
		/// The player that usually accepts this quest
		/// </summary>
		public Player acceptedBy;

		/// <summary>
		/// The player that usually triggers the skip with this quest
		/// </summary>
		public Player triggeredBy;
	}
}
