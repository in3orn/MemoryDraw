using UnityEngine;

namespace Dev.Krk.MemoryDraw.Summary
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField]
        private PopUpCanvas settings;

        void Start()
        {
        }

        public void Show()
        {
            settings.Show();
        }

        public void Hide()
        {
            settings.Hide();
        }
    }
}