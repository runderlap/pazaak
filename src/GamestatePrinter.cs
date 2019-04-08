using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class GamestatePrinter
    {
        public static string PlayField(PlayerStatus Player1, PlayerStatus Player2)
        {
            Console.Clear();
            var FirstRowPlayer1 = GetfourCardStringValue(Player1.FieldCards, 0, 4);
            var FirstRowPlayer2 = GetfourCardStringValue(Player2.FieldCards, 0, 4);
            var SecondRowPlayer1 = GetfourCardStringValue(Player1.FieldCards, 4, 8);
            var SecondRowPlayer2 = GetfourCardStringValue(Player2.FieldCards, 4, 8);
            var HandCardsPlayer1 = Player1.HandCards.Select(c => c.ToString()).ToList();
            var HandCardsPlayer2 = Player2.HandCards.Select(c => c.ToString()).ToList();
            var result = new StringBuilder();
            result.AppendLine("╔═════════════════════════════════╗");
            result.AppendLine("║             PAZAAK!             ║");
            result.AppendLine("╟────────────────╥────────────────╢");
            result.AppendLine($"║ PLAYER1  {WinsPrint(Player1.RoundWins)}║ PLAYER2  {WinsPrint(Player2.RoundWins)}║");
            result.AppendLine("╟────────────────╫────────────────╢");
            CreateCardRows(ref result, FirstRowPlayer1, FirstRowPlayer2, false);
            CreateCardRows(ref result, SecondRowPlayer1, SecondRowPlayer2, false);
            result.AppendLine($"║ TOTAL:     {Player1.FieldValue.ToString("D2")}  ║ TOTAL:     {Player2.FieldValue.ToString("D2")}  ║");
            result.AppendLine("╟────────────────╫────────────────╢");
            CreateCardRows(ref result, HandCardsPlayer1, HandCardsPlayer2, true);
            result.AppendLine("╚════════════════╩════════════════╝");
            return result.ToString();
        }

        private static List<string> GetfourCardStringValue(List<int> fieldCards, int skip, int length)
        {
            var result = new List<string>();
            for (int i = skip; i < length + skip; i++)
            {
                if (fieldCards.Count > i)
                {
                    if (fieldCards[i] > 9)
                    {
                        result.Add(fieldCards[i].ToString());
                    }
                    else if (fieldCards[i] > 0)
                    {
                        result.Add($"+{fieldCards[i].ToString()}");
                    }
                    else if (fieldCards[i] < 0)
                    {
                        result.Add($"{fieldCards[i].ToString()}");
                    }

                }
            }
            return result;
        }

        private static void CreateCardRows(ref StringBuilder result, List<string> cardsLeft, List<string> cardsRight, bool hideOpponentsValues)
        {
            var firstCards = cardsLeft ?? new List<string>();
            var nextCards = cardsRight ?? new List<string>();
            var width = 4;

            //The top part
            var topLine = "║";
            for (int i = 0; i < width; i++)
            {
                topLine += (firstCards.Count >= i + 1) ? "┌──┐" : "    ";
            }
            topLine += "║";
            for (int i = 0; i < width; i++)
            {
                topLine += (nextCards.Count >= i + 1) ? "┌──┐" : "    ";
            }
            topLine += "║";
            result.AppendLine(topLine);

            //The numbers
            var middleLine = "║";
            for (int i = 0; i < width; i++)
            {
                middleLine += (firstCards.Count >= i + 1) ? $"│{firstCards.ElementAt(i)}│" : "    ";
            }
            middleLine += "║";
            //@TODO: hideOpponentsValues assumes that P2 is always the AI player.
            //it should somehow evaluate if player 1 OR 2 is AI (if both are, nothing needs to be hidden)
            for (int i = 0; i < width; i++)
            {
                middleLine += (nextCards.Count >= i + 1) ? (hideOpponentsValues ? "│  │" : $"│{nextCards.ElementAt(i)}│") : "    ";
            }
            middleLine += "║";
            result.AppendLine(middleLine);

            //The bottom part
            var bottomLine = "║";
            for (int i = 0; i < width; i++)
            {
                bottomLine += (firstCards.Count >= i + 1) ? "└──┘" : "    ";
            }
            bottomLine += "║";
            for (int i = 0; i < width; i++)
            {
                bottomLine += (nextCards.Count >= i + 1) ? "└──┘" : "    ";
            }
            bottomLine += "║";
            result.AppendLine(bottomLine);
        }
        private static string WinsPrint(int yourWins)
        {
            var result = string.Empty;
            for (var i = 0; i < Rules.WINS_NEEDED; i++)
            {
                result += (yourWins > i) ? "X " : "O ";
            }
            return result;
        }

    }
}
