using UnityEngine;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Progress;
using System.Collections.Generic;

namespace Dev.Krk.MemoryDraw.Game.State
{
    public class ProgressController : MonoBehaviour
    {
        private readonly string GROUP_NAME = "Group";

        private readonly string DRAWING_NAME = "Drawing";

        private readonly string MAP_INDEX = "Map";


        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private GameProgressDataInitializer gameProgressDataInitializer;

        [SerializeField]
        private LivesController livesController;


        private int groupIndex;

        private int drawingIndex;

        private int mapIndex;


        private GameProgressData gameProgressData;


        public int GroupIndex { get { return groupIndex; } }

        public int DrawingIndex { get { return drawingIndex; } }

        public int MapIndex { get { return mapIndex; } }


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
            GroupProgressData groupData = gameProgressData.Groups[groupIndex];
            PlayerPrefs.SetString(GROUP_NAME, groupData.Name);

            DrawingProgressData drawingData = groupData.Drawings[drawingIndex];
            PlayerPrefs.SetString(DRAWING_NAME, drawingData.Name);
            
            PlayerPrefs.SetInt(MAP_INDEX, mapIndex);

            gameProgressDataInitializer.Save(gameProgressData);
        }

        private void LoadData()
        {
            gameProgressData = gameProgressDataInitializer.Load();
            Adjust(gameProgressData, groupsDataInitializer.Data);

            groupIndex = GetGroupIndex(PlayerPrefs.GetString(GROUP_NAME));
            drawingIndex = GetDrawingIndex(PlayerPrefs.GetString(DRAWING_NAME), groupIndex);

            mapIndex = PlayerPrefs.GetInt(MAP_INDEX);
        }

        private int GetGroupIndex(string name)
        {
            for(int i = 0; i < gameProgressData.Groups.Count; i++)
            {
                GroupProgressData groupData = gameProgressData.Groups[i];
                if (groupData.Name == name) return i;
            }
            return 0;
        }

        private int GetDrawingIndex(string name, int groupIndex)
        {
            GroupProgressData groupData = gameProgressData.Groups[groupIndex];

            for (int i = 0; i < groupData.Drawings.Count; i++)
            {
                DrawingProgressData drawingData = groupData.Drawings[i];
                if (drawingData.Name == name) return i;
            }
            return 0;
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
            this.groupIndex = group;
            this.drawingIndex = drawing;
            mapIndex = 0;
        }

        public void NextMap()
        {
            mapIndex++;
        }

        public bool IsDrawingCompleted()
        {
            return mapIndex == groupsDataInitializer.Data.Groups[groupIndex].Drawings[drawingIndex].Maps.Length;
        }

        public void FinishDrawing()
        {
            GroupProgressData groupProgressData = gameProgressData.Groups[groupIndex];
            DrawingProgressData drawingProgressData = groupProgressData.Drawings[drawingIndex];

            drawingProgressData.Completed = true;
            drawingProgressData.Stars = livesController.Lives;

            SaveData();
        }
    }
}