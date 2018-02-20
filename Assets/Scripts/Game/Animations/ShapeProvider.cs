using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Data.Initializers;
using Dev.Krk.MemoryDraw.Game.State;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game.Animations
{
    public class ShapeProvider : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private int lowerLimit;

        [Header("Dependencies")]
        [SerializeField]
        private ThemeController themeController;

        [SerializeField]
        private ShapesDataInitializer shapesDataInitializer;


        private int[] shapes;


        void Start()
        {

        }


        void OnEnable()
        {
            themeController.OnInitialized += ProcessThemeInitialized;
            themeController.OnThemeUpdated += ProcessThemeUpdated;
        }

        void OnDisable()
        {
            if (themeController != null)
            {
                themeController.OnInitialized -= ProcessThemeInitialized;
                themeController.OnThemeUpdated -= ProcessThemeUpdated;
            }
        }

        private void ProcessThemeInitialized()
        {
            UpdateShapes();
        }

        private void ProcessThemeUpdated()
        {
            UpdateShapes();
        }

        private void UpdateShapes()
        {
            ThemeData data = themeController.GetCurrentTheme();
            shapes = data.Shapes;
        }


        public ShapeData GetShapeData(int horizontalLength, int verticalLength)
        {
            List<int> allowed = GetAllowedShapeIndices(horizontalLength, verticalLength);
            int shape = allowed[Random.Range(0, allowed.Count)];

            return shapesDataInitializer.Data.Shapes[shape];
        }

        private List<int> GetAllowedShapeIndices(int horizontalLength, int verticalLength)
        {
            List<int> result = new List<int>();

            foreach (int shape in shapes)
            {
                ShapeData shapeData = shapesDataInitializer.Data.Shapes[shape];
                if (horizontalLength - lowerLimit <= shapeData.HorizontalFields.Length && shapeData.HorizontalFields.Length <= horizontalLength &&
                    verticalLength - lowerLimit <= shapeData.VerticalFields.Length && shapeData.VerticalFields.Length <= verticalLength)
                    result.Add(shape);
            }

            return result;
        }
    }
}