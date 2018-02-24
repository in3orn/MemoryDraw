using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class GroupData
    {
        public string Name;
        public string[] Images;
        public DrawingData[] Drawings;
    }
}