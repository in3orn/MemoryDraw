using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class MapData
    {
        public RowData[] HorizontalFields;
        public RowData[] VerticalFields;

        public PointData Start;
        public PointData Finish;

        public PointData Offset;
    }
}