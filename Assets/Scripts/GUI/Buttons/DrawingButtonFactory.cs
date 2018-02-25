using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Progress;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class DrawingButtonFactory : ButtonFactory
    {
        [Header("Dependencies")]
        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private ProgressController progressController;

        [SerializeField]
        private ThemeController themeController;
        
        
        public override ButtonController[] CreateButtons(int parentId)
        {
            GroupData groupData = groupsDataInitializer.Data.Groups[parentId];
            GroupProgressData groupProgressData = progressController.GetGroupData(parentId);

            int size = groupData.Drawings.Length;
            ButtonController[] result = new ButtonController[size];

            for (int i = 0; i < size; i++)
            {
                GameObject instance = CreateButton();

                DrawingData drawingData = groupData.Drawings[i];
                DrawingProgressData drawingProgressData = groupProgressData.Drawings[i];

                DrawingButtonController buttonController = instance.GetComponent<DrawingButtonController>();
                buttonController.Init(i, themeController.GetCurrentTheme(), drawingData, drawingProgressData);
                result[i] = buttonController;
            }

            return result;
        }

        public override void UpdateButton(ButtonController button)
        {
            GroupData groupData = groupsDataInitializer.Data.Groups[progressController.GroupIndex];
            DrawingData drawingData = groupData.Drawings[button.Id];

            GroupProgressData groupProgressData = progressController.GetGroupData(progressController.GroupIndex);
            DrawingProgressData drawingProgressData = groupProgressData.Drawings[button.Id];

            DrawingButtonController buttonController = button.GetComponent<DrawingButtonController>();
            buttonController.Init(button.Id, themeController.GetCurrentTheme(), drawingData, drawingProgressData);
        }
    }
}