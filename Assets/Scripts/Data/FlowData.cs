using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class FlowData
    {
        public int Level;

        public LevelData[] Levels;
    }
}