using System;

namespace TicTacToe
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			GameMgr game = new GameMgr();
			Statistics stats = new Statistics();

			stats.ChooseAlgorithm(ref game);

            while (game.Update()) {}

			stats.WriteDataToFile(game.algorithmUsed, game.recursiveCalls);
			stats.ShowData();

            Console.ReadKey();
        }
	}
}
