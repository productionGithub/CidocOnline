using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LanguageTitleEntry : MonoBehaviour
    {
        public TextMeshProUGUI _titleTxt;

        public void Show(string title)
        {
            _titleTxt.text = $"{title}";
        }
    }
}



