using Igor.TCP;
using System.Threading;

namespace BL2Quests_Server {
	public class Program {

		private static ManualResetEventSlim evnt = new ManualResetEventSlim();

		static void Main(string[] args) {
			new Thread(new ThreadStart(() => {
				Borderlands bl = new Borderlands();
			})).Start();

			evnt.Wait();
		}
	}
}
