using System.Collections.Generic;

namespace Dev.Krk.MemoryDraw.Data.Initializers
{
    public class CompoundResourcesInitializer : ResourcesInitializer
    {
        private ResourcesInitializer[] initializers;

        void Awake()
        {
            List<ResourcesInitializer> initializers = new List<ResourcesInitializer>(GetComponentsInChildren<ResourcesInitializer>());
            initializers.Remove(this);
            this.initializers = initializers.ToArray();
        }

        void OnEnable()
        {
            foreach (ResourcesInitializer initializer in initializers)
            {
                initializer.OnInitialized += ProcessOnInitialized;
            }
        }

        void OnDisable()
        {
            foreach (ResourcesInitializer initializer in initializers)
            {
                initializer.OnInitialized -= ProcessOnInitialized;
            }
        }

        public override void Init()
        {
            foreach(ResourcesInitializer initializer in initializers)
            {
                initializer.Init();
            }
        }

        private void ProcessOnInitialized()
        {
            foreach (ResourcesInitializer initializer in initializers)
            {
                if (!initializer.Initialized) return;
            }

            initialized = true;
            if (OnInitialized != null) OnInitialized();
        }
    }
}