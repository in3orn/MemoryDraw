using System;
using System.Collections.Generic;

namespace Dev.Krk.MemoryDraw.Progress
{
    [Serializable]
    public class GroupProgressData
    {
        public string Name;
        public bool Unlocked;
        public List<DrawingProgressData> Drawings = new List<DrawingProgressData>();
    }
}