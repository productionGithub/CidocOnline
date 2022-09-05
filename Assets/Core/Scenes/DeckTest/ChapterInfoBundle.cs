using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterCore.Core.Scenes.DeckTest
{
    public class ChapterInfoBundle
    {
        public string _scenarioTitle;
        public string _chapterTitle;

        public ChapterInfoBundle(string stitle, string ctitle)
        {
            _scenarioTitle = stitle;
            _chapterTitle = ctitle;
        }
    }
}
