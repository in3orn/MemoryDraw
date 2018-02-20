using UnityEngine;
using System.Collections;
using Dev.Krk.MemoryDraw.Common;
using Dev.Krk.MemoryDraw.Game;

namespace Dev.Krk.MemoryDraw.Misc
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {

        [Header("Fixed width settings")]
        [SerializeField]
        private FloatRange fixedWidthRatio;

        [SerializeField]
        private FloatRange mapSize;


        [Header("Zoom settings")]
        [SerializeField]
        private float zoomDelay;

        [SerializeField]
        private float zoomDuration;

        private float initialZoom;

        private float zoomFactor;


        [Header("Dependencies")]
        [SerializeField]
        private LevelController level;

        private Camera myCamera;


        void Awake()
        {
            myCamera = GetComponent<Camera>();
            InitZoomParams();
        }

        private void InitZoomParams()
        {
            float h2w = Screen.height / (float) Screen.width;
            FloatRange ortoSize = new FloatRange
            {
                Min = fixedWidthRatio.Min * h2w,
                Max = fixedWidthRatio.Max * h2w
            };

            zoomFactor = (ortoSize.Max - ortoSize.Min) / (mapSize.Max - mapSize.Min);
            initialZoom = ortoSize.Min - mapSize.Min * zoomFactor;

            myCamera.orthographicSize = ortoSize.Min;
        }

        void Start()
        {
            level.OnLevelStarted += Adapt;
        }

        private void Adapt()
        {
            int size = Mathf.Max(level.HorizontalLength, level.VerticalLength);
            SetZoom(initialZoom + size * zoomFactor);
        }

        public void SetZoom(float zoom)
        {
            StartCoroutine(setZoom(myCamera.orthographicSize, zoom, zoomDelay, zoomDuration));
        }

        private IEnumerator setZoom(float from, float to, float delay, float duration)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            float t = 0f;
            while (true)
            {
                t += Time.deltaTime / duration;
                myCamera.orthographicSize = Mathf.Lerp(from, to, t);

                if (t >= 1) break;
                yield return null;
            }
        }
    }
}