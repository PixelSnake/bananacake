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

        public static Token NativeCode()
        {
            return new Token
            {
                FilePath = "<native code>",
                Value = "",
            };
        }

        public override string ToString() => $"Token { Value }";
    }
}
