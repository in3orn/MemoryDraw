using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class ButtonController : MonoBehaviour
    {
        public UnityAction<ButtonController> OnClicked;


        private int id;

        protected bool locked;


        public int Id { get { return id; } }

        public bool Locked { get { return locked; } }


        public void Click()
        {
            OnClicked(this);
        }

        public void Init(int id)
        {
            this.id = id;
        }
    }

}