namespace Cluedo_Winform_HelperApp.Entity
{

    public class Player(int playerNumber)
    {
        public int PlayerNumber { get; private set; } = playerNumber;
        public List<Card> Cards { get; private set; } = [];

        // Track which cards this player has explicitly shown not to have
        public List<Card> ExcludedCards { get; private set; } = [];

        // Track which sets of cards this player has indicated they have at least one of
        public List<CardSet> PossibleCards { get; private set; } = [];

        public void AddCard(Card card)
        {
            if (!Cards.Contains(card))
            {
                Cards.Add(card);
            }
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public bool HasCard(Card card)
        {
            return Cards.Contains(card);
        }

        public bool HasAnyCard(List<Card> cards)
        {
            return cards.Any(c => HasCard(c));
        }

        public void AddExcludedCard(Card card)
        {
            if (!ExcludedCards.Contains(card))
            {
                ExcludedCards.Add(card);
            }
        }

        public void AddPossibleCardSet(Card suspect, Card weapon, Card room)
        {
            var cardSet = new CardSet(suspect, weapon, room);
            PossibleCards.Add(cardSet);
        }

        public override string ToString()
        {
            return $"Player {PlayerNumber}";
        }
    }
}