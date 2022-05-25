namespace BCake.Parser
{
    public class Token
    {
        public string Value, FilePath;
        public int Line, Column;

        public static Token Anonymous(string value)
        {
            return new Token
            {
                Value = value
            };
        }

        public override string ToString() => $"Token { Value }";
    }
}
