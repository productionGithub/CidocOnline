using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LanguageTitleDivider : MonoBehaviour
    {
        public RectTransform _transform;

        public void Show()
        {
            _transform.gameObject.SetActive(true);
        }
    }
}



