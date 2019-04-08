using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class PazaakHumanPlayer : IPazaakPlayer
    {
        public bool IsHuman()
        {
            return true;
        }
        public string Name()
        {
            return "Human";
        }        

        public bool WillStand(GameState gameState)
        {
            var userInput = GetValidatedUserInput("Will You stand?", new Dictionary<string, string>{ { "y", "Stand" }, { "n", "Continue" } });
            return (userInput.Equals("y"));
        }

        public Card CardToPlay(GameState gameState)
        {
            var handcards = gameState.HandCards;
            if (handcards.Any())
            {
                var userInputWantToPlayACard = GetValidatedUserInput("Want to play a card?", new Dictionary<string, string> { { "y", "Play a Card" }, { "n", "Don't play a card" } });
                if (userInputWantToPlayACard.Equals("y"))
                {
                    var userInputWhichCard = GetValidatedUserInput("Which card?", YourHandCardsAsDictionary(handcards));                  
                    var card = handcards[int.Parse(userInputWhichCard)];
                    if (card.Switchable)
                    {
                        var userInputSwitchCard = GetValidatedUserInput("Positive or negative value", new Dictionary<string, string> { {"p", $"Positive (+{card.Value})"}, {"n", $"Negative (-{card.Value})"} });
                        if (userInputSwitchCard.Equals("p"))
                        {
                            card.ChosenSwitch = CardSwitch.Positive;
                        }
                        else
                        {
                            card.ChosenSwitch = CardSwitch.Negative;
                        }
                    }
                    return card;
                }
            }
            return null;
        }

        public void RoundOver(GameState gameState)
        {
            GetValidatedUserInput("Round over. Press y to continue to the next round", new Dictionary<string, string>() { { "y","Continue"} });
        }
        public void GameOver(int yourWins, int opponentWins)
        {
            //nothing to do here I guess
        }

        private Dictionary<string, string> YourHandCardsAsDictionary(List<Card> handCards)
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < handCards.Count; i++)
            {
                result.Add(i.ToString(),handCards[i].ToString());
            }
            return result;
        }

        private string GetValidatedUserInput(string message, Dictionary<string, string> options)
        {
            string userInput = string.Empty;
            var validInput = false;
            while (!validInput)
            {
                w(message);
                foreach (var option in options)
                {
                    w($"{option.Key}) {option.Value}");
                }
                userInput = Console.ReadKey(true).KeyChar.ToString().ToLowerInvariant();
                if (options.ContainsKey(userInput))
                {
                    validInput = true;
                }
                else
                {
                    w($"'{userInput}' is not a valid choice.");
                }
            }
            w($">{userInput}");
            return userInput;
        }

        private static void w(string msg)
        {
            Console.WriteLine(msg);
        }

    }
}
