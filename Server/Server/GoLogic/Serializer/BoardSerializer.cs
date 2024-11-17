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
            List<Stone> potentialKoPositions = new List<Stone>();
            GameBoard boardCopy = new GameBoard(logic.Board.Size);

            // R�cup�re tous les voisins vides de la pierre pr�c�dente
            foreach (Stone stone in logic.GetNeighbors(logic.PreviousStone))
            {
                if (stone.Color == StoneColor.Empty)
                {
                    // Pour chaque voisin vide, fait une nouvelle copie et teste
                    boardCopy.Board = logic.Board.CopyBoard();
                    boardCopy.PreviousBoard = logic.Board.PreviousBoard;
                    GameLogic logicCopy = new GameLogic(boardCopy);

                    // Essaie de placer une pierre de la couleur du joueur actuel
                    Stone testStone = boardCopy.Board[stone.X, stone.Y];
                    testStone.Color = currentTurn;

                    // Capture toutes les pierres adverses
                    logicCopy.CapturesOpponent(testStone);

                    // V�rifie si cela cr�e une situation de Ko
                    if (logicCopy.IsKoViolation(boardCopy))
                    {
                        potentialKoPositions.Add(stone);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x,y,color");

            foreach (Stone stone in logic.Board.Board)
            {
                string color = stone.Color.ToString();
                if (potentialKoPositions.Any(k => k.X == stone.X && k.Y == stone.Y))
                {
                    color = "Ko";
                }
                sb.AppendLine($"{stone.X},{stone.Y},{color}");
            }

            return sb.ToString();
        }
    }
} 