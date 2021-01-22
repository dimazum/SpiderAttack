using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneGenerator))]
public class ArrayGUI : Editor
{
    SceneGenerator sceneGenerator;

    public ArrayGUI()
    {
        sceneGenerator = new SceneGenerator();
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("RegenerateMap"))
        {
            if (sceneGenerator == null)
            {
                sceneGenerator = new SceneGenerator();
            }
            sceneGenerator.CopyTilemapToMapList();
        }

        if (GUILayout.Button("DestroyBorders"))
        {
            if (sceneGenerator == null)
            {
                sceneGenerator = new SceneGenerator();
            }
            sceneGenerator.RemoveBorders();
        }
    }

}


//[CustomEditor(typeof(SaveManager))]
//public class SaveManagerGUI : Editor
//{
//    SaveManager sceneManager;

//    public SaveManagerGUI()
//    {
//        sceneManager = new SaveManager();
//    }
//    public override void OnInspectorGUI()
//    {


//        if (GUILayout.Button("GenerateBorders"))
//        {
//            if (sceneManager == null)
//            {
//                sceneManager = new SaveManager();
//            }
//            sceneManager.GenerateBorders();
//        }
//    }

//}

