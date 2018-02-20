using Dev.Krk.MemoryDraw.Game;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Output
{
    public class VibrationController : MonoBehaviour
    {
        [SerializeField]
        private LevelController levelController;

        void Start()
        {

        }

        void OnEnable()
        {
            levelController.OnPlayerFailed += ProcessPlayerFailed;
        }

        void OnDisable()
        {
            if (levelController != null)
            {
                levelController.OnPlayerFailed -= ProcessPlayerFailed;
            }
        }

        private void ProcessPlayerFailed()
        {
            Handheld.Vibrate();
        }
    }
}