using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.GUI
{
    [RequireComponent(typeof(Image))]
    public class StateButton : MonoBehaviour
    {
        public UnityAction<int> OnStateChanged;

        [SerializeField]
        private Sprite[] sprites;

        private Image image;

        private int state;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void Start()
        {
            image.sprite = sprites[state];
        }

        public void SetState(int state)
        {
            this.state = state;
            image.sprite = sprites[state];
        }

        public void NextState()
        {
            state++;
            state %= sprites.Length;
            image.sprite = sprites[state];

            if (OnStateChanged != null)
                OnStateChanged(state);
        }
    }
}