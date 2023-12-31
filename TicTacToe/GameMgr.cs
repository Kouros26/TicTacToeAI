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
        public int algorithmUsed = -1;
        public int recursiveCalls = 0;
        int maxDepth = 30;

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
                int result = mainBoard.Evaluate(Player.Cross);
                if (result == 100)
                    Console.Write("you win\n\n");
                else if (result == -100)
                    Console.Write("you lose\n\n");
                else
                    Console.Write("it's a draw!\n\n");

                return false;
            }
            return true;
        }

        // ***** AI : random move
        void ComputeAIMove()
        {
            int bestScore = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            Move bestMove = new Move();

            List<Move> availableMoves = mainBoard.GetAvailableMoves();
            foreach (Move move in availableMoves)
            {
                mainBoard.MakeMove(move);

                int score = 0;

                switch (algorithmUsed)
                {
                    case 0:
                        score = MiniMax(mainBoard, maxDepth, false);
                        break;

                    case 1:
                        score = -NegaMax(mainBoard, maxDepth);
                        break;

                    case 2:
                        score = AlphaBetaPruning(mainBoard, maxDepth, alpha, beta, false);
                        break;
                }

                mainBoard.UndoMove(move);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            mainBoard.MakeMove(bestMove);
        }


        int MiniMax(Board board, int depth, bool isMaximizing)
        {
            recursiveCalls++;

            if (board.IsGameOver() || depth == 0)
            {
                return board.Evaluate(Player.Circle);
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

        int NegaMax(Board board, int depth)
        {
            recursiveCalls++;

            if (board.IsGameOver() || depth == 0)
            {
                return board.Evaluate();
            }

            int bestScore = int.MinValue;
            List<Move> availableMoves = board.GetAvailableMoves();

            foreach (Move move in availableMoves)
            {
                board.MakeMove(move);
                int score = -NegaMax(board, depth - 1);
                bestScore = Math.Max(bestScore, score);
                board.UndoMove(move);
            }

            return bestScore;
        }

        int AlphaBetaPruning(Board board, int depth, int alpha, int beta, bool isMaximizing)
        {

            recursiveCalls++;

            if (board.IsGameOver() || depth == 0)
            {
                return board.Evaluate(Player.Circle);
            }

            List<Move> availableMoves = board.GetAvailableMoves();

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                foreach (Move move in availableMoves)
                {
                    board.MakeMove(move);
                    int score = AlphaBetaPruning(board, depth - 1, alpha, beta, false);
                    board.UndoMove(move);
                    bestScore = Math.Max(bestScore, score);
                    alpha = Math.Max(alpha, score);

                    if (alpha >= beta)
                        break;
                }
                return bestScore;
            }

            else
            {
                int bestScore = int.MaxValue;
                foreach (Move move in availableMoves)
                {
                    board.MakeMove(move);
                    int score = AlphaBetaPruning(board, depth - 1, alpha, beta, true);
                    board.UndoMove(move);
                    bestScore = Math.Min(bestScore, score);
                    beta = Math.Min(beta, score);

                    if (alpha >= beta)
                        break;
                }
                return bestScore;
            }
        }
    }
}

