using UnityEngine;
using System.Collections.Generic;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;

public class JsonFieldMapDataFactory : MonoBehaviour
{
    [SerializeField]
    private MapsDataInitializer mapsDataInitializer;

    public FieldMapData Create(LevelData levelData)
    {
        List<MapData> maps = FilterMatchingMaps(levelData);

        int index = Random.Range(0, maps.Count);
        MapData mapData = maps[index];

        Debug.Log("Map data:" + index + " / " + maps.Count);

        int[,] horizontalFields = RowsToArray(mapData.HorizontalFields);
        int[,] verticalFields = RowsToArray(mapData.VerticalFields);

        return new FieldMapData(horizontalFields, verticalFields);
    }

    private List<MapData> FilterMatchingMaps(LevelData levelData)
    {
        List<MapData> result = new List<MapData>();

        Vector2 minMaxDifficulty = CalculateMinMaxDifficulty(levelData.Width, levelData.Height);

        foreach (MapData mapData in mapsDataInitializer.Data.Maps)
        {
            if (IsMatchingSize(mapData, levelData.Width, levelData.Height))
            {
                if (minMaxDifficulty.y > minMaxDifficulty.x)
                {
                    float difficulty = CalculateDifficulty(mapData);
                    float normalizedDifficulty = (difficulty - minMaxDifficulty.x) / (minMaxDifficulty.y - minMaxDifficulty.x);
                    if (levelData.MinDifficulty <= normalizedDifficulty && normalizedDifficulty <= levelData.MaxDifficulty)
                    {
                        result.Add(mapData);
                    }
                }
                else
                {
                    result.Add(mapData);
                }

            }
        }

        return result;
    }

    private Vector2 CalculateMinMaxDifficulty(int width, int height)
    {
        float min = Mathf.Infinity;
        float max = Mathf.NegativeInfinity;
        foreach (MapData mapData in mapsDataInitializer.Data.Maps)
        {
            if (IsMatchingSize(mapData, width, height))
            {
                float difficulty = CalculateDifficulty(mapData);
                if (difficulty < min)
                    min = difficulty;
                if (difficulty > max)
                    max = difficulty;
            }
        }
        return new Vector2(min, max);
    }

    private bool IsMatchingSize(MapData mapData, int width, int height)
    {
        return mapData.HorizontalFields[0].Fields.Length == width && mapData.VerticalFields.Length == height;
    }

    private float CalculateDifficulty(MapData mapData)
    {
        return mapData.NumOfTurns / (float)mapData.PathLength;
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
