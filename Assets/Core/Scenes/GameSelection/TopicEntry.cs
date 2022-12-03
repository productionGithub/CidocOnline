using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class TopicEntry : MonoBehaviour
    {
        public Toggle _topicToggle;
        public TextMeshProUGUI _topicTxt;
        public string _topicKey;

        public event Action OnTopicEntryToggleEvent;

        public void Init()
        {
            _topicToggle.onValueChanged.AddListener(delegate {
                OnTopicToggleValueChanged();
            });
        }

        private void OnTopicToggleValueChanged()
        {
            OnTopicEntryToggleEvent?.Invoke();
        }

        public void OnDestroy()
        {
            _topicToggle.onValueChanged.RemoveListener(delegate {
                OnTopicToggleValueChanged();
            });
        }
    }
}
