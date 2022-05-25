using System.Linq;

namespace BCake.Parser {
    public static class ParserHelper {
        public static int FindClosingScope(Token[] tokens, int startTokenIndex) {
            var token = tokens[startTokenIndex];
            string closing = null;
            var level = 0;

            switch (token.Value.Trim()) {
                case "{":
                    closing = "}";
                    break;
                case "(":
                    closing = ")";
                    break;
                case "[":
                    closing = "]";
                    break;
                case "<":
                    closing = ">";
                    break;
            }

            for (int i = startTokenIndex; i < tokens.Length; ++i) {
                if (tokens[i].Value == token.Value) level++;
                if (tokens[i].Value == closing) level--;
                if (level == 0) return i;
            }

            return -1;
        }

        public static string FindString(string content, int startPos, out int lineBreaks, out int column, out int end) {
            // currently unused but will be used in the future for different kinds of strings
            // e.g. strings with a $ prefix where variables can be interpolated
            var mode = StringMode.String;
            var escapeNext = false;
            var result = "";
            lineBreaks = 0;
            column = 0;
            end = -1;

            var prefix = "";
            var stringBegin = content.IndexOf("\"", startPos);
            if (stringBegin - startPos > 0) {
                prefix = content.Substring(startPos, stringBegin - startPos);
            }

            for (int i = stringBegin + 1; i < content.Length; ++i) {
                var c = content[i];

                switch (c) {
                    case '"':
                        if (!escapeNext) {
                            end = i + 1;
                            return prefix + "\"" + result + "\"";
                        }
                        escapeNext = false;
                        break;

                    case '\\':
                        if (!escapeNext) {
                            escapeNext = true;
                            continue;
                        }
                        break;

                    case '\n':
                        lineBreaks++;
                        column = 0;
                        break;
                }

                column++;
                result += c;
            }

            return null;
        }

        public static int FindListItemEnd(Token[] tokens, int startTokenIndex) => FindListItemEnd(tokens, startTokenIndex, new string[] { });
        public static int FindListItemEnd(Token[] tokens, int startTokenIndex, string[] terminatingTokens) {
            var brackets = new string[] { "(", "{", "[", "<" };

            for (int i = startTokenIndex; i < tokens.Length; ++i) {
                var token = tokens[i];
                if (terminatingTokens.Contains(token.Value)) return i;
                if (brackets.Contains(token.Value)) i = FindClosingScope(tokens, i);
                else if (token.Value == ",") return i;
            }

            return -1;
        }


        public enum StringMode {
            None = 0,
            String
        }
    }
}