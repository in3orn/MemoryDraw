using Dev.Krk.MemoryDraw.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Krk.MemoryDraw.Game.Animations
{
    public class LevelAnimator : MonoBehaviour
    {
        [Header("Complete Flow Settings")]
        [SerializeField]
        private float moveDuration;

        [SerializeField]
        private float showDuration;

        [SerializeField]
        private float showInterval;

        [SerializeField]
        private float hideInterval;


        [Header("Fail Level Settings")]
        [SerializeField]
        private float breakInterval;


        private List<Field> updated;


        void Start()
        {
            updated = new List<Field>();
        }
        

        public void CompleteFlow(List<Field> horizontalFields, List<Field> verticalFields, int size)
        {
            StartCoroutine(AnimateShape(horizontalFields, verticalFields, size));
        }

        private IEnumerator AnimateShape(List<Field> horizontalFields, List<Field> verticalFields, int size)
        {   
            yield return new WaitForSeconds(showDuration);
            
            yield return HideShape(horizontalFields, verticalFields, size);

            //TODO end game animation -> show drawn image
        }
        
        private IEnumerator HideShape(List<Field> horizontalFields, List<Field> verticalFields, int size)
        {
            updated.Clear();

            foreach (var field in horizontalFields)
                field.Hide();

            yield return new WaitForSeconds(hideInterval);

            foreach (var field in verticalFields)
                field.Hide();
        }

        private bool CanUpdate(Field field, int x, int y)
        {
            Vector2 pos = field.transform.position / Field.SIZE;
            return !field.Broken && !updated.Contains(field) && (int)pos.x == x && (int)pos.y == y;
        }

        private void Move(Field field, PointData target)
        {
            StartCoroutine(MoveFieldTo(field, new Vector2(target.X * Field.SIZE, target.Y * Field.SIZE)));
        }

        private IEnumerator MoveFieldTo(Field field, Vector3 endPosition)
        {
            float time = 0;
            Vector3 startPosition = field.transform.position;
            while (time < moveDuration)
            {
                field.transform.position = Vector3.Lerp(startPosition, endPosition, time / moveDuration);

                time += Time.deltaTime;
                yield return null;
            }

            field.transform.position = endPosition;
        }


        public void FailLevel(List<Field> horizontalFields, List<Field> verticalFields, int size)
        {
            StartCoroutine(BreakFields(horizontalFields, verticalFields, size));
        }

        private IEnumerator BreakFields(List<Field> horizontalFields, List<Field> verticalFields, int size)
        {
            updated.Clear();

            for (int s = size - 1; s > 0; s--)
            {
                for (int ds = 0; ds < s; ds++)
                {
                    int y = ds;
                    int x = s - ds - 1;
                    foreach (var field in horizontalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.Break();
                        }

                    foreach (var field in verticalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.Break();
                        }
                }
                yield return new WaitForSeconds(breakInterval / size);
            }
        }
    }
}