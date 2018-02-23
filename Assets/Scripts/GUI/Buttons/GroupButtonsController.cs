using Dev.Krk.MemoryDraw.Common;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.Krk.MemoryDraw.GUI.Buttons
{
    public class GroupButtonsController : MonoBehaviour
    {
        public UnityAction<int> OnClicked;


        [Header("Settings")]
        [SerializeField]
        private float spacing;

        [SerializeField]
        private GameObject template;


        [Header("Dependencies")]
        [SerializeField]
        private GroupsDataInitializer groupsDataInitializer;

        [SerializeField]
        private ThemeController themeController;


        private List<ButtonController> buttons;


        private int current;


        void Awake()
        {
            buttons = new List<ButtonController>();
        }

        void Start()
        {
        }


        void OnEnable()
        {
            groupsDataInitializer.OnInitialized += ProcessGroupsDataInitialized;

            Subscribe();
        }

        private void Subscribe()
        {
            foreach (var button in buttons)
                button.OnClicked += ProcessButtonClicked;
        }

        void OnDisable()
        {
            if (groupsDataInitializer != null)
            {
                groupsDataInitializer.OnInitialized -= ProcessGroupsDataInitialized;
            }

            Unsubscribe();
        }

        private void Unsubscribe()
        {
            foreach (var button in buttons)
                if (button != null)
                    button.OnClicked -= ProcessButtonClicked;
        }

        private void ProcessGroupsDataInitialized()
        {
            Init();
        }

        private void Init()
        {
            current = 0; //TODO maybe taken from PlayerPrefs? :)

            for (int i = 0; i < groupsDataInitializer.Data.Groups.Length; i++)
            {
                GroupData groupData = groupsDataInitializer.Data.Groups[i];

                GameObject instance = Instantiate(template);

                Sprite sprite = Resources.Load<Sprite>("Drawings/" + groupData.Image);

                ButtonController buttonController = instance.GetComponent<ButtonController>();
                buttonController.Init(i, themeController.GetCurrentTheme(), sprite);
                buttons.Add(buttonController);

                InitPosition(buttonController);
            }

            Subscribe();
        }

        private void InitPosition(ButtonController buttonController)
        {
            int diff = buttonController.Id - current;
            float scale = Mathf.Pow(2, Mathf.Abs(diff));

            RectTransform rectTransform = buttonController.GetComponent<RectTransform>();
            rectTransform.SetParent(GetComponent<RectTransform>());
            rectTransform.anchoredPosition = Vector2.right * spacing * diff * scale;

            rectTransform.localScale = Vector3.one / scale;
        }

        private void ProcessButtonClicked(int id)
        {
            if (id < current)
            {
                current--;
                UpdatePositions();
            }
            else if (id > current)
            {
                current++;
                UpdatePositions();
            }
            else
            {
                OnClicked(id);
            }
        }

        private void UpdatePositions()
        {
            foreach (var button in buttons) UpdatePosition(button);
        }

        private void UpdatePosition(ButtonController buttonController)
        {
            int diff = buttonController.Id - current;
            float scale = Mathf.Pow(2, Mathf.Abs(diff));

            RectMover rectMover = buttonController.GetComponent<RectMover>();
            rectMover.Target = Vector2.right * spacing * diff * scale;

            RectScaler rectScaler = buttonController.GetComponent<RectScaler>();
            rectScaler.TargetScale = Vector3.one / scale;
        }
    }

}