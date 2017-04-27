using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LaoHan.Tools.SearchAllReference
{
    public class lhSearchAllReference
    {
        [MenuItem("Assets/SearchAllReference")]
        public static void OnSearchForReferences()
        {
            if (Selection.gameObjects.Length != 1)
            {
                return;
            }

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    EditorApplication.OpenScene(scene.path);
                    GameObject[] gos = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
                    foreach (GameObject go in gos)
                    {
                        if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                        {
                            UnityEngine.Object parentObject = PrefabUtility.GetPrefabParent(go);
                            string path = AssetDatabase.GetAssetPath(parentObject);
                            if (path == AssetDatabase.GetAssetPath(Selection.activeGameObject))
                            {
                                Debug.Log(scene.path.Replace("Assets/",""));
                            }
                        }
                    }
                }
            }
        }
        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    }
}