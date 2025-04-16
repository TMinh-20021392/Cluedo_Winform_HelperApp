namespace Cluedo_Winform_HelperApp.SuggestionInput
{
    // Event arguments for response
    public class ResponseEventArgs(string who, string what, string where, bool hasResponse) : EventArgs
    {
        public string Who { get; private set; } = who;
        public string What { get; private set; } = what;
        public string Where { get; private set; } = where;
        public bool HasResponse { get; private set; } = hasResponse;
    }
}
