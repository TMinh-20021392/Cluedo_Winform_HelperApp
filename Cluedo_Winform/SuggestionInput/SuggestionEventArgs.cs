namespace Cluedo_Winform_HelperApp.SuggestionInput
{
    // Event arguments for suggestion
    public class SuggestionEventArgs(string who, string what, string where) : EventArgs
    {
        public string Who { get; private set; } = who;
        public string What { get; private set; } = what;
        public string Where { get; private set; } = where;
    }
}
