using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    public class PlayerStatus
    {
        public IPazaakPlayer Player { get; set; }
        public bool Stand { get; set; }
        public List<int> FieldCards { get; set; }
        public List<Card> HandCards { get; set; }
        public int RoundWins { get; set; }
        public int GameWins { get; set; }
        public int FieldValue => FieldCards.Sum();
    }
}
