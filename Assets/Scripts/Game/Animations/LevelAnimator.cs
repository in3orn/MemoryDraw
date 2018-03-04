using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Game.Drawing;
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

        [SerializeField]
        private DrawingController drawing;


        [Header("Fail Level Settings")]
        [SerializeField]
        private float breakInterval;


        [Header("Complete Level Settings")]
        [SerializeField]
        private float completeLevelInterval;


        private List<Field> updated;


        void Start()
        {
            updated = new List<Field>();
        }
        

        public void CompleteFlow(List<Field> horizontalFields, List<Field> verticalFields)
        {
            StartCoroutine(AnimateShape(horizontalFields, verticalFields));
        }

        private IEnumerator AnimateShape(List<Field> horizontalFields, List<Field> verticalFields)
        {
            drawing.Show();

            yield return new WaitForSeconds(showDuration);
            
            yield return HideShape(horizontalFields, verticalFields);

            yield return new WaitForSeconds(showDuration);

            drawing.Hide();
        }
        
        private IEnumerator HideShape(List<Field> horizontalFields, List<Field> verticalFields)
        {
            updated.Clear();

            int min = (int)(Mathf.Min(CalculateLevelMin(horizontalFields), CalculateLevelMin(verticalFields)) / Field.SIZE);
            int max = (int)(Mathf.Max(CalculateLevelMax(horizontalFields), CalculateLevelMax(verticalFields)) / Field.SIZE);
            int size = (max - min) * 2;

            for (int s = size; s >= 0; s--)
            {
                for (int ds = 0; ds < s; ds++)
                {
                    int y = min + ds;
                    int x = min + s - ds - 1;
                    foreach (var field in horizontalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.Hide();
                        }

                    foreach (var field in verticalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.Hide();
                        }
                }
                yield return new WaitForSeconds(breakInterval / size);
            }
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


        public void FailLevel(List<Field> horizontalFields, List<Field> verticalFields)
        {
            StartCoroutine(BreakFields(horizontalFields, verticalFields));
        }

        private IEnumerator BreakFields(List<Field> horizontalFields, List<Field> verticalFields)
        {
            updated.Clear();
            
            int min = (int)(Mathf.Min(CalculateLevelMin(horizontalFields), CalculateLevelMin(verticalFields)) / Field.SIZE);
            int max = (int)(Mathf.Max(CalculateLevelMax(horizontalFields), CalculateLevelMax(verticalFields)) / Field.SIZE);
            int size = (max - min) * 2;

            for (int s = size; s >= 0; s--)
            {
                for (int ds = 0; ds < s; ds++)
                {
                    int y = min + ds;
                    int x = min + s - ds - 1;
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

        public void ChangeToOld(List<Field> horizontalFields, List<Field> verticalFields)
        {
            StartCoroutine(MakeOld(horizontalFields, verticalFields));
        }

        private IEnumerator MakeOld(List<Field> horizontalFields, List<Field> verticalFields)
        {
            updated.Clear();

            int min = (int)(Mathf.Min(CalculateLevelMin(horizontalFields), CalculateLevelMin(verticalFields)) / Field.SIZE);
            int max = (int)(Mathf.Max(CalculateLevelMax(horizontalFields), CalculateLevelMax(verticalFields)) / Field.SIZE);
            int size = (max - min) * 2;

            for (int s = size; s >= 0; s--)
            {
                for (int ds = 0; ds < s; ds++)
                {
                    int y = min + ds;
                    int x = min + s - ds - 1;
                    foreach (var field in horizontalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.MakeOld();
                        }

                    foreach (var field in verticalFields)
                        if (CanUpdate(field, x, y))
                        {
                            updated.Add(field);
                            field.MakeOld();
                        }
                }
                yield return new WaitForSeconds(completeLevelInterval / size);
            }
        }

        private float CalculateLevelMin(List<Field> fields)
        {
            float min = Mathf.Infinity;

            foreach (var field in fields)
            {
                if (min > field.transform.position.x)
                    min = field.transform.position.x;
                if (min > field.transform.position.y)
                    min = field.transform.position.y;
            }

            return min;
        }

        private float CalculateLevelMax(List<Field> fields)
        {
            float max = Mathf.NegativeInfinity;

            foreach (var field in fields)
            {
                if (max < field.transform.position.x)
                    max = field.transform.position.x;
                if (max < field.transform.position.y)
                    max = field.transform.position.y;
            }

            return max;
        }
    }
}