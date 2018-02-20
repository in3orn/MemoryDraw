using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Common.Animations
{
    [RequireComponent(typeof(Animator))]
    public class ShowAnimator : MonoBehaviour
    {
        [SerializeField]
        private float showDelay;

        [SerializeField]
        private float hideDelay;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {

        }

        public void Show()
        {
            StartCoroutine(SetAnimatorBool("shown", true, showDelay));
        }

        public void Hide()
        {
            StartCoroutine(SetAnimatorBool("shown", false, hideDelay));
        }

        private IEnumerator SetAnimatorBool(string name, bool value, float delay)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            animator.SetBool(name, value);
        }
    }
}