using System;
using System.Linq;

namespace StarterCore.Core.Scenes.Board.Challenge
{
    public class ParserTool
    {
        /// <summary>
        /// Utility class for string parsing
        /// Used by ChallengeEvaluator Class.
        /// </summary>


        //Return true if string is a '*'
        public bool isWildCard(string toParse)
        {
            return TrimWhiteSpace(toParse) == "*";
        }



        //return same string but without space characters
        public string TrimWhiteSpace(string s)
        {
            return (string)String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
        }



        //Process "-E23, E21" or "-P34, P89, ..." and return an array of Ids.
        public string[] GetArrayOfValues(string s)
        {
            string[] listOfIds = null;

            if (s != "")
            {
                s = TrimSelector(s);
                //Remove all whitespaces
                string cleanString = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                //Split with comma
                listOfIds = cleanString.Split(',');
            }
            return listOfIds;
        }



        //Remove '-' or ':' or '[]' from strings
        public string TrimSelector(string s)
        {
            //Don't trim if it's a wildcard
            if (s == "*") return s;

            string trimmed = "";

            switch (s[0])
            {
                case '-':
                    trimmed = s.TrimStart('-');
                    break;

                case ':':
                    trimmed = s.TrimStart(':');
                    break;
                case '[':
                    trimmed = s.TrimStart('[').TrimEnd(']');
                    break;
            }

            return trimmed;
        }

    }
}