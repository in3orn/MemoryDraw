using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class ShapeData
    {
        public string Name;
        public PointData[] HorizontalFields;
        public PointData[] VerticalFields;
    }
}