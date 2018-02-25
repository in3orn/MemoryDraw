using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class DrawingData
    {
        public string Name;
        public string Image;

        public MapData[] Maps;
    }
}