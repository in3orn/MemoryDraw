using System.Collections.Generic;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.Level
{
    public class BucketPathFinder
    {
        public float[] LengthToTurnsRatioBuckets;

        public int BucketSize;

        private List<List<MapData>> buckets = new List<List<MapData>>();

        private SinglePathFinder singlePathFinder;

        private PathFinder pathFinder;

        public BucketPathFinder(PathFinder pathFinder, SinglePathFinder singlePathFinder)
        {
            this.pathFinder = pathFinder;
            this.singlePathFinder = singlePathFinder;
        }

        public List<MapData> FindPaths(int width, int height)
        {
            if (height < 4)
            {
                pathFinder.LengthToTurnsRatioBuckets = LengthToTurnsRatioBuckets;
                pathFinder.BucketSize = BucketSize;

                return pathFinder.FindPaths(width, height);
            }
            else
            {
                InitBuckets();
                FillBuckets(width, height);

                return GetPathsFromBuckets();
            }
        }

        public void Clear()
        {
            ClearBuckets();
            pathFinder.Clear();
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

        private void FillBuckets(int width, int height)
        {
            while(!AreBucketsFull())
            {
                MapData data = singlePathFinder.FindPath(width, height);
                if (data != null)
                {
                    float ratio = data.PathLength / data.NumOfTurns;
                    for (int i = 0; i < LengthToTurnsRatioBuckets.Length; i++)
                    {
                        var bucket = buckets[i];
                        if (bucket.Count < BucketSize && ratio < LengthToTurnsRatioBuckets[i])
                        {
                            bucket.Add(data);
                        }
                    }
                }
            }
        }

        private bool AreBucketsFull()
        {
            foreach (var bucket in buckets)
            {
                if (bucket.Count < BucketSize) return false;
            }
            return true;
        }

        private List<MapData> GetPathsFromBuckets()
        {
            List<MapData> result = new List<MapData>();
            foreach (var bucket in buckets)
            {
                result.AddRange(bucket);
            }
            return result;
        }
    }
}