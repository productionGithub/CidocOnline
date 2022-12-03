using UnityEngine;
using System;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.GameSelection
{
    public class TopicEntryController : MonoBehaviour
    {
        [SerializeField] private Transform _topicsContainer;

        public event Action<List<string>> OnTopicUpdateEvent;

        public List<string> SelectedTopics;

        public void Init()
        {
            var topicToggles = _topicsContainer.GetComponentsInChildren<TopicEntry>();

            foreach (TopicEntry topic in topicToggles)
            {
                SelectedTopics.Add(topic._topicKey);

                topic.Init();
                topic.OnTopicEntryToggleEvent += () => OnDomainToggleCLicked(topic);
            }
        }

        private void OnDomainToggleCLicked(TopicEntry toggle)
        {
            if (toggle._topicToggle.isOn)
            {
                if (!SelectedTopics.Contains(toggle._topicKey))
                {
                    SelectedTopics.Add(toggle._topicKey);
                }
            }
            else
            {
                SelectedTopics.Remove(toggle._topicKey);
            }

            OnTopicUpdateEvent?.Invoke(SelectedTopics);
        }
    }
}
