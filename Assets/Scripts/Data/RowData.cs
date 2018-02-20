using System;

namespace Dev.Krk.MemoryDraw.Data
{
    [Serializable]
    public class RowData : ICloneable
    {
        public int[] Fields;

        public object Clone()
        {
            return new RowData
            {
                Fields = CloneFields(this.Fields)
            };
        }

        private int[] CloneFields(int[] fields)
        {
            int[] result = new int[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                result[i] = fields[i];
            }
            return result;
        }
    }
}