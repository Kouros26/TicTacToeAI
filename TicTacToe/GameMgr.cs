using System;
using System.Collections.Generic;

namespace TicTacToe
{
	public struct Move
	{
		public int Line;
		public int Column;
	}

    public enum Player
    {
        None = 0,
        Cross = 1,
        Circle = 2
    }

    public class GameMgr
    {
        bool isGameOver = false;
        public bool IsGameOver { get { return isGameOver; } }
        Board mainBoard = new Board();
        int recursiveCalls = 0;
        int maxDepth = 10;

        public GameMgr()
        {
            mainBoard.Init();
            mainBoard.CurrentPlayer = Player.Cross;
        }

        bool IsPlayerTurn()
        {
            return mainBoard.CurrentPlayer == Player.Cross;
        }

        private int GetPlayerInput(bool isColumn)
        {
            Console.Write("\n{0} turn : enter {1} number\n", IsPlayerTurn() ? "Player" : "Computer", isColumn ? "column" : "line");
            ConsoleKeyInfo inputKey;
            int resNum = -1;
            while (resNum < 0 || resNum > 2)
            {
                inputKey = Console.ReadKey();
                int inputNum = -1;
                if (int.TryParse(inputKey.KeyChar.ToString(), out inputNum))
                    resNum = inputNum;
            }
            return resNum;
        }

        public bool Update()
        {
            mainBoard.Draw();

            Move crtMove = new Move();
            if (IsPlayerTurn())
            {
                crtMove.Column = GetPlayerInput(true);
                crtMove.Line = GetPlayerInput(false);
                if (mainBoard.BoardSquares[crtMove.Line, crtMove.Column] == 0)
                {
                    mainBoard.MakeMove(crtMove);
                }
            }
            else
            {
                ComputeAIMove();
            }

            if (mainBoard.IsGameOver())
            {
                mainBoard.Draw();
                Console.Write("game over - ");
                int result = mainBoard.Evaluate(Player.Cross, 0);
                if (result == 100)
                    Console.Write("you win\n");
                else if (result == -100)
                    Console.Write("you lose\n");
                else
                    Console.Write("it's a draw!\n");

                Console.Write("IA Recursive calls this game : " + recursiveCalls);
                Console.ReadKey();

                return false;
            }
            return true;
        }

        // ***** AI : random move
        void ComputeAIMove()
        {
            int bestScore = int.MinValue;
            Move bestMove = new Move();

            // Loop through all available moves
            List<Move> availableMoves = mainBoard.GetAvailableMoves();
            foreach (Move move in availableMoves)
            {
                // Make the move
                mainBoard.MakeMove(move);

                // Calculate the score for this move
                int score = MiniMax(mainBoard, maxDepth, false);

                // Undo the move
                mainBoard.UndoMove(move);

                // If this move has a higher score, update the best move
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            // Make the best move
            mainBoard.MakeMove(bestMove);
        }


        int MiniMax(Board board, int depth, bool isMaximizing)
        {
            recursiveCalls++;

            if (board.IsGameOver() || depth == 0)
            {
                return board.Evaluate(Player.Circle, maxDepth - depth);
            }

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
            List<Move> availableMoves = board.GetAvailableMoves();

            foreach (Move move in availableMoves)
            {
                if (isMaximizing)
                {
                    board.MakeMove(move);
                    int score = MiniMax(board, depth - 1, false);
                    bestScore = Math.Max(bestScore, score);
                    board.UndoMove(move);
                }

                else
                {
                    board.MakeMove(move);
                    int score = MiniMax(board, depth - 1, true);
                    bestScore = Math.Min(bestScore, score);
                    board.UndoMove(move);
                }
            }

            return bestScore;
        }
    }
}

