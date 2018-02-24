using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class GameplayInput : GameStateDependentInput
    {
        public UnityAction OnMoveUpActionLaunched;
        public UnityAction OnMoveDownActionLaunched;
        public UnityAction OnMoveLeftActionLaunched;
        public UnityAction OnMoveRightActionLaunched;
    }
}