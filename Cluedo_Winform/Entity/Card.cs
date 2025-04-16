namespace Cluedo_Winform_HelperApp.Entity
{

    public enum CardType
    {
        Suspect,
        Weapon,
        Room
    }

    public class Card(string name, CardType type)
    {
        public string Name { get; private set; } = name;
        public CardType Type { get; private set; } = type;

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card other)
            {
                return Name == other.Name && Type == other.Type;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }
    }
}