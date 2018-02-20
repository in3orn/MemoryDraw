using UnityEngine;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.Level
{
    //TODO no longer needed :P
    public class MapDataProvider : MonoBehaviour
    {
        [SerializeField]
        private JsonFieldMapDataFactory mapDataFactory;

        public FieldMapData GetMapData(MapData mapData)
        {
            return mapDataFactory.Create(mapData);
        }
    }

}