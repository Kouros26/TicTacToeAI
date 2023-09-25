using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Statistics
    {
        private ulong miniMaxAverage = 0;
        private ulong negaMaxAverage = 0;
        private ulong ABPrunnigAverage = 0;

        private int miniMaxCount = 0; //Number of times the IA played using these algorithms
        private int negaMaxCount = 0;
        private int ABPruningCount = 0;

        public void ChooseAlgorithm(ref GameMgr game)
        {
            Console.WriteLine(
                "Which algorithm would you like to use ?\n'0' for MiniMax, '1' for NegaMax, '2' for ABPrunning");

            string algoStr = Console.ReadLine();
            int algoChoice = 0;

            if (int.TryParse(algoStr, out algoChoice))
            {
                switch (algoChoice)
                {
                    case 0:
                        game.algorithmUsed = 0;
                        break;
                    case 1:
                        game.algorithmUsed = 1; 
                        break;
                    case 2:
                        game.algorithmUsed = 2; 
                        break;

                    default:
                        Console.WriteLine("This is not valid, try again\n");
                        ChooseAlgorithm(ref game);
                        break;
                }
            }

            else
            {
                Console.WriteLine("This is not valid, try again\n");
                ChooseAlgorithm(ref game);
            }
        }

        public void WriteDataToFile(int functionID, int recursiveCalls)
        {
            List<string> list = new List<string>();
            string[] lines = File.ReadAllLines("../../algoStats.txt");
            list.AddRange(lines);
            list.Add(functionID.ToString() + ' ' + recursiveCalls.ToString());
            
            File.WriteAllLines("../../algoStats.txt", list);
        }

        public void ShowData()
        {
            string[] lines = File.ReadAllLines("../../algoStats.txt");

            foreach (var line in lines)
            {
                switch (line[0])
                {
                    case '0':
                        miniMaxAverage += ulong.Parse(line.Substring(2));
                        miniMaxCount++;
                        break;

                    case '1':
                        negaMaxAverage += ulong.Parse(line.Substring(2));
                        negaMaxCount++;
                        break;

                    case '2':
                        ABPrunnigAverage += ulong.Parse(line.Substring(2));
                        ABPruningCount++;
                        break;
                }
            }

            if (miniMaxCount != 0) 
                miniMaxAverage /= (ulong)miniMaxCount;

            if (negaMaxCount != 0)
                negaMaxAverage /= (ulong)negaMaxCount;

            if (ABPrunnigAverage != 0)
                ABPrunnigAverage /= (ulong)ABPruningCount;

            Console.WriteLine("Average AI function calls using MiniMax's algorithm : " + miniMaxAverage + '\n' +
                              "Sample size : " + miniMaxCount);

            Console.WriteLine("Average AI function calls using NegaMax's algorithm : " + negaMaxAverage + '\n' +
                              "Sample size : " + negaMaxCount);

            Console.WriteLine("Average AI function calls using ABPrunning's algorithm : " + ABPrunnigAverage + '\n' +
                              "Sample size : " + ABPruningCount);
        }
    }
}
