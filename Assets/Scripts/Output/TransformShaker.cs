using Dev.Krk.MemoryDraw.Game;
using System.Collections;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Output
{
    public class TransformShaker : MonoBehaviour
    {
        [SerializeField]
        private float interval;

        [SerializeField]
        private float duration;

        [SerializeField]
        private Rect shakeArea;

        [SerializeField]
        private LevelController levelController;

        private Vector3 startPosition;

        void Awake()
        {
            startPosition = transform.localPosition;
        }

        void Start()
        {

        }

        void OnEnable()
        {
            levelController.OnPlayerFailed += ProcessPlayerFailed;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnPlayerFailed -= ProcessPlayerFailed;
            }
        }

        private void ProcessPlayerFailed()
        {
            StartCoroutine(Shake());
        }

        private IEnumerator Shake()
        {
            float time = 0f;
            while(time < duration)
            {
                float rx = Random.Range(shakeArea.xMin, shakeArea.xMax);
                float ry = Random.Range(shakeArea.yMin, shakeArea.yMax);

                transform.localPosition = startPosition + new Vector3(rx, ry, 0f);

                time += interval;

                yield return new WaitForSeconds(interval);
            }

            transform.localPosition = startPosition;
        }
    }
}