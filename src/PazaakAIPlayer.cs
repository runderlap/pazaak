using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class PazaakAIPlayer : IPazaakPlayer
    {
        private int FieldSizeToStand = 17;
        public string Name()
        {
            return "Robot";
        }
        public bool IsHuman()
        {
            return false;
        }

        public bool WillStand(GameState gameState)
        {
            return gameState.YourFieldValue >= FieldSizeToStand;
        }

        public Card CardToPlay(GameState gameState)
        {
            Card card = null;
            if (gameState.HandCards.Any())
            {
                if (gameState.YourFieldValue<Rules.MAX_ALLOWED_FIELD_VALUE)
                {
                    card = GetCardToIncreaseFieldValue(gameState.HandCards, gameState.YourFieldValue);
                }
                else
                {
                    card = GetCardToDecreaseFieldValue(gameState.HandCards, gameState.YourFieldValue);
                }
            }
            return card;
        }
        public void RoundOver(GameState gameState)
        {
            //@TODO: handle the end round result;
        }
        public void GameOver(int yourWins, int opponentWins)
        {
            //@TODO: handle the end game result;
        }

        private Card GetCardToIncreaseFieldValue(List<Card> handCards, int currentFieldValue)
        {
            foreach (var card in handCards)
            {
                if (currentFieldValue + card.Value == Rules.MAX_ALLOWED_FIELD_VALUE)
                {
                    if (card.Switchable)
                    {
                        card.ChosenSwitch = CardSwitch.Positive;
                    }
                    return card;
                }
            }
            return null;
        }
        private Card GetCardToDecreaseFieldValue(List<Card> handCards, int currentFieldValue)
        {
            foreach (var card in handCards.Where(c => c.Value<0 || c.Switchable))
            {
                if (currentFieldValue + card.Value <= Rules.MAX_ALLOWED_FIELD_VALUE)
                {
                    return card;
                }
                if (card.Switchable && currentFieldValue - card.Value <= Rules.MAX_ALLOWED_FIELD_VALUE)
                {
                    card.ChosenSwitch = CardSwitch.Negative;
                    return card;
                }                
            }
            return null; //we're screwed
        }

    }
}
