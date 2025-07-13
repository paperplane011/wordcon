#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSO))]
public class LevelSOEditor : Editor
{
    private const float CellSize = 34f;
    private const float Padding = 12f;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        //DrawDefaultInspector();

        SerializedProperty levelNumProp = serializedObject.FindProperty("LevelNum");
        SerializedProperty levelLettersProp = serializedObject.FindProperty("LevelLetters");
        SerializedProperty wordsPositionsProp = serializedObject.FindProperty("WordsPositions");

        EditorGUILayout.PropertyField(levelNumProp);
        EditorGUILayout.PropertyField(levelLettersProp);

        
        EditorGUILayout.Space(10);

        // Получаем доступ к данным сетки
        SerializedProperty gridProp = serializedObject.FindProperty("LayoutString");
        SerializedProperty dataProp = gridProp.FindPropertyRelative("_gridData");
        
        // Текстовое поле для редактирования всей строки
        string fullGridString = EditorGUILayout.TextField("Grid Data", dataProp.stringValue);
        if (fullGridString.Length != CharGrid.Size * CharGrid.Size)
        {
            fullGridString = fullGridString.PadRight(CharGrid.Size * CharGrid.Size, ' ').Substring(0, CharGrid.Size * CharGrid.Size);
        }
        dataProp.stringValue = fullGridString;
        
        // Визуальная сетка
        EditorGUILayout.Space(10);
        DrawCharGrid(dataProp);

        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(wordsPositionsProp);
        
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawCharGrid(SerializedProperty dataProp)
    {
        string gridData = dataProp.stringValue;
        char[] gridChars = gridData.ToCharArray();
        bool modified = false;
        int id = 0;

        for (int y = 0; y < CharGrid.Size; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < CharGrid.Size; x++)
            {

                int index = y * CharGrid.Size + x;
                string currentChar = gridChars[index].ToString();

                // Создаем стиль для ячейки
                GUIStyle cellStyle = new GUIStyle(EditorStyles.textField)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14,
                    fixedWidth = CellSize,
                    fixedHeight = CellSize,
                    margin = new RectOffset(0, 0, (int)Padding, (int)Padding)
                };

                // Рисуем TextField для каждого символа
                string newChar = EditorGUILayout.TextField(currentChar, cellStyle, GUILayout.Width(CellSize));

                // Обрабатываем ввод
                if (newChar.Length > 0 && newChar != currentChar)
                {
                    gridChars[index] = newChar[0];
                    modified = true;
                }
                else if (newChar == "")
                {
                    gridChars[index] = ' '; // Пустая ячейка
                    modified = true;
                }





            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            
            for (int k = 0; k < CharGrid.Size; k++)
            {
                GUIStyle idStyle = new GUIStyle(EditorStyles.textField)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 8,
                    fontStyle = FontStyle.Italic,
                    normal = { textColor = Color.gray },
                    fixedHeight = CellSize / 4,
                    margin = new RectOffset(0, 0, (int)Padding, (int)Padding),
                    
                    

                };

                EditorGUILayout.LabelField($"{id}", idStyle, GUILayout.Width(CellSize-2.6f), GUILayout.Height(1));
                id++;
            }
            
                

            EditorGUILayout.EndHorizontal();
        }



        
        if (modified)
        {
            dataProp.stringValue = new string(gridChars);
        }
    }
}
#endif