using UnityEngine;

namespace LaoHan.Infrastruture
{
    public enum ProjectType
    {
        develop = 0x01,
        debug = 0x02,
        release = 0x03
    }
    public enum Orientation
    {
        None,
        Horizontal,
        Vertical
    }
    public class lhDefine
    {
        public static readonly string platform=
#if UNITY_STANDALONE_WIN
            "StandaloneWindows";
#elif UNITY_STANDALONE_OSX
        "StandaloneOSXIntel";
#elif UNITY_WEBPLAYER
        "WebPlayer";
#elif UNITY_IPHONE
        "iOS";
#elif UNITY_ANDROID
        "Android";
#elif UNITY_METRO
        "wsaplayer";
#elif UNITY_WP8
        "wp8";
#else
        "unknown";
#endif
        public static readonly ProjectType projectType =
#if RELEASE
        ProjectType.release;
#else
    #if DEVELOP
        ProjectType.develop;
    #elif DEBUG && !DEVELOP
        ProjectType.debug;
    #else
        ProjectType.develop;
    #endif
#endif

        public static readonly string languageType = "chinese";
        public static Orientation orientation =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
 (UnityEngine.Screen.width / UnityEngine.Screen.height) >= 1 ? Orientation.Horizontal : Orientation.Vertical;
#elif UNITY_ANDROID || UNITY_IPHONE
    Orientation.Horizontal;
#endif
        public static readonly string streamingAssetsUrl =
#if UNITY_ANDROID && !UNITY_EDITOR
            Application.streamingAssetsPath;
#else
    #if RELEASE
                 "file://" + Application.streamingAssetsPath + "/";
    #else
                Application.streamingAssetsPath + "/";
    #endif
#endif
        public static readonly string persistentUrl = Application.persistentDataPath+ "/lhVersion/";
        public static readonly string dataUrl = Application.dataPath + "/Resources/";
        public static readonly string tempUrl = Application.persistentDataPath + "/lhTemp/";
        public static readonly string cacheFilePath = Application.persistentDataPath + "/lhCache/HistoryData.txt";
        public static readonly string sourceUrl =
#if RELEASE
        persistentUrl;
#else
    #if DEVELOP
        dataUrl;
    #elif DEBUG && !DEVELOP
        streamingAssetsUrl;
    #else
        dataUrl;
    #endif
#endif


        public static readonly string manifestUrl = sourceUrl + bundleFolder + "/" + platform + "/" + platform;
        public static string sourceVariant = "sd";
        //-------------------------------------------------------------------------------------Bundle
        public const string bundleFolder = "lhBundle";
        public const string sceneFolder = "lhScene";
        public const string bundleStuff = ".bundle";
        public const string sceneStuff = ".unity3d";
        //-------------------------------------------------------------------------------------UI
        public static readonly float currentUIRatio = (float)Screen.width / (float)Screen.height;
        public const float baseUIRatio = 1920f / 1080f;
        public const float baseUIWidth = 1920f;
        public const float baseUIHeight = 1080f;

    }
}
