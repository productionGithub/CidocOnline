using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterCore.Core.Scenes.DeckTest
{
    public class ChapterInfoBundle
    {
        public string _scenarioTitle;
        public string _chapterTitle;
        public string _username;

        public ChapterInfoBundle(string stitle, string ctitle, string username)
        {
            _scenarioTitle = stitle;
            _chapterTitle = ctitle;
            _username = username;
        }
    }
}
