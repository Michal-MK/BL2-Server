using System;
using System.Threading;

namespace BL2Quests_Server {
	public class Program {

		private static Borderlands instance;

		static void Main(string[] args) {
			Run();

			while (true) {
				string s = Console.ReadLine();
				switch (s) {
					case "restart": {
						instance.Restart();
						break;
					}
				}
			}
		}

		private static void Run() {
			new Thread(new ThreadStart(() => {
				instance = new Borderlands();
			})).Start();
		}
	}
}
