using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace LaoHan.Tools.RemoveUnusedComponent
{
    public class lhRemoveUnusedComponent:Editor
    {
        [MenuItem("lhTools/UnusedComponent/Remove unusedComponent")]
        static public void Remove()
        {
            GameObject[] rootObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in rootObjects)
            {
                if (go != null && go.transform.parent != null)
                {
                    Renderer render = go.GetComponent<Renderer>();
                    if (render != null && render.sharedMaterial != null && render.sharedMaterial.shader.name == "Diffuse" && render.sharedMaterial.color == Color.white)
                    {
                        render.sharedMaterial.shader = Shader.Find("Mobile/Diffuse");
                    }
                }
                
                foreach (MeshCollider collider in UnityEngine.Object.FindObjectsOfType(typeof(MeshCollider)))
                {
                    DestroyImmediate(collider);
                }
                
                foreach (Animation animation in UnityEngine.Object.FindObjectsOfType(typeof(Animation)))
                {
                    if (animation.clip == null)
                        DestroyImmediate(animation);
                }
                
                foreach (Animator animator in UnityEngine.Object.FindObjectsOfType(typeof(Animator)))
                {
                    DestroyImmediate(animator);
                }
            }
            AssetDatabase.SaveAssets();
        }
        [MenuItem("lhTools/UnusedComponent/Remove All scene unuse component")]
        static public void RemoveAll()
        {
            foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    EditorApplication.OpenScene(scene.path);
                    Remove();
                }
            }
            EditorApplication.SaveScene();
        }
        [MenuItem("lhTools/UnusedComponent/synchro SceneSetting")]
        static void CheckSceneSetting()
        {
            List<string> dirs = new List<string>();
            GetDirs(Application.dataPath, ref dirs);
            EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[dirs.Count];
            for (int i = 0; i < newSettings.Length; i++)
            {
                newSettings[i] = new EditorBuildSettingsScene(dirs[i], true);
            }
            EditorBuildSettings.scenes = newSettings;
            EditorApplication.SaveAssets();
        }
        private static void GetDirs(string dirPath, ref List<string> dirs)
        {
            foreach (string path in Directory.GetFiles(dirPath))
            {
                if (System.IO.Path.GetExtension(path) == ".unity")
                {
                    dirs.Add(path.Substring(path.IndexOf("Assets/")));
                }
            }
            if (Directory.GetDirectories(dirPath).Length > 0)
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                    GetDirs(path, ref dirs);
            }
        }
    }
}