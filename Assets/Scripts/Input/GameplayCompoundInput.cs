namespace Dev.Krk.MemoryDraw.Inputs
{
    public class GameplayCompoundInput : GameplayInput
    {
        private GameplayInput[] inputs;


        void Awake()
        {
            inputs = GetComponentsInChildren<GameplayInput>();
        }


        void OnEnable()
        {
            foreach (var input in inputs)
            {
                if (input != this)
                {
                    input.OnMoveUpActionLaunched += ProcessMoveUpActionLaunched;
                    input.OnMoveDownActionLaunched += ProcessMoveDownActionLaunched;
                    input.OnMoveLeftActionLaunched += ProcessMoveLeftActionLaunched;
                    input.OnMoveRightActionLaunched += ProcessMoveRightActionLaunched;
                }
            }
        }

        void OnDisable()
        {
            foreach (var input in inputs)
            {
                if (input != null && input != this)
                {
                    input.OnMoveUpActionLaunched -= ProcessMoveUpActionLaunched;
                    input.OnMoveDownActionLaunched -= ProcessMoveDownActionLaunched;
                    input.OnMoveLeftActionLaunched -= ProcessMoveLeftActionLaunched;
                    input.OnMoveRightActionLaunched -= ProcessMoveRightActionLaunched;
                }
            }
        }

        private void ProcessMoveUpActionLaunched()
        {
            OnMoveUpActionLaunched();
        }

        private void ProcessMoveDownActionLaunched()
        {
            OnMoveDownActionLaunched();
        }

        private void ProcessMoveLeftActionLaunched()
        {
            OnMoveLeftActionLaunched();
        }

        private void ProcessMoveRightActionLaunched()
        {
            OnMoveRightActionLaunched();
        }


        protected override bool IsSupported()
        {
            return true;
        }

        protected override void UpdateInput()
        {

        }
    }
}