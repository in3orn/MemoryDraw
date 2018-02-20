using UnityEngine;

namespace Assets.Scripts.Game.State
{
    class RandomController : MonoBehaviour
    {
        void Awake()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
    }
}
