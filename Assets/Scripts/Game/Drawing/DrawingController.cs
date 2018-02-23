using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using System;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game.Drawing
{
    [RequireComponent(typeof(Animator))]
    public class DrawingController : MonoBehaviour
    {
        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;


        private SpriteRenderer spriteRenderer;

        private Animator animator;


        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }


        public void Show()
        {
            animator.SetBool("shown", true);
        }

        public void Hide()
        {
            animator.SetBool("shown", false);
        }

        public void SetDrawing(int group, int drawing)
        {
            GroupData groupData = groupsDataInitializer.Data.Groups[group];
            DrawingData drawingData = groupData.Drawings[drawing];

            spriteRenderer.sprite = Resources.Load<Sprite>("Drawings/" + drawingData.Image);
        }
    }

}