using UnityEngine;
using System.Collections.Generic;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.Level
{
    public class PathFinder
    {
        private enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        public float[] LengthToTurnsRatioBuckets;

        public int BucketSize;

        List<List<MapData>> buckets = new List<List<MapData>>();

        public List<MapData> FindPaths(int width, int height)
        {
            InitBuckets();

            MapData data = CreateData(width, height);
            FindPaths(data, 0, 0, Direction.None);

            List<MapData> result = new List<MapData>();
            foreach (var bucket in buckets)
            {
                result.AddRange(bucket);
            }
            return result;
        }

        public void Clear()
        {
            ClearBuckets();
        }

        private void InitBuckets()
        {
            ClearBuckets();

            for (int i = 0; i < LengthToTurnsRatioBuckets.Length; i++)
            {
                buckets.Add(new List<MapData>());
            }
        }

        private void ClearBuckets()
        {
            foreach (var bucket in buckets)
            {
                bucket.Clear();
            }
            buckets.Clear();
        }

        private MapData CreateData(int width, int height)
        {
            MapData data = new MapData
            {
                VerticalFields = CreateRows(width + 1, height),
                HorizontalFields = CreateRows(width, height + 1)
            };

            return data;
        }

        private RowData[] CreateRows(int width, int height)
        {
            RowData[] rows = new RowData[height];

            for (int i = 0; i < height; i++)
            {
                rows[i] = new RowData
                {
                    Fields = CreateFields(width)
                };
            }

            return rows;
        }

        private int[] CreateFields(int width)
        {
            int[] fields = new int[width];
            for (int j = 0; j < width; j++)
            {
                fields[j] = 0;
            }
            return fields;
        }

        private void FindPaths(MapData data, int posX, int posY, Direction prevDirection)
        {
            if (IsPathCompleted(data, posX, posY))
            {
                float ratio = data.PathLength / (float) data.NumOfTurns;
                for (int i = 0; i < LengthToTurnsRatioBuckets.Length; i++)
                {
                    float bucketRatio = LengthToTurnsRatioBuckets[i];
                    if (ratio <= bucketRatio)
                    {
                        data.RandomRank = Random.value;

                        var bucket = buckets[i];
                        if (bucket.Count < BucketSize)
                        {
                            bucket.Add(data);
                        }
                        else
                        {
                            int index = Random.Range(0, bucket.Count);
                            if (bucket[index].RandomRank < data.RandomRank)
                            {
                                bucket[index] = data;
                            }
                        }

                        break;
                    }
                }
            }
            else
            {
                // symmetry trick
                if (prevDirection == Direction.None)
                {
                    if (CanMoveRight(data, posX, posY))
                    {
                        MoveRight(data, posX, posY, prevDirection);
                    }
                }
                else
                {
                    Direction[] directions = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
                    Shuffle(directions);
                    foreach (var direction in directions)
                    {
                        switch (direction)
                        {
                            case Direction.Up:
                                if (CanMoveUp(data, posX, posY))
                                {
                                    MoveUp(data, posX, posY, prevDirection);
                                }
                                break;
                            case Direction.Down:
                                if (CanMoveDown(data, posX, posY))
                                {
                                    MoveDown(data, posX, posY, prevDirection);
                                }
                                break;
                            case Direction.Left:
                                if (CanMoveLeft(data, posX, posY))
                                {
                                    MoveLeft(data, posX, posY, prevDirection);
                                }
                                break;
                            case Direction.Right:
                                if (CanMoveRight(data, posX, posY))
                                {
                                    MoveRight(data, posX, posY, prevDirection);
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void Shuffle(Direction[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                Direction temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }
        }

        private bool IsPathCompleted(MapData data, int posX, int posY)
        {
            return posX == data.HorizontalFields[0].Fields.Length && posY == data.VerticalFields.Length;
        }

        private bool CanMoveUp(MapData data, int posX, int posY)
        {
            return posY > 0 && CanVisit(data, posX, posY - 1);
        }

        private bool CanMoveDown(MapData data, int posX, int posY)
        {
            return posY < data.VerticalFields.Length && CanVisit(data, posX, posY + 1);
        }

        private bool CanMoveLeft(MapData data, int posX, int posY)
        {
            return posX > 0 && CanVisit(data, posX - 1, posY);
        }

        private bool CanMoveRight(MapData data, int posX, int posY)
        {
            return posX < data.HorizontalFields[0].Fields.Length && CanVisit(data, posX + 1, posY);
        }

        private bool CanVisit(MapData data, int pointX, int pointY)
        {
            if (pointX > 0 && data.HorizontalFields[pointY].Fields[pointX - 1] != 0) return false;
            if (pointX < data.HorizontalFields[0].Fields.Length && data.HorizontalFields[pointY].Fields[pointX] != 0) return false;

            if (pointY > 0 && data.VerticalFields[pointY - 1].Fields[pointX] != 0) return false;
            if (pointY < data.VerticalFields.Length && data.VerticalFields[pointY].Fields[pointX] != 0) return false;

            return true;
        }

        private void MoveUp(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Up);
            newData.VerticalFields[posY - 1].Fields[posX] = 1;
            FindPaths(newData, posX, posY - 1, Direction.Up);
        }

        private void MoveDown(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Down);
            newData.VerticalFields[posY].Fields[posX] = 1;
            FindPaths(newData, posX, posY + 1, Direction.Down);
        }

        private void MoveLeft(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Left);
            newData.HorizontalFields[posY].Fields[posX - 1] = 1;
            FindPaths(newData, posX - 1, posY, Direction.Left);
        }

        private void MoveRight(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Right);
            newData.HorizontalFields[posY].Fields[posX] = 1;
            FindPaths(newData, posX + 1, posY, Direction.Right);
        }

        private MapData CreateNewData(MapData data, Direction prevDirection, Direction direction)
        {
            MapData result = data.Clone() as MapData;
            result.PathLength++;
            if (prevDirection != Direction.None && prevDirection != direction)
            {
                result.NumOfTurns++;
            }
            return result;
        }
    }
}