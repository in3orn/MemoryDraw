using UnityEngine;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.Level
{
    public class SinglePathFinder
    {
        private enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        private Direction[] directions = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };

        public MapData FindPath(int width, int height)
        {
            MapData data = CreateData(width, height);
            return FindPath(data, 0, 0, Direction.None);
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

        private MapData FindPath(MapData data, int posX, int posY, Direction prevDirection)
        {
            if (IsPathCompleted(data, posX, posY))
            {
                return data;
            }
            else
            {
                // symmetry trick
                if (prevDirection == Direction.None)
                {
                    if (CanMoveRight(data, posX, posY))
                    {
                        MapData result = MoveRight(data, posX, posY, prevDirection);
                        if (result != null) return result;
                    }
                }
                else
                {
                    Shuffle(directions);
                    foreach (var direction in directions)
                    {
                        switch (direction)
                        {
                            case Direction.Up:
                                if (CanMoveUp(data, posX, posY))
                                {
                                    MapData result = MoveUp(data, posX, posY, prevDirection);
                                    if (result != null) return result;
                                }
                                break;
                            case Direction.Down:
                                if (CanMoveDown(data, posX, posY))
                                {
                                    MapData result = MoveDown(data, posX, posY, prevDirection);
                                    if (result != null) return result;
                                }
                                break;
                            case Direction.Left:
                                if (CanMoveLeft(data, posX, posY))
                                {
                                    MapData result = MoveLeft(data, posX, posY, prevDirection);
                                    if (result != null) return result;
                                }
                                break;
                            case Direction.Right:
                                if (CanMoveRight(data, posX, posY))
                                {
                                    MapData result = MoveRight(data, posX, posY, prevDirection);
                                    if (result != null) return result;
                                }
                                break;
                        }
                    }
                }
                return null;
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

        private MapData MoveUp(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Up);
            newData.VerticalFields[posY - 1].Fields[posX] = 1;
            return FindPath(newData, posX, posY - 1, Direction.Up);
        }

        private MapData MoveDown(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Down);
            newData.VerticalFields[posY].Fields[posX] = 1;
            return FindPath(newData, posX, posY + 1, Direction.Down);
        }

        private MapData MoveLeft(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Left);
            newData.HorizontalFields[posY].Fields[posX - 1] = 1;
            return FindPath(newData, posX - 1, posY, Direction.Left);
        }

        private MapData MoveRight(MapData data, int posX, int posY, Direction prevDirection)
        {
            MapData newData = CreateNewData(data, prevDirection, Direction.Right);
            newData.HorizontalFields[posY].Fields[posX] = 1;
            return FindPath(newData, posX + 1, posY, Direction.Right);
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