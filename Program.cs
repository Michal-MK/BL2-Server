using Igor.TCP;
using System;
using System.Threading;

namespace BL2Quests_Server {
	public class Program {

		private static ManualResetEventSlim evnt = new ManualResetEventSlim();

		private static Borderlands bl;

		static void Main(string[] args) {
			Run();

			while (true) {
				string s = Console.ReadLine();
				switch (s) {
					case "restart": {
						bl.Restart();
						break;
					}
				}
			}
		}


		private static void Run() {
			new Thread(new ThreadStart(() => {
				bl = new Borderlands();
			})).Start();
		}
	}
}
