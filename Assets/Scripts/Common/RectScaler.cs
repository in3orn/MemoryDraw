using System.Collections;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Common
{
    [RequireComponent(typeof(RectTransform))]
    public class RectScaler : MonoBehaviour
    {
        [SerializeField]
        private float duration;


        private RectTransform rectTransform;

        private Vector3 targetScale = Vector3.negativeInfinity;


        public Vector3 TargetScale
        {
            set {
                if (targetScale != value)
                {
                    targetScale = value;
                    StopAllCoroutines();
                    StartCoroutine(Scale());
                }
            }
        }


        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private IEnumerator Scale()
        {
            Vector3 start = rectTransform.localScale;
            float time = 0f;

            while (time < duration)
            {
                rectTransform.localScale = Vector3.Lerp(start, targetScale, time / duration);
                time += Time.deltaTime;

                yield return null;
            }

            rectTransform.localScale = targetScale;
        }
    }
}