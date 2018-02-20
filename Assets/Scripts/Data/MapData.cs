using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class MapData : ICloneable
    {
        public int PathLength;
        public int NumOfTurns;

        public RowData[] HorizontalFields;
        public RowData[] VerticalFields;

        [NonSerialized]
        public float RandomRank;

        public object Clone()
        {
            return new MapData
            {
                PathLength = this.PathLength,
                NumOfTurns = this.NumOfTurns,
                HorizontalFields = CloneFields(this.HorizontalFields),
                VerticalFields = CloneFields(this.VerticalFields)
            };
        }

        private RowData[] CloneFields(RowData[] fields)
        {
            RowData[] result = new RowData[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                result[i] = fields[i].Clone() as RowData;
            }
            return result;
        }
    }
}