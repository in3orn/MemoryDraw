using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.Inputs
{
    public abstract class MenuInput : GameStateDependentInput
    {
        public UnityAction OnPrevActionLaunched;
        public UnityAction OnNextActionLaunched;
        public UnityAction OnMainActionLaunched;
    }
}