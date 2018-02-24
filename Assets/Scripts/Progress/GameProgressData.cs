using System;
using System.Collections.Generic;

namespace Dev.Krk.MemoryDraw.Progress
{
    [Serializable]
    public class GameProgressData
    {
        public List<GroupProgressData> Groups = new List<GroupProgressData>();
    }
}