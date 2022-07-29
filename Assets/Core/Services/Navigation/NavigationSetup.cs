using System.Collections.Generic;
using UnityEngine;

namespace StarterCore.Core.Services.Navigation
{
    [CreateAssetMenu(fileName = "NavSetup", menuName = "OntoMatchGame/NavSetup", order = 0)]

    public class NavigationSetup : ScriptableObject
    {
        [field: SerializeField] public List<string> SetupScenes { get; private set; }

        public bool Contains(string sceneName)
        {
            return SetupScenes.Contains(sceneName);
        }
    }
}
