﻿using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.Resources
{
    public class ResourcesInitializer : MonoBehaviour
    {
        public UnityAction OnInitialized;

        protected bool initialized;

        public bool Initialized { get { return initialized; } }

        public virtual void Init()
        {
            initialized = true;
        }
    }
}
