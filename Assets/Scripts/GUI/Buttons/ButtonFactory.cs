using UnityEngine;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public abstract class ButtonFactory : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private GameObject template;


        public abstract ButtonController[] CreateButtons(int parentId);
        

        protected GameObject CreateButton()
        {
            return Instantiate(template);
        }
    }
}