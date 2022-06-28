using System.Collections;
using UnityEngine;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [RequireComponent(typeof(TMP_InputField))]
    [RequireComponent(typeof(Animator))]
    public class CustomInputField : MonoBehaviour
    {
        [Header("Resources")]
        public TMP_InputField inputText;
        public Animator inputFieldAnimator;

        // Hidden variables
        private string inAnim = "In";
        private string outAnim = "Out";
        private string instaInAnim = "Instant In";
        private string instaOutAnim = "Instant Out";

        void Start()
        {
            if (inputText == null) { inputText = gameObject.GetComponent<TMP_InputField>(); }
            if (inputFieldAnimator == null) { inputFieldAnimator = gameObject.GetComponent<Animator>(); }

            inputText.onSelect.AddListener(delegate { AnimateIn(); });
            inputText.onEndEdit.AddListener(delegate { AnimateOut(); });
            UpdateStateInstant();
        }

        void OnEnable()
        {
            if (inputText == null)
                return;

            inputText.ForceLabelUpdate();
            UpdateStateInstant();

            if (gameObject.activeInHierarchy == true) { StartCoroutine("DisableAnimator"); }
        }

        public void AnimateIn() 
        {
            StopCoroutine("DisableAnimator");
            inputFieldAnimator.enabled = true;
            inputFieldAnimator.Play(inAnim);
        }

        public void AnimateOut()
        {
            inputFieldAnimator.enabled = true;
            
            if (inputText.text.Length == 0) { inputFieldAnimator.Play(outAnim); }
          
            StartCoroutine("DisableAnimator");
        }

        public void UpdateState()
        {
            inputFieldAnimator.enabled = true;

            if (inputText.text.Length == 0) { AnimateOut(); }
            else { AnimateIn(); }

            StartCoroutine("DisableAnimator");
        }

        public void UpdateStateInstant()
        {
            inputFieldAnimator.enabled = true;

            if (inputText.text.Length == 0) { inputFieldAnimator.Play(instaOutAnim); }
            else { inputFieldAnimator.Play(instaInAnim); }

            StartCoroutine("DisableAnimator");
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSeconds(0.5f);
            inputFieldAnimator.enabled = false;
        }
    }
}