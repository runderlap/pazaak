using System;
using Euler.Tools;

namespace Pazaak
{
    public class Card
    {
        public readonly int Value;
        public readonly bool Switchable;
        public int FieldValue => GetFieldValue();
        public CardSwitch ChosenSwitch { get; set; }
        public Card(int value) { Value = value; }
        public Card(int min, int max) {
            Value = RandomTool.Get(min, max);
            ChosenSwitch = CardSwitch.None;
            Switchable = RandomTool.Get();
            if (Switchable && Value < 0)
            {
                Value = Value * -1;
            }
        }
        public static Card HandCard() => new Card(Rules.MIN_HAND_CARD_VALUE, Rules.MAX_HAND_CARD_VALUE);
        public static Card FieldCard() => new Card(Rules.MIN_CARD_VALUE, Rules.MAX_CARD_VALUE);
        private int GetFieldValue()
        {
            if (Switchable && ChosenSwitch==CardSwitch.Negative)
            {
                return Value * -1;
            }            
            return Value;
        }
        public override string ToString()
        {
            var sign = Switchable ? @"±" : (Value > 0 ? "+" : string.Empty);
            return $"{sign}{Value}";
        }
        public bool Equals(Card card)
        {
            return Value == card.Value && Switchable == card.Switchable;
        }
    }
}
