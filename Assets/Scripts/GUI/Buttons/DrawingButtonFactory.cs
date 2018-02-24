using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class DrawingButtonFactory : ButtonFactory
    {
        [Header("Dependencies")]
        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private ThemeController themeController;
        
        
        public override ButtonController[] CreateButtons(int parentId)
        {
            GroupData groupData = groupsDataInitializer.Data.Groups[parentId];

            int size = groupData.Drawings.Length;
            ButtonController[] result = new ButtonController[size];

            for (int i = 0; i < size; i++)
            {
                GameObject instance = CreateButton();

                DrawingData drawingData = groupData.Drawings[i];

                DrawingButtonController buttonController = instance.GetComponent<DrawingButtonController>();
                buttonController.Init(i, themeController.GetCurrentTheme(), drawingData);
                result[i] = buttonController;
            }

            return result;
        }
    }
}