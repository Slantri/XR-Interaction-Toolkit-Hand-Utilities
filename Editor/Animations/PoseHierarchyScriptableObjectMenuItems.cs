using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using XR.Interaction.Toolkit.Hand.Utilities.Animations;
using System.IO;

public static class PoseHierarchyScriptableObjectMenuItems
{
    private static string lastSaveFileLocation = "Assets";
    [MenuItem("XRITHandUtilities/PoseHierarchyScriptableObject/GenerateFromSelectedGameObject")]
    public static void GenerateFromSelectedGameObject()
    {
        foreach (var gameObject in Selection.gameObjects)
        {
            var poseHierarchySO = ScriptableObject.CreateInstance<PoseHierarchyScriptableObject>();
            poseHierarchySO.poseHierarchy = new PoseHierarchy(gameObject.transform);
            var filepath = EditorUtility.SaveFilePanel("Save Pose Hierarchy Scriptable Object", GetSaveFileLocation(), "poseHierarchy", "asset");
            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }
            filepath = filepath.Replace(Application.dataPath, "Assets");

            AssetDatabase.CreateAsset(poseHierarchySO, filepath);
            lastSaveFileLocation = Path.GetDirectoryName(filepath);
        }
    }


    private static string GetSaveFileLocation()
    {
        if (lastSaveFileLocation != null)
        {
            return lastSaveFileLocation;
        }
        return Application.dataPath;
    }
}
