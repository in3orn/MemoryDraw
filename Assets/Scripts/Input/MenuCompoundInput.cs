namespace Dev.Krk.MemoryDraw.Inputs
{
    public class MenuCompoundInput : MenuInput
    {
        private MenuInput[] inputs;


        void Awake()
        {
            inputs = GetComponentsInChildren<MenuInput>();
        }


        void OnEnable()
        {
            foreach (var input in inputs)
            {
                if (input != this)
                {
                    input.OnPrevActionLaunched += ProcessPrevActionLaunched;
                    input.OnNextActionLaunched += ProcessNextActionLaunched;
                    input.OnMainActionLaunched += ProcessMainActionLaunched;
                }
            }
        }

        void OnDisable()
        {
            foreach (var input in inputs)
            {
                if (input != null && input != this)
                {
                    input.OnPrevActionLaunched -= ProcessPrevActionLaunched;
                    input.OnNextActionLaunched -= ProcessNextActionLaunched;
                    input.OnMainActionLaunched -= ProcessMainActionLaunched;
                }
            }
        }

        private void ProcessPrevActionLaunched()
        {
            OnPrevActionLaunched();
        }

        private void ProcessNextActionLaunched()
        {
            OnNextActionLaunched();
        }

        private void ProcessMainActionLaunched()
        {
            OnMainActionLaunched();
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