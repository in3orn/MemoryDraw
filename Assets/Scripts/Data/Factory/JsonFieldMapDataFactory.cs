using UnityEngine;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;

public class JsonFieldMapDataFactory : MonoBehaviour
{
    public FieldMapData Create(MapData mapData)
    {
        int[,] horizontalFields = RowsToArray(mapData.HorizontalFields);
        int[,] verticalFields = RowsToArray(mapData.VerticalFields);

        return new FieldMapData(horizontalFields, verticalFields);
    }

    private int[,] RowsToArray(RowData[] rows)
    {
        int rowSize = rows.Length;
        int colSize = rows[0].Fields.Length;

        int[,] result = new int[rowSize, colSize];

        for (int i = 0; i < rowSize; i++)
        {
            RowData row = rows[i];
            for (int j = 0; j < colSize; j++)
            {
                result[i, j] = row.Fields[j];
            }
        }

        return result;
    }
}
