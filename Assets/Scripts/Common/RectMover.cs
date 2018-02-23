using System.Collections;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Common
{
    [RequireComponent(typeof(RectTransform))]
    public class RectMover : MonoBehaviour
    {
        [SerializeField]
        private float duration;


        private RectTransform rectTransform;

        private Vector2 target = Vector2.negativeInfinity;


        public Vector2 Target
        {
            set {
                if (target != value)
                {
                    target = value;
                    StopAllCoroutines();
                    StartCoroutine(Move());
                }
            }
        }


        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private IEnumerator Move()
        {
            Vector2 start = rectTransform.anchoredPosition;
            float time = 0f;

            while (time < duration)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(start, target, time / duration);
                time += Time.deltaTime;

                yield return null;
            }

            rectTransform.anchoredPosition = target;
        }
    }
}