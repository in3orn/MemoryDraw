using UnityEngine;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Data;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        private readonly string GROUP = "Group";

        private readonly string DRAWING = "Drawing";

        private readonly string MAP = "Map";

        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;


        private int group;

        private int drawing;

        private int map;


        public int Group { get { return group; } }

        public int Drawing { get { return drawing; } }

        public int Map { get { return map; } }


        void Start()
        {
            LoadData();
        }


        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        void OnApplicationQuit()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            PlayerPrefs.SetInt(GROUP, group);
            PlayerPrefs.SetInt(DRAWING, drawing);
            PlayerPrefs.SetInt(MAP, map);
        }

        private void LoadData()
        {
            group = PlayerPrefs.GetInt(GROUP);
            drawing = PlayerPrefs.GetInt(DRAWING);
            map = PlayerPrefs.GetInt(MAP);
        }


        public void StartDrawing(int group, int drawing)
        {
            this.group = group;
            this.drawing = drawing;
            map = 0;
        }

        public void NextMap()
        {
            map++;
        }

        public bool IsDrawingCompleted()
        {
            return map == groupsDataInitializer.Data.Groups[group].Drawings[drawing].Maps.Length;
        }

        public void FinishDrawing()
        {
            group = -1;
            drawing = -1;
            map = -1;
        }
    }
}