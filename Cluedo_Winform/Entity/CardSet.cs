namespace Cluedo_Winform_HelperApp.Entity
{
    // Helper class to track sets of cards for which a player has at least one
    public class CardSet(Card suspect, Card weapon, Card room)
    {
        public Card Suspect { get; private set; } = suspect;
        public Card Weapon { get; private set; } = weapon;
        public Card Room { get; private set; } = room;

        public List<Card> ToList()
        {
            return [Suspect, Weapon, Room];
        }

        public override string ToString()
        {
            return $"{Suspect.Name}, {Weapon.Name}, {Room.Name}";
        }
    }
}