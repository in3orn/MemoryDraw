using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class ButtonController : MonoBehaviour
    {
        public UnityAction<int> OnClicked;


        private int id;


        public int Id { get { return id; } }
        

        public void Click()
        {
            OnClicked(Id);
        }

        public void Init(int id)
        {
            this.id = id;
        }
    }

}