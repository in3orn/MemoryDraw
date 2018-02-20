using UnityEngine;
using UnityEngine.UI;
using Dev.Krk.MemoryDraw.Game.State;

namespace Dev.Krk.MemoryDraw.Game.GUI
{
    public class LifeDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private int lifeIndex;

        [Header("Dependencies")]
        [SerializeField]
        private LivesController livesController;

        [SerializeField]
        private ScoreController scoreController;


        private Animator animator;

        private bool shown;


        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            UpdateLife();
        }


        void OnEnable()
        {
            livesController.OnLivesUpdated += UpdateLife;
        }

        void OnDisable()
        {
            if (livesController != null)
            {
                livesController.OnLivesUpdated -= UpdateLife;
            }
        }

        private void UpdateLife()
        {
            if (!shown && scoreController.Level > 0 && livesController.Lives > lifeIndex)
            {
                animator.SetTrigger("Show");
                shown = true;
            }
            else if (shown && livesController.Lives <= lifeIndex)
            {
                animator.SetTrigger("Hide");
                shown = false;
            }
        }
    }
}
