using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game.Animations
{
    [RequireComponent(typeof(Animator))]
    public class ArrowAnimator : MonoBehaviour
    {
        private enum DirectionEnum
        {
            Up = 0,
            Left,
            Right,
            Down
        }

        [SerializeField]
        private DirectionEnum direction;

        [SerializeField]
        private LevelController levelController;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {

        }

        void Update()
        {
            bool valid = IsValid();
            if(animator.GetBool("valid") != valid)
            {
                animator.SetBool("valid", valid);
            }
        }

        private bool IsValid()
        {
            switch (direction)
            {
                case DirectionEnum.Up:
                    return levelController.CanMoveUp();
                case DirectionEnum.Left:
                    return levelController.CanMoveLeft();
                case DirectionEnum.Right:
                    return levelController.CanMoveRight();
                case DirectionEnum.Down:
                    return levelController.CanMoveDown();
                default:
                    return false;
            }
        }
    }
}