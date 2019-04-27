using UnityEngine;
using System.Collections;
using UnityEditor;
 
public class SelectByTag : MonoBehaviour {
    public static string SelectedTag = "Zone2";
 
    [MenuItem("Helpers/Select By Tag")]
    public static void SelectObjectsWithTag()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
        Selection.objects = objects;
    }
}