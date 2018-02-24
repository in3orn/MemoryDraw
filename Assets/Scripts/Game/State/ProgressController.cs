using UnityEngine;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Progress;
using System.Collections.Generic;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        private readonly string GROUP = "Group";

        private readonly string DRAWING = "Drawing";

        private readonly string MAP = "Map";

        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private GameProgressDataInitializer gameProgressDataInitializer;

        [SerializeField]
        private LivesController livesController;


        private int group;

        private int drawing;

        private int map;


        private GameProgressData gameProgressData;


        public int Group { get { return group; } }

        public int Drawing { get { return drawing; } }

        public int Map { get { return map; } }


        public GroupProgressData GetGroupData(int i)
        {
            return gameProgressData.Groups[i];
        }


        void OnEnable()
        {
            groupsDataInitializer.OnInitialized += ProcessConfigInitialized;
        }

        void OnDisable()
        {
            if(groupsDataInitializer != null)
            {
                groupsDataInitializer.OnInitialized -= ProcessConfigInitialized;
            }
        }

        private void ProcessConfigInitialized()
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
            PlayerPrefs.SetInt(GROUP, group); //TODO switch to names as ordering can be modified within updates!!
            PlayerPrefs.SetInt(DRAWING, drawing);
            PlayerPrefs.SetInt(MAP, map);

            gameProgressDataInitializer.Save(gameProgressData);
        }

        private void LoadData()
        {
            group = PlayerPrefs.GetInt(GROUP);
            drawing = PlayerPrefs.GetInt(DRAWING);
            map = PlayerPrefs.GetInt(MAP);

            gameProgressData = gameProgressDataInitializer.Load();

            Adjust(gameProgressData, groupsDataInitializer.Data);
        }
        
        private void Adjust(GameProgressData gameProgressData, GameData gameConfigData)
        {
            List<GroupProgressData> orderedData = new List<GroupProgressData>(gameConfigData.Groups.Length);
            foreach (var groupConfigData in gameConfigData.Groups)
            {
                GroupProgressData groupProgressData = GetByName(gameProgressData.Groups, groupConfigData.Name);
                Adjust(groupProgressData, groupConfigData);
                orderedData.Add(groupProgressData);
            }

            gameProgressData.Groups.Clear();
            gameProgressData.Groups.AddRange(orderedData);
        }
        
        private void Adjust(GroupProgressData groupProgressData, GroupData groupConfigData)
        {
            if (!groupProgressData.Unlocked)
            {
                groupProgressData.Unlocked = groupConfigData.Unlocked;
            }

            List<DrawingProgressData> orderedData = new List<DrawingProgressData>(groupConfigData.Drawings.Length);
            foreach (var drawingConfigData in groupConfigData.Drawings)
            {
                DrawingProgressData drawingProgressData = GetByName(groupProgressData.Drawings, drawingConfigData.Name);
                Adjust(drawingProgressData, drawingConfigData);
                orderedData.Add(drawingProgressData);
            }

            groupProgressData.Drawings.Clear();
            groupProgressData.Drawings.AddRange(orderedData);
        }

        private void Adjust(DrawingProgressData drawingProgressData, DrawingData drawingConfigData)
        {
        }

        private GroupProgressData GetByName(List<GroupProgressData> list, string name)
        {
            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            return new GroupProgressData()
            {
                Name = name
            };
        }

        private DrawingProgressData GetByName(List<DrawingProgressData> list, string name) //TODO refactor: name based classes
        {
            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            return new DrawingProgressData()
            {
                Name = name
            };
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
            GroupProgressData groupProgressData = gameProgressData.Groups[group];
            DrawingProgressData drawingProgressData = groupProgressData.Drawings[drawing];

            drawingProgressData.Completed = true;
            drawingProgressData.Stars = livesController.Lives;

            SaveData();
        }
    }
}