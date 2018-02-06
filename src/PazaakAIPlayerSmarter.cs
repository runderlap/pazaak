using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class PazaakAIPlayerSmarter : IPazaakPlayer
    {
        private int FieldSizeToStand = 17;
        private int FieldSizeToTakeRiskWith = 14;
        public string Name()
        {
            return "T-2000";
        }
        public bool IsHuman()
        {
            return false;
        }

        public bool WillStand(GameState gameState)
        {
            return
                //you are higher then your opponent and he's standing
                //(gameState.YourFieldValue < Rules.MAX_ALLOWED_FIELD_VALUE && gameState.YourFieldValue > gameState.OpponentFieldValue && gameState.OpponentStands) ||
                //you have reached your FieldSizeToStand
                gameState.YourFieldValue >= FieldSizeToStand && !GotBigEnoughNegativeCard(gameState);
        }

        private bool GotBigEnoughNegativeCard(GameState gameState)
        {
            //do I believe that I can fix it when I go over the MAX_ALLOWED?
            if (gameState.HandCards.Any())
            {
                var biggestNegativeCardValue = 0;
                foreach (var card in gameState.HandCards)
                {
                    var value = (card.Switchable) ? card.Value * -1 : card.Value;
                    if (value < biggestNegativeCardValue)
                    {
                        value = biggestNegativeCardValue;
                    }
                }
                if (gameState.YourFieldValue + biggestNegativeCardValue <= FieldSizeToTakeRiskWith)
                {
                    return true;
                }
            }
            return false;
        }

        public Card CardToPlay(GameState gameState)
        {
            Card card = null;
            if (gameState.YourFieldValue < Rules.MAX_ALLOWED_FIELD_VALUE && gameState.YourFieldValue > gameState.OpponentFieldValue && gameState.OpponentStands)
            {
                return card;
            }
            if (gameState.HandCards.Any())
            {
                if (gameState.YourFieldValue<Rules.MAX_ALLOWED_FIELD_VALUE)
                {
                    //I might play a card, or I might save my cards for later

                    //opponent is standing game
                    
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
