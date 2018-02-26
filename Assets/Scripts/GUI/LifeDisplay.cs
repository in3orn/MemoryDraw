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
        private ProgressController progressController;


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
            if (!shown && livesController.Lives > lifeIndex && (progressController.GroupIndex > 0 || progressController.DrawingIndex > 0))
            {
                animator.SetTrigger("Show");
                shown = true;
            }
            else if (shown && (livesController.Lives <= lifeIndex || (progressController.GroupIndex <= 0 && progressController.DrawingIndex <= 0)))
            {
                animator.SetTrigger("Hide");
                shown = false;
            }
        }
    }
}
