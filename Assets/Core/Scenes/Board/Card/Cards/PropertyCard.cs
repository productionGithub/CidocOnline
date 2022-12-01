using System.Collections.Generic;
using UnityEngine;

namespace StarterCore.Core.Scenes.Board.Card.Cards
{
    public class PropertyCard
    {
        public int index;
        public string id;
        public string about;
        public string label;
        public string inverseOf;
        public string domain;
        public string range;
        public string comment;
        public List<string> domainColors;
        public List<string> rangeColors;
        public List<string> superProperties = new List<string>();
        public List<string> subProperties = new List<string>();
    }
}
