using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InventoryShape))]
public class InventoryShapePropertyDrawer : PropertyDrawer
{
    const int GridSize = 16;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pWidth = property.FindPropertyRelative("width");
        var pHeight = property.FindPropertyRelative("height");
        var pShape = property.FindPropertyRelative("shape");

        if (pWidth.intValue <= 0) { pWidth.intValue = 1; }
        if (pHeight.intValue <= 0) { pHeight.intValue = 1; }

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var halfWidth = position.width / 2;
        var widthRect = new Rect(position.x, position.y, halfWidth, GridSize);
        var heightRect = new Rect(position.x + halfWidth, position.y, halfWidth, GridSize);

        EditorGUIUtility.labelWidth = 40;
        EditorGUI.PropertyField(widthRect, pWidth, new GUIContent("Width"));
        EditorGUI.PropertyField(heightRect, pHeight, new GUIContent("Height"));

        var width = pWidth.intValue;
        var height = pHeight.intValue;
        pShape.arraySize = width * height;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var index = x + width * y;
                var rect = new Rect(position.x + (x * GridSize), position.y + GridSize + (y * GridSize), GridSize, GridSize);
                EditorGUI.PropertyField(rect, pShape.GetArrayElementAtIndex(index), GUIContent.none);
            }
        }

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label);
        height += property.FindPropertyRelative("height").intValue * GridSize;
        return height;
    }
}
