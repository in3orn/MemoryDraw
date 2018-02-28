using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game
{
    public class Field : MonoBehaviour
    {
        public static readonly float SIZE = 1.28f;

        public enum TypeEnum
        {
            Horizontal = 0,
            Vertical
        }

        private enum StateEnum
        {
            Hidden = 0,
            Shown,
            Masked,
            Visited,
            Broken
        }

        private StateEnum state;

        private StateEnum State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    animator.SetInteger("state", (int)state);
                }
            }
        }

        private TypeEnum type;

        public TypeEnum Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    float rotation = type == TypeEnum.Vertical ? 90f : 0f;
                    rotator.rotation = Quaternion.Euler(0f, 0f, rotation);
                }
            }
        }

        private bool valid;

        public bool Valid { get { return valid; } }

        public bool Broken { get { return State == StateEnum.Broken; } }

        public bool Masked { get { return State == StateEnum.Masked; } }

        public bool Hidden { get { return State == StateEnum.Hidden; } }

        public bool Visited { get { return State == StateEnum.Visited; } }

        [SerializeField]
        private Transform rotator;
        
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(Vector2 position, bool valid)
        {
            transform.position = position;
            this.valid = valid;
        }

        public void Show()
        {
            if (valid && State == StateEnum.Hidden)
            {
                State = StateEnum.Shown;
            }
        }

        public void Mask()
        {
            if (State == StateEnum.Hidden || State == StateEnum.Shown)
            {
                State = StateEnum.Masked;
            }
        }

        public void Visit(Vector3 from)
        {
            if (valid && State == StateEnum.Masked || State == StateEnum.Shown)
            {
                bool reversed = from.x - transform.position.x > Field.SIZE / 4f || from.y - transform.position.y > Field.SIZE / 4f;
                animator.SetBool("reversed", reversed);
                State = StateEnum.Visited;
            }
        }

        public void Break()
        {
            if (State == StateEnum.Masked || State == StateEnum.Shown || State == StateEnum.Visited)
            {
                State = StateEnum.Broken;
            }

        }

        public void Hide()
        {
            State = StateEnum.Hidden;
        }
    }
}