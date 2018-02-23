using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class DrawingButtonsController : MonoBehaviour
    {
        public UnityAction<int> OnClicked;


        [Header("Settings")]
        [SerializeField]
        private GameObject template;


        [Header("Dependencies")]
        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private ThemeController themeController;


        private List<ButtonController> buttons;


        void Awake()
        {
            buttons = new List<ButtonController>();
        }

        void Start()
        {
        }


        void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            foreach (var button in buttons)
                button.OnClicked += ProcessButtonClicked;
        }

        void OnDisable()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            foreach (var button in buttons)
                if (button != null)
                    button.OnClicked -= ProcessButtonClicked;
        }


        public void Init(int group)
        {
            Clear();

            GroupData groupData = groupsDataInitializer.Data.Groups[group];

            for (int i = 0; i < groupData.Drawings.Length; i++)
            {
                DrawingData drawingData = groupData.Drawings[i];

                GameObject instance = Instantiate(template);
                RectTransform rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.SetParent(GetComponent<RectTransform>());
                rectTransform.anchoredPosition = Vector2.right * 100f * i;
                
                Sprite sprite = Resources.Load<Sprite>("Drawings/" + drawingData.Image);

                ButtonController buttonController = instance.GetComponent<ButtonController>();
                buttonController.Init(i, themeController.GetCurrentTheme(), sprite);

                buttons.Add(buttonController);
            }

            Subscribe();
        }

        private void Clear()
        {
            Unsubscribe();

            foreach (var button in buttons)
                if (button != null)
                    Destroy(button.gameObject);

            buttons.Clear();
        }

        private void ProcessButtonClicked(int id)
        {
            OnClicked(id);
        }
    }

}