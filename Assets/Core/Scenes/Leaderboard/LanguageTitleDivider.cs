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



