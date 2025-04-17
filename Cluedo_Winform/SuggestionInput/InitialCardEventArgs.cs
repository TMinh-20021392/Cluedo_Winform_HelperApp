namespace Cluedo_Winform_HelperApp.SuggestionInput
{
    public class InitialCardEventArgs(int playerNumber, string cardName, string cardType) : EventArgs
    {
        public int PlayerNumber { get; private set; } = playerNumber;
        public string CardName { get; private set; } = cardName;
        public string CardType { get; private set; } = cardType;
    }
}
