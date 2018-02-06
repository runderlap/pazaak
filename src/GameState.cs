using System.Collections.Generic;

namespace Pazaak
{
    public class GameState
    {
        public List<Card> HandCards;
        public int YourFieldValue;
        public int OpponentFieldValue;
        public int YourWins;
        public int OpponentWins;
        public int NumberOfOpponentHandCards;
        public bool OpponentStands;
    }
}