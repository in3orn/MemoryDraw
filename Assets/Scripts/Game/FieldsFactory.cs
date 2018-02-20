using UnityEngine;
using Dev.Krk.MemoryDraw.Game;
using Dev.Krk.MemoryDraw.Game.State;
using Dev.Krk.MemoryDraw.Game.Theme;

public class FieldsFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject pattern;

    [SerializeField]
    private LevelController level;

    [SerializeField]
    private ThemeController themeController;

    public Field[,] Create(int[,] fieldTypes, Field.TypeEnum fieldType, Vector2 offset)
    {
        Field[,] fields = new Field[fieldTypes.GetLength(0), fieldTypes.GetLength(1)];

        for (int y = 0; y < fieldTypes.GetLength(0); y++)
        {
            for (int x = 0; x < fieldTypes.GetLength(1); x++)
            {
                GameObject instance = Instantiate(pattern, level.gameObject.transform) as GameObject;

                InitThemeSprites(instance);

                Field field = instance.GetComponent<Field>();
                InitField(field, offset.x + x ,offset.y + y, fieldType, fieldTypes[y, x] > 0);
                fields[y, x] = field;
            }
        }

        return fields;
    }

    private void InitField(Field field, float x, float y, Field.TypeEnum fieldType, bool valid)
    {
        Vector2 position = new Vector2(x * Field.SIZE, y * Field.SIZE);
        if (fieldType == Field.TypeEnum.Horizontal)
        {
            position += Vector2.right * Field.SIZE / 2;
        }
        else
        {
            position += Vector2.up * Field.SIZE / 2;
            field.Type = fieldType;
        }
        field.Init(position, valid);
    }

    private void InitThemeSprites(GameObject instance)
    {
        ThemeSprite[] themeSprites = instance.GetComponentsInChildren<ThemeSprite>();
        foreach(var sprite in themeSprites)
        {
            sprite.Controller = themeController;
        }
    }
}
