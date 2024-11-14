using System.Text;

namespace GoLogic.Serializer
{
    public class BoardSerializer
    {
        private GameLogic logic;

        public BoardSerializer(GameLogic logic)
        {
            this.logic = logic;
        }

        /// <summary>
        /// V�rifie et retourne une repr�sentation du plateau avec les positions Ko marqu�es
        /// Une case est Ko si le coup remet le plateau dans son �tat pr�c�dent
        /// </summary>
        /// <returns>Repr�sentation du plateau avec les positions Ko marqu�es</returns>
        public string ChecksGobanForKo(GameLogic logic, StoneColor currentTurn)
        {
            Stone koCase = new Stone(0, 0);
            GameBoard boardCopy = new GameBoard(logic.Board.Size);
            boardCopy.Board = logic.Board.CopyBoard();
            boardCopy.PreviousBoard = logic.Board.PreviousBoard;
            GameLogic logicCopy = new GameLogic(boardCopy);

            foreach (Stone stone in logic.GetNeighbors(logic.PreviousStone))
            {
                if (stone.Color == StoneColor.Empty) koCase = logic.Board.Board[stone.X, stone.Y];
            }

            // Place la pierre dans une copie pour la v�rification de Ko
            boardCopy.Board[koCase.X, koCase.Y].Color = currentTurn;
            logicCopy.CapturesOpponent(koCase);
            bool isKoViolation = logicCopy.IsKoViolation(boardCopy);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x,y,color");

            // Parcourt le plateau dans l'ordre des pierres
            foreach (Stone stone in logic.Board.Board)
            {
                string color = stone.Color.ToString();
                if (stone.X == koCase.X && stone.Y == koCase.Y && isKoViolation)
                {
                    color = "Ko";
                }
                sb.AppendLine($"{stone.X},{stone.Y},{color}");
            }

            return sb.ToString();
        }
    }
} 