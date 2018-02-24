using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Progress;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class GroupButtonFactory : ButtonFactory
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
            int size = groupsDataInitializer.Data.Groups.Length;
            ButtonController[] result = new ButtonController[size];

            for (int i = 0; i < size; i++)
            {
                GameObject instance = CreateButton();

                GroupData groupData = groupsDataInitializer.Data.Groups[i];
                GroupProgressData progressData = progressController.GetGroupData(i);

                GroupButtonController buttonController = instance.GetComponent<GroupButtonController>();
                buttonController.Init(i, themeController.GetCurrentTheme(), groupData, progressData);
                result[i] = buttonController;
            }

            return result;
        }
    }
}