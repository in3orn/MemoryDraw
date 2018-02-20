namespace Dev.Krk.MemoryDraw.Data
{
    public class FieldMapData
    {
        private int[,] horizontalFields;
        private int[,] verticalFields;

        public FieldMapData(int[,] horizontalFields, int[,] verticalFields)
        {
            this.horizontalFields = horizontalFields;
            this.verticalFields = verticalFields;
        }

        public int[,] HorizontalFields { get { return horizontalFields; } }
        public int[,] VerticalFields { get { return verticalFields; } }

        public void ReflectByDiagonal()
        {
            int si = horizontalFields.GetLength(0);
            int sj = horizontalFields.GetLength(1);
            for (int i = 0; i < si; i++)
            {
                for (int j = 0; j < sj; j++)
                {
                    int temp = horizontalFields[i, j];
                    horizontalFields[i, j] = verticalFields[j, i];
                    verticalFields[j, i] = temp;
                }
            }
        }

        public void ReflectByContrdiagonal()
        {
            int si = horizontalFields.GetLength(0);
            int sj = horizontalFields.GetLength(1);
            for (int i = 0; i < si; i++)
            {
                for (int j = 0; j < sj; j++)
                {
                    int temp = horizontalFields[i, j];
                    horizontalFields[i, j] = verticalFields[sj - j - 1, si - i - 1];
                    verticalFields[sj - j - 1, si - i - 1] = temp;
                }
            }
        }
        
        public void ReflectByCenter()
        {
            int si = horizontalFields.GetLength(0);
            int sj = horizontalFields.GetLength(1);
            for (int i = 0; i < si; i++)
            {
                for (int j = 0; j < sj; j++)
                {
                    if (i < si / 2 || (i == si - i - 1 && j < sj / 2))
                    {
                        int temp = horizontalFields[i, j];
                        horizontalFields[i, j] = horizontalFields[si - i - 1, sj - j - 1];
                        horizontalFields[si - i - 1, sj - j - 1] = temp;
                    }
                }
            }

            si = verticalFields.GetLength(0);
            sj = verticalFields.GetLength(1);
            for (int i = 0; i < si; i++)
            {
                for (int j = 0; j < sj; j++)
                {
                    if (j < sj / 2 || (j == sj - j - 1 && i < si / 2))
                    {
                        int temp = verticalFields[i, j];
                        verticalFields[i, j] = verticalFields[si - i - 1, sj - j - 1];
                        verticalFields[si - i - 1, sj - j - 1] = temp;
                    }
                }
            }
        }
    }
}