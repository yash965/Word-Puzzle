using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Controller))]
public class CustomInspector : Editor
{
    SerializedProperty dataModel;
    SerializedProperty animator;
    SerializedProperty correct;
    SerializedProperty view;
    SerializedProperty levels;

    SerializedProperty question;
    SerializedProperty words;
    SerializedProperty correctQ;
    SerializedProperty QuesToClearLevel;
    SerializedProperty weatherType;
    SerializedProperty completeWeatherType;

    SerializedProperty levelArr;
    SerializedProperty levelIndex;

    private SerializedObject dataModelObject;
    private SerializedObject levelObject;
    private string levelName;

    private void OnEnable()
    {
        // Controller properties
        dataModel = serializedObject.FindProperty("dataModel");
        animator = serializedObject.FindProperty("animator");
        correct = serializedObject.FindProperty("correct");
        view = serializedObject.FindProperty("view");
        //levelChoice = serializedObject.FindProperty("levelChoice");
        levels = serializedObject.FindProperty("levels");

        UpdateDataModelObject();
        UpdateLevelObject();
    }

    private void UpdateDataModelObject()
    {
        if (dataModel.objectReferenceValue != null)
        {
            dataModelObject = new SerializedObject(dataModel.objectReferenceValue);
            question = dataModelObject.FindProperty("question");
            words = dataModelObject.FindProperty("words");
            correctQ = dataModelObject.FindProperty("correctQ");
            QuesToClearLevel = dataModelObject.FindProperty("QuesToClearLevel");
            weatherType = dataModelObject.FindProperty("selectedWeatherType");
            completeWeatherType = dataModelObject.FindProperty("completeWeatherType");
        }
        else
        {
            dataModelObject = null;
        }
    }

    private void UpdateLevelObject()
    {
        if(levels.objectReferenceValue != null)
        {
            levelObject = new SerializedObject(levels.objectReferenceValue);
            levelArr = levelObject.FindProperty("levels");
            levelIndex = levelObject.FindProperty("levelIndex");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();          // Alligns with the current object, which is "Controller" here.

        // Draw Controller properties
        EditorGUILayout.PropertyField(dataModel, new GUIContent("Data Model"));
        EditorGUILayout.PropertyField(animator, new GUIContent("Animator"));
        EditorGUILayout.PropertyField(view, new GUIContent("View"));
        //EditorGUILayout.PropertyField(levelChoice, new GUIContent("Level Choice"));
        EditorGUILayout.PropertyField(levels, new GUIContent("Level Choice"));

        if (levelArr.arraySize > 0)
        {
            string[] levelNames = new string[levelArr.arraySize];
            for (int i = 0; i < levelArr.arraySize; i++)
            {
                levelNames[i] = levelArr.GetArrayElementAtIndex(i).stringValue;
            }

            levelIndex.intValue = EditorGUILayout.Popup("Selected Level", levelIndex.intValue, levelNames);

            levelObject.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.LabelField("No levels available. Add some levels to use the dropdown.");
        }

        if (GUILayout.Button("Update selected level"))
        {
            LoadLevel();
        }

        if (dataModelObject != null)
        {
            dataModelObject.Update();       // Allign values with scriptable object.

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Data Model Properties", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(question, new GUIContent("Question"));
            EditorGUILayout.PropertyField(words, new GUIContent("Words"), true);
            EditorGUILayout.PropertyField(correctQ, new GUIContent("Correct Answers"), true);
            EditorGUILayout.PropertyField(QuesToClearLevel, new GUIContent("Questions to Clear Level"));
            EditorGUILayout.PropertyField(weatherType, new GUIContent("Selected Weather Type"));
            EditorGUILayout.PropertyField(completeWeatherType, new GUIContent("Completion Weather Type"));

            dataModelObject.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a DataModel ScriptableObject to view its properties.", MessageType.Info);
        }

        if (GUILayout.Button("Update"))
        {
            UpdateValues();
        }

        levelName = EditorGUILayout.TextField("Level Name", levelName);

        if (GUILayout.Button("New Level"))
        {
            NewLevel();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateValues()
    {
        Controller controller = (Controller)target;

        if(controller.dataModel != null && controller.view != null)
        {
            controller.view.SetDefaultValues(controller.dataModel);
            controller.UpdateWeatherEffect();
        }
        else
        {
            Debug.Log("Values not changed in scene");
        }
    }

    private void LoadLevel()
    {
        DataModel loadedDataModel = Resources.Load<DataModel>(levelArr.GetArrayElementAtIndex(levelIndex.intValue).stringValue); 

        if (loadedDataModel != null)
        {
            dataModel.objectReferenceValue = loadedDataModel;
            serializedObject.ApplyModifiedProperties();
            UpdateDataModelObject();
            UpdateValues();

            Debug.Log("DataModel updated successfully!");
        }
        else
        {
            Debug.LogError($"Failed to load DataModel with name from Resources.");
        }
    }

    private void NewLevel()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogError("Level name is empty. Cannot create asset.");
            return;
        }

        levelArr.arraySize++;
        SerializedProperty newLevel = levelArr.GetArrayElementAtIndex(levelArr.arraySize-1);
        newLevel.stringValue = levelName;

        DataModel newData = ScriptableObject.CreateInstance<DataModel>();

        string path = $"Assets/Word Puzzle/Resources/{levelName}.asset";

        path = AssetDatabase.GenerateUniqueAssetPath(path);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Failed to generate a unique asset path.");
            return;
        }
        AssetDatabase.CreateAsset(newData, path);

        newData.correctQ = new bool[7];
        newData.words = new string[7];

        // Step 3: Save and refresh the AssetDatabase
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
