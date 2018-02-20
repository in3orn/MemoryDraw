using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Game;

public class FieldMap : MonoBehaviour
{
    //TODO think if can be used?
    public enum StateEnum
    {
        Shown = 0,
        Masked,
        Hidden
    }

    public UnityAction OnShown;
    public UnityAction OnMasked;
    public UnityAction OnHidden;

    [SerializeField]
    private float showInterval = 0.1f;

    [SerializeField]
    private float hideInterval = 0.1f;

    private Field[,] horizontalFields;

    private Field[,] verticalFields;

    [SerializeField]
    FieldsFactory fieldsFactory;

    public Field[,] HorizontalFields { get { return horizontalFields; } }

    public Field[,] VerticalFields { get { return verticalFields; } }

    public void Init(FieldMapData data, Vector2 offset)
    {
        horizontalFields = fieldsFactory.Create(data.HorizontalFields, Field.TypeEnum.Horizontal, offset);
        verticalFields = fieldsFactory.Create(data.VerticalFields, Field.TypeEnum.Vertical, offset);
    }

    public void Clear()
    {
        horizontalFields = ClearFields(horizontalFields);
        verticalFields = ClearFields(verticalFields);
    }

    private Field[,] ClearFields(Field[,] fields)
    {
        if (fields != null)
        {
            for (int y = 0; y < fields.GetLength(0); y++)
            {
                for (int x = 0; x < fields.GetLength(1); x++)
                {
                    fields[y, x] = null;
                }
            }
            fields = null;
        }
        return null;
    }

    public int HorizontalLength
    {
        get { return horizontalFields.GetLength(1); }
    }

    public int VerticalLength
    {
        get { return verticalFields.GetLength(0); }
    }

    public float ShowInterval { get { return showInterval; } }

    public float HideInterval { get { return hideInterval; } }

    public Field GetVerticalField(int x, int y)
    {
        return verticalFields[y, x];
    }

    public Field GetHorizontalField(int x, int y)
    {
        return horizontalFields[y, x];
    }

    public bool CanMoveLeft(int x, int y)
    {
        if (x >= 0 && y < VerticalLength + 1)
        {
            Field field = GetHorizontalField(x, y);
            return !field.Hidden && !field.Broken;
        }

        return false;
    }

    public bool CanMoveRight(int x, int y)
    {
        if (x < HorizontalLength && y < VerticalLength + 1)
        {
            Field field = GetHorizontalField(x, y);
            return !field.Hidden && !field.Broken;
        }

        return false;
    }

    public bool CanMoveUp(int x, int y)
    {
        if (x < HorizontalLength + 1 && y < VerticalLength)
        {
            Field field = GetVerticalField(x, y);
            return !field.Hidden && !field.Broken;
        }

        return false;
    }

    public bool CanMoveDown(int x, int y)
    {
        if (x < HorizontalLength + 1 && y >= 0)
        {
            Field field = GetVerticalField(x, y);
            return !field.Hidden && !field.Broken;
        }

        return false;
    }

    public void ShowPreview()
    {
        StartCoroutine(showFields(false));
    }

    public void ShowPlayMode()
    {
        StartCoroutine(showFields(true));
        StartCoroutine(maskFields());
    }

    private IEnumerator showFields(bool all) //TODO show from player
    {
        int size = HorizontalLength + VerticalLength + 1;
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                tryShowHorizontalField(x, y, all);
                tryShowVerticalField(x, y, all);
            }
            yield return new WaitForSeconds(showInterval);
        }
        if (!all && OnShown != null) OnShown();
    }

    private void tryShowHorizontalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < horizontalFields.GetLength(0) && y < horizontalFields.GetLength(1))
        {
            Field field = horizontalFields[x, y];
            if (all || field.Valid) field.Show();
        }
    }

    private void tryShowVerticalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < verticalFields.GetLength(0) && y < verticalFields.GetLength(1))
        {
            Field field = verticalFields[x, y];
            if (all || field.Valid) field.Show();
        }
    }

    public void Hide()
    {
        StartCoroutine(hideFields(true));
    }

    private IEnumerator hideFields(bool all)
    {
        int size = HorizontalLength + VerticalLength + 1;
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                tryHideHorizontalField(x, y, all);
                tryHideVerticalField(x, y, all);
            }
            yield return new WaitForSeconds(hideInterval);
        }
        if (OnHidden != null) OnHidden();
    }

    private void tryHideHorizontalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < horizontalFields.GetLength(0) && y < horizontalFields.GetLength(1))
        {
            Field field = horizontalFields[x, y];
            if (all || field.Valid) field.Hide();
        }
    }

    private void tryHideVerticalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < verticalFields.GetLength(0) && y < verticalFields.GetLength(1))
        {
            Field field = verticalFields[x, y];
            if (all || field.Valid) field.Hide();
        }
    }

    private IEnumerator maskFields() //TODO show from player
    {
        int size = HorizontalLength + VerticalLength + 1;
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                tryMaskHorizontalField(x, y, true);
                tryMaskVerticalField(x, y, true);
            }
            yield return new WaitForSeconds(showInterval);
        }
    }

    private void tryMaskHorizontalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < horizontalFields.GetLength(0) && y < horizontalFields.GetLength(1))
        {
            Field field = horizontalFields[x, y];
            if (all || field.Valid) field.Mask();
        }
    }

    private void tryMaskVerticalField(int x, int y, bool all)
    {
        if (x >= 0 && y >= 0 && x < verticalFields.GetLength(0) && y < verticalFields.GetLength(1))
        {
            Field field = verticalFields[x, y];
            if (all || field.Valid) field.Mask();
        }
    }

    public void HideNotValid()
    {
        StartCoroutine(HideNotValidFields());
    }

    private IEnumerator HideNotValidFields()
    {
        int size = HorizontalLength + VerticalLength + 1;
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                HideInvalidFields(horizontalFields, x, y);
                HideInvalidFields(verticalFields, x, y);
            }
            yield return new WaitForSeconds(hideInterval);
        }
        if (OnHidden != null) OnHidden();
    }

    private void HideInvalidFields(Field[,] fields, int x, int y)
    {
        if (x >= 0 && y >= 0 && x < fields.GetLength(0) && y < fields.GetLength(1))
        {
            Field field = fields[x, y];
            if (!field.Valid || field.Masked) field.Hide();
        }
    }
}