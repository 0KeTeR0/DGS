using GoLogic.Goban;
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
            //GameBoard boardCopy = new GameBoard(logic.Goban.Size);

            // R�cup�re tous les voisins vides de la pierre pr�c�dente
            foreach (Stone stone in logic.Goban.GetNeighbors(logic.PreviousStone))
            {
                if (stone.Color == StoneColor.Empty)
                {
                    // Pour chaque voisin vide, fait une nouvelle copie et teste
                    IBoard boardCopy = logic.Goban.Clone();
                    CaptureManager captureManagerCopy = new CaptureManager(boardCopy);
                    Stone stoneCopy = boardCopy.GetStone(stone.X, stone.Y);

                    // Essaie de placer une pierre de la couleur du joueur actuel
                    stoneCopy.ChangeColor(currentTurn);

                    // Capture toutes les pierres adverses
                    captureManagerCopy.CapturesOpponent(stoneCopy);

                    // V�rifie si cela cr�e une situation de Ko
                    if (boardCopy.IsKoViolation())
                    {
                        potentialKoPositions.Add(stone);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("x,y,color");

            for (int i = 0; i < logic.Goban.Size; i++)
            {
                for (int j = 0; j < logic.Goban.Size; j++)
                {
                    Stone stone = logic.Goban.GetStone(i, j);
                    string color = stone.Color.ToString();
                    if (potentialKoPositions.Any(k => k.X == stone.X && k.Y == stone.Y))
                    {
                        color = "Ko";
                    }
                    sb.AppendLine($"{stone.X},{stone.Y},{color}");
                }
            }

            return sb.ToString();
        }
    }
} 