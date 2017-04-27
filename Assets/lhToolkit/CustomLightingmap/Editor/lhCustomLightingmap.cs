using UnityEngine;
using UnityEditor;

namespace LaoHan.Tools.CustomLightingmap
{
    public class lhCustomLightingmap : EditorWindow
    {
        private int maxAtlasHeight=512;
        private int maxAtlasWidth=512;

        [MenuItem("lhTools/CustomLightingmap %t")]
        static void Intialize()
        {
            lhCustomLightingmap window = EditorWindow.GetWindow<lhCustomLightingmap>(true, "CustomLightingmap    (LaoHan)      QQ:369016334", true);
            window.Show();
        }
        void OnDestroy()
        {
        }
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                maxAtlasHeight = EditorGUILayout.IntField(new GUIContent("maxAtlasHeight"), maxAtlasHeight);
                maxAtlasWidth = EditorGUILayout.IntField(new GUIContent("maxAtlasHeight"), maxAtlasHeight);
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Clear"))
                    {
                        Clear();
                    }
                    if (GUILayout.Button("Bake"))
                    {
                        Bake();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("尺寸需要是2的幂次方",MessageType.Info);
            }
            EditorGUILayout.EndVertical();
        }
        void Clear()
        {
            Lightmapping.Clear();
        }
        void Bake()
        {
            LightmapEditorSettings.maxAtlasHeight = maxAtlasHeight;
            LightmapEditorSettings.maxAtlasWidth = maxAtlasWidth;
            Lightmapping.Clear();
            Lightmapping.Bake();
        }
    }
}