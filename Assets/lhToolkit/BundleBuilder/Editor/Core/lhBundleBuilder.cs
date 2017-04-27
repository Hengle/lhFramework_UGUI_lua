using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using LaoHan.Infrastruture;
using System.Text;

namespace LaoHan.Tools.BundleBuilder
{
    class lhBundleBuilder
    {
        public class BuildParameter
        {
            public ApplicationPath applicationPath{get;set; }
            public string outputRootFolder { get; set; }
            public uint crc { get; set; }
            public string stuff { get; set; }
            public BuildAssetBundleOptions buildAssetBundleOptions { get; set; }
            public BuildOptions buildOptions { get; set; }
            public BuildTarget buildTarget { get; set; }
            public JsonObject ToJson()
            {
                var json = new JsonObject();
                json["applicationPath"] = new JsonNumber((int)applicationPath);
                json["outputRootFolder"] = new JsonString(outputRootFolder);
                json["crc"] = new JsonNumber(crc);
                json["stuff"] = new JsonString(stuff);
                json["buildAssetBundleOptions"] = new JsonNumber((int)buildAssetBundleOptions);
                json["buildOptions"] = new JsonNumber((int)buildOptions);
                json["buildTarget"] = new JsonNumber((int)buildTarget);
                return json;
            }
            public BuildParameter()
            {

            }
            public BuildParameter(IDictionary<string,IJsonNode> json)
            {
                applicationPath = (ApplicationPath)json["applicationPath"].AsInt();
                outputRootFolder = json["outputRootFolder"].AsString();
                crc = Convert.ToUInt32(json["crc"].AsInt());
                stuff = json["stuff"].AsString();
                buildAssetBundleOptions = (BuildAssetBundleOptions)json["buildAssetBundleOptions"].AsInt();
                buildOptions = (BuildOptions)json["buildOptions"].AsInt();
                buildTarget = (BuildTarget)json["buildTarget"].AsInt();
            }
        }
        public enum ApplicationPath
        {
            StreamingAssetsPath,
            PersistentDataPath,
            TemporaryCachePath,
            DataPath
        }
        public class AssetData
        {
            public string guid;
            public string path;
            public string hashCode;
            public string metaHashCode;
            public AssetData(string path)
            {
                this.path = path;
                var shal1=new System.Security.Cryptography.SHA1Managed();
                byte[] fileBytes=File.ReadAllBytes(Application.dataPath.Replace("Assets","")+path);
                byte[] hashBytes = shal1.ComputeHash(fileBytes); 
                hashCode = Convert.ToBase64String(hashBytes);
                string metaFile = Application.dataPath.Replace("Assets", "") + path + ".meta";
                if (File.Exists(metaFile))
                {
                    byte[] metaBytes = File.ReadAllBytes(metaFile);
                    metaHashCode = Convert.ToBase64String(shal1.ComputeHash(metaBytes));
                }
                else
                    metaHashCode = "";
                guid = AssetDatabase.AssetPathToGUID(path);
            }
            public AssetData(IDictionary<string,IJsonNode> json)
            {
                guid = json["guid"].AsString();
                path = json["path"].AsString();
                hashCode = json["hashCode"].AsString();
                metaHashCode = json["metaHashCode"].AsString();

            }
            public bool CheckHas()
            {
                if (File.Exists(path))
                    return true;
                else
                    return false;
            }
            public JsonObject ToJson()
            {
                var json = new JsonObject();
                json["guid"] = new JsonString(guid);
                json["path"] = new JsonString(path);
                json["hashCode"] = new JsonString(hashCode);
                json["metaHashCode"] = new JsonString(metaHashCode);
                return json;
            }
        }
        public class BundleGroup
        {
            #region class
            public class DependenceNode
            {
                public string bundleName { get; set; }
                public int priority { get; set; }
                public List<AssetData> includeList { get; set; }
                public List<AssetData> referenceList { get; set; }

                public AssetData mainAsset { get; set; }
                public BundleType bundleType { get; set; }

                public BundleGroup group { get; set; }
                public DependenceNode parent { get; private set; }
                public List<DependenceNode> childList { get; private set; }
                public string oldBundleName { get; set; }
                public bool editorOpen { get; set; }
                public int layer { get; set; }
                public int index { get; set; }
                public bool renaming { get; set; }
                public bool isSelected { get; set; }
                public int version { get; set; }
                public long size { get; set; }

                public Action<DependenceNode> deleteHandler;

                public DependenceNode(BundleGroup group, DependenceNode parent,string bundleName, bool editorOpen, int layer, int index, BundleType bundleType, Action<DependenceNode> onDeleteOwn)
                {
                    this.group = group;
                    this.bundleName = bundleName;
                    this.oldBundleName = bundleName;
                    this.editorOpen = editorOpen;
                    this.bundleType = bundleType;
                    this.layer = layer;
                    this.index = index;
                    this.parent = parent;
                    renaming = true;
                    childList = new List<DependenceNode>();
                    includeList = new List<AssetData>();
                    referenceList = new List<AssetData>();

                    deleteHandler = onDeleteOwn;
                }
                public DependenceNode(IDictionary<string, IJsonNode> json,BundleGroup group, DependenceNode parent, Action<DependenceNode> onDeleteOwn)
                {
                    this.parent = parent;
                    deleteHandler = onDeleteOwn;
                    this.group = group;
                    childList = new List<DependenceNode>();
                    includeList = new List<AssetData>();
                    referenceList = new List<AssetData>();
                    bundleName = json["bundleName"].AsString();
                    priority = json["priority"].AsInt();
                    if (json["mainAsset"].type==EJsonType.Object)
                    {
                        mainAsset = new AssetData(json["mainAsset"].AsDict());
                        if (!mainAsset.CheckHas())
                            mainAsset = null;
                    }
                    bundleType = (BundleType)json["bundleType"].AsInt();
                    editorOpen=Convert.ToBoolean(json["editorOpen"].AsString());
                    layer=json["layer"].AsInt();
                    index=json["index"].AsInt();
                    version = json["version"].AsInt();
                    //size = (long)json["size"].AsDouble();

                    var includeJson = json["includeList"].AsList();
                    for (int i = 0; i < includeJson.Count; i++)
                    {
                        var data = new AssetData(includeJson[i].AsDict());
                        if (!File.Exists(GetRealPath(data.path)))
                            continue;
                        includeList.Add(data);
                        size += GetFileSize(data.path);
                    }

                    var referenceJson = json["referenceList"].AsList();
                    for (int i = 0; i < referenceJson.Count; i++)
                    {
                        var data = new AssetData(referenceJson[i].AsDict());
                        if (!File.Exists(GetRealPath(data.path)))
                            continue;
                        referenceList.Add(data);
                    }
                    var childJson = json["childList"].AsList();
                    for (int i = 0; i < childJson.Count; i++)
                    {
                        childList.Add(new DependenceNode(childJson[i].AsDict(),group, this, DeleteChild));
                    }
                }
                public void CreateBundle()
                {
                    editorOpen = true;
                    childList.Add(new DependenceNode(group, this, "new AssetBundle" + (layer + 1) + "_" + childList.Count, false, layer + 1, childList.Count, BundleType.AssetBundle, DeleteChild));
                }
                public void CreateScene()
                {
                    editorOpen = true;
                    childList.Add(new DependenceNode(group, this, "new Scene" + (layer + 1) + "_" + childList.Count, false, layer + 1, childList.Count, BundleType.Scene, DeleteChild));
                }
                public JsonObject BuildBundle(string groupFolder)
                {
                    Build(groupFolder);
                    return BuildConfig();
                }
                public void Build(string groupFolder)
                {
                    version++;
                    BuildPipeline.PushAssetDependencies();
                    if (bundleType == BundleType.AssetBundle)
                        BuildAssetBundle(groupFolder);
                    else
                        BuildScene(groupFolder);
                    if (childList.Count != 0)
                    {
                        for (int i = 0; i < childList.Count;i++ )
                            childList[i].Build(groupFolder);
                    }
                    BuildPipeline.PopAssetDependencies();
                }
                public JsonObject BuildConfig()
                {
                    JsonObject json = new JsonObject();

                    JsonObject jsonObj = new JsonObject();
                    jsonObj["bundleName"] = new JsonString(bundleName + lhBundleBuilder.buildParameter.stuff);
                    jsonObj["mainAsset"] =new JsonString(Path.GetFileNameWithoutExtension(mainAsset.path));
                    JsonArray allAssets = new JsonArray();
                    for (int i = 0; i < includeList.Count; i++)
                    {
                        allAssets.Add(new JsonString(Path.GetFileNameWithoutExtension(includeList[i].path)));
                    }
                    jsonObj["allAssets"] = allAssets;
                    JsonArray dependencyJson = new JsonArray();
                    var dependencyList = GetAllDependencies();
                    for (int i = 0; i < dependencyList.Count; i++)
                    {
                        dependencyJson.Add(new JsonString(dependencyList[i]));
                    }
                    jsonObj["dependencies"] = dependencyJson;
                    json[bundleName]=jsonObj;
                    if(childList!=null)
                    {
                        for (int i = 0; i < childList.Count; i++)
                        {
                            json.AddArrayValue(childList[i].BuildConfig());
                        }
                    }
                    return json;
                }
                public void DeleteInclude(string path)
                {
                    AssetData data = GetTargetPath(includeList, path);
                    if(data!=null)
                    {
                        includeList.Remove(data);
                        size -= GetFileSize(path);
                    }
                    UpdateReference();
                    AssetData mainData = GetTargetPath(includeList, mainAsset.path);
                    if (mainData == null)
                    {
                        mainAsset = mainData;
                    }
                    else
                        mainAsset = null;
                }
                public void AddInclude(string[] paths)
                {
                    for(int i=0;i<paths.Length;i++)
                    {
                        AssetData data = GetTargetPath(includeList,paths[i]);
                        if(data==null)
                        {
                            data = new AssetData(paths[i]);
                            includeList.Add(data);
                            size += (new FileInfo(Application.dataPath.Replace("Assets","")+paths[i])).Length;
                        }
                    }
                    string[] referenceArr = AssetDatabase.GetDependencies(paths);
                    for (int j = 0; j < referenceArr.Length; j++)
                    {
                        AssetData referenceData = GetTargetPath(referenceList, referenceArr[j]);
                        if (referenceData == null)
                        {
                            referenceData = new AssetData(referenceArr[j]);
                            if (GetTargetPath(includeList, referenceArr[j])!=null)continue;
                            referenceList.Add(referenceData);
                        }
                    }
                    if (mainAsset==null && includeList.Count != 0)
                        mainAsset = includeList[0];

                }
                public void DeleteOwn()
                {
                    deleteHandler(this);
                }
                public bool HasNode(DependenceNode node)
                {
                    bool has = false;
                    for(int i=0;i<childList.Count;i++)
                    {
                        if (childList[i] == node)
                            has = true;
                        else
                            has=childList[i].HasNode(node);
                    }
                    return has;
                }
                public bool HasSelected()
                {
                    bool has = false;
                    if (isSelected)
                        has = true;
                    else
                    {
                        for (int i = 0; i < childList.Count; i++)
                        {
                            has = childList[i].HasSelected();
                        }
                    }
                    return has;
                }
                public void ChangeLayer(int layer)
                {
                    this.layer = layer;
                    if(childList.Count!=0)
                    {
                        for (int i = 0; i < childList.Count; i++)
                            childList[i].ChangeLayer(layer + 1);
                    }
                }
                public void ChangeIndex(int index)
                {
                    this.index = index;
                }
                public void AddChild(DependenceNode node)
                {
                    node.parent = this;
                    node.ChangeLayer(layer+1);
                    editorOpen = true;
                    node.deleteHandler = DeleteChild;
                    childList.Add(node);
                    node.ChangeIndex(childList.IndexOf(node));
                }
                public void DeleteIncludeAll()
                {
                    includeList.Clear();
                    referenceList.Clear();

                }
                public List<AssetData> GetParentReferences()
                {
                    List<AssetData> list = new List<AssetData>();
                    if (parent != null)
                    {
                        list.AddRange(parent.referenceList);
                        list.AddRange(parent.includeList);
                        list.AddRange(parent.GetParentReferences());
                    }
                    return list;
                }
                public bool HasSameName(DependenceNode node,string nodeName)
                {
                    if (string.Equals(bundleName, nodeName) && node != this)
                        return true;
                    bool has=false;
                    foreach(var child in childList)
                    {
                        if (string.Equals(child.bundleName, nodeName) && child != node)
                        {
                            has = true;
                            break;
                        }
                        has = child.HasSameName(node, nodeName);
                        if (has)
                            break;
                    }
                    return has;
                }
                public JsonObject ToJson()
                {
                    var json = new JsonObject();
                    json["bundleName"] = new JsonString(bundleName);
                    json["priority"] = new JsonNumber(priority);
                    json["mainAsset"] =mainAsset==null?null: mainAsset.ToJson();
                    json["bundleType"] = new JsonNumber((int)bundleType);
                    json["editorOpen"] = new JsonString(editorOpen.ToString());
                    json["layer"] = new JsonNumber(layer);
                    json["index"] = new JsonNumber(index);
                    json["version"] = new JsonNumber(version);
                    json["size"] = new JsonNumber(size);

                    var includeJson = new JsonArray();
                    for (int i = 0; i < includeList.Count; i++)
                    {
                        includeJson.Add(includeList[i].ToJson());
                    }
                    json["includeList"] = includeJson;

                    var referenceJson = new JsonArray();
                    for (int i = 0; i < referenceList.Count; i++)
                    {
                        referenceJson.Add(referenceList[i].ToJson());
                    }
                    json["referenceList"] = referenceJson;

                    var childJson = new JsonArray();
                    for (int i = 0; i < childList.Count; i++)
                    {
                        childJson.Add(childList[i].ToJson());
                    }
                    json["childList"] = childJson;
                    return json;
                }
                private void BuildAssetBundle(string groupFolder)
                {
                    string outputPath = CombinePath(
                        lhBundleBuilder.GetApplicationPath(),
                        lhBundleBuilder.buildParameter.outputRootFolder,
                        lhBundleBuilder.buildParameter.buildTarget.ToString(), 
                        groupFolder,
                        bundleName +  lhBundleBuilder.buildParameter.stuff
                        );
                    FileInfo fileInfo = new FileInfo(outputPath);
                    if (!fileInfo.Directory.Exists)
                        fileInfo.Directory.Create();
                    uint crc = lhBundleBuilder.buildParameter.crc;
                    BuildAssetBundleOptions options = lhBundleBuilder.buildParameter.buildAssetBundleOptions;
                    BuildTarget target = lhBundleBuilder.buildParameter.buildTarget;

                    // Load all of assets in this bundle
                    List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
                    foreach (var include in includeList)
                    {
                        if (mainAsset.path.Equals(include)) continue;
                        UnityEngine.Object[] assetsAtPath = AssetDatabase.LoadAllAssetsAtPath(include.path);
                        if (assetsAtPath != null || assetsAtPath.Length != 0)
                            assets.AddRange(assetsAtPath);
                        else
                            Debug.LogError("LaoHan:Cannnot load [" + include + "] as asset object");
                    }

                    UnityEngine.Object main = AssetDatabase.LoadAssetAtPath(mainAsset.path,typeof(UnityEngine.Object));
                    bool succeed = BuildPipeline.BuildAssetBundle(main,
                                                                    assets.ToArray(),
                                                                    outputPath,
                                                                    out crc,
                                                                    options,
                                                                    target);
                    //return succeed;
                }
                private void BuildScene(string groupFolder)
                {
                    string outputPath = CombinePath(
                        lhBundleBuilder.GetApplicationPath(),
                        lhBundleBuilder.buildParameter.outputRootFolder,
                        lhBundleBuilder.buildParameter.buildTarget.ToString(),
                        groupFolder,
                        bundleName + lhBundleBuilder.buildParameter.stuff
                        );
                    FileInfo fileInfo = new FileInfo(outputPath);
                    if (!fileInfo.Directory.Exists)
                        fileInfo.Directory.Create();
                    uint crc = lhBundleBuilder.buildParameter.crc;
                    BuildOptions options = lhBundleBuilder.buildParameter.buildOptions;
                    BuildTarget target = lhBundleBuilder.buildParameter.buildTarget;

                    string[] sceneArr = new string[includeList.Count];
                    for (int i = 0; i < includeList.Count; i++)
                    {
                        sceneArr[i] = includeList[i].path;
                    }
                    string error = BuildPipeline.BuildStreamedSceneAssetBundle(sceneArr, outputPath, target, out crc, options);
                }
                private void DeleteChild(DependenceNode node)
                {
                    childList.Remove(node);
                }
                private void UpdateReference()
                {
                    string[] pathArr = new string[includeList.Count];
                    for (int i = 0; i < includeList.Count; i++)
                    {
                        pathArr[i] = includeList[i].path;
                    }
                    string[] referenceArr = AssetDatabase.GetDependencies(pathArr);
                    referenceList.Clear();
                    foreach(string path in referenceArr)
                    {
                        var includeData = GetTargetPath(includeList, path);
                        if (includeData!=null)
                            continue;
                        referenceList.Add(new AssetData(path));
                    }
                }
                private string CombinePath(params string[] pathArr)
                {
                    if(pathArr.Length<1)return "";
                    string path=pathArr[0];
                    for(int i=1;i<pathArr.Length;i++)
                    {
                        path=Path.Combine(path,pathArr[i]);
                    }
                    return path;
                }
                private string GetFileName(string file)
                {
                    FileInfo info = new FileInfo(file);
                    return info.Name.Replace(info.Extension, "");
                }
                private List<string> GetAllDependencies()
                {
                    List<string> list=new List<string>();
                    if(parent!=null)
                    {
                        list.Add(parent.bundleName + lhBundleBuilder.buildParameter.stuff);
                        list.AddRange(parent.GetAllDependencies());
                    }
                    return list;
                }
                private AssetData GetTargetGuid(List<AssetData> list, string guid)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].guid.Equals(guid))
                            return list[i];
                    }
                    return null;
                }
                private AssetData GetTargetPath(List<AssetData> list, string path)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].path.Equals(path))
                            return list[i];
                    }
                    return null;
                }
                public string GetRealPath(string path)
                {
                    return Application.dataPath.Replace("Assets", "") + path;
                }
                private long GetFileSize(string path)
                {
                    return (new FileInfo(Application.dataPath.Replace("Assets", "") + path)).Length;
                }
            }
            public enum BundleType
            {
                AssetBundle=0,
                Scene=1
            }
            #endregion

            #region property
            public string groupName { get { return m_groupName; } set { m_groupName=value ; } }

            public string oldGroupName { get; set; }
            public bool editorOpen { get; set; }
            public bool renaming { get; set; }
            public List<DependenceNode> treeList
            {
                get { return m_treeList; }
                set { m_treeList = value; }
            }
            #endregion

            #region private member
            private string m_groupName="";
            private List<DependenceNode> m_treeList;
            
            #endregion

            #region Ctor
            public BundleGroup(string groupName)
            {
                this.m_groupName = groupName;
                this.oldGroupName = groupName;
                renaming = true;
                m_treeList = new List<DependenceNode>();
            }
            public BundleGroup(IDictionary<string,IJsonNode> json)
            {
                m_treeList = new List<DependenceNode>();
                groupName = json["groupName"].AsString();
                this.oldGroupName = groupName;
                editorOpen = Convert.ToBoolean(json["editorOpen"].AsString());

                var treeJson = json["treeList"].AsList();
                for (int i = 0; i < treeJson.Count; i++)
                {
                    m_treeList.Add(new DependenceNode(treeJson[i].AsDict(),this, null, DeleteTree));
                }
            }
            #endregion

            #region public methods
            public void CreateBundle()
            {
                m_treeList.Add(new DependenceNode(this,null, "new AssetBundle_0_"+m_treeList.Count,false, 0,0, BundleType.AssetBundle, DeleteTree));
                editorOpen = true;
            }
            public void BuildAll()
            {
                string configPath = CombinePath(
                    lhBundleBuilder.GetApplicationPath(),
                    lhBundleBuilder.buildParameter.outputRootFolder,
                    lhBundleBuilder.buildParameter.buildTarget.ToString(),
                    m_groupName,
                    m_groupName
                    );
                FileInfo configFileInfo = new FileInfo(configPath);
                if (!configFileInfo.Directory.Exists)
                    configFileInfo.Directory.Create();
                configFileInfo = null;

                var json = new JsonObject();
                json["manifestFileVersion"] = new JsonNumber(1);
                json["crc"] = new JsonNumber(lhBundleBuilder.buildParameter.crc);
                var assetBundleManifest = new JsonObject();

                var jsonObject = new JsonObject();
                for (int i = 0; i < m_treeList.Count; i++)
                {
                    jsonObject.AddArrayValue(m_treeList[i].BuildBundle(m_groupName));
                }
                jsonObject=CheckUnusedFile(configPath, jsonObject);
                assetBundleManifest["assetBundleInfos"] = jsonObject;

                json["assetBundleManifest"] = assetBundleManifest;
                File.WriteAllText(configPath, lhJson.PrettyPrint(json.ToString()));

                EditorUtility.DisplayDialog("Information", "Build Success", "Ok");
            }
            public void BuildSelected()
            {
                string configPath = CombinePath(
                    lhBundleBuilder.GetApplicationPath(),
                    lhBundleBuilder.buildParameter.outputRootFolder,
                    lhBundleBuilder.buildParameter.buildTarget.ToString(),
                    m_groupName,
                    m_groupName
                    );
                FileInfo configFileInfo = new FileInfo(configPath);
                if (!configFileInfo.Directory.Exists)
                    configFileInfo.Directory.Create();
                configFileInfo = null;
                JsonObject jsonObject = new JsonObject();
                for (int i = 0; i < m_treeList.Count; i++)
                {
                    if (m_treeList[i].HasSelected())
                    {
                        jsonObject.AddArrayValue(m_treeList[i].BuildBundle(m_groupName));
                    }
                }
                JsonObject json=null;
                if (File.Exists(configPath))
                {
                    json = lhJson.Parse(File.ReadAllText(configPath)) as JsonObject;
                    json["manifestFileVersion"] = new JsonNumber(1);
                    json["crc"] = new JsonNumber(lhBundleBuilder.buildParameter.crc);
                    var assetBundleManifest = json["assetBundleManifest"].AsDict();
                    var oldObj = assetBundleManifest["assetBundleInfos"].AsDict();
                    var newObj = ((JsonObject)oldObj);
                    newObj.AddArrayValue(jsonObject);
                    newObj=CheckUnusedFile(configPath, newObj);
                    assetBundleManifest["assetBundleInfos"] = newObj;
                    File.WriteAllText(configPath, lhJson.PrettyPrint(json.ToString()));
                }
                else
                {
                    json = new JsonObject();
                    json["manifestFileVersion"] = new JsonNumber(1);
                    json["crc"] = new JsonNumber(lhBundleBuilder.buildParameter.crc);
                    var assetBundleManifest = new JsonObject();
                    jsonObject=CheckUnusedFile(configPath, jsonObject);
                    assetBundleManifest["assetBundleInfos"] = jsonObject;

                    json["assetBundleManifest"] = assetBundleManifest;
                    File.WriteAllText(configPath, lhJson.PrettyPrint(json.ToString()));
                }
                EditorUtility.DisplayDialog("Information", "Build Success", "Ok");
            }
            public void Clear()
            {
                if (EditorUtility.DisplayDialog("Warning:", "Delete All\n You can never recover, please be careful.", "ok", "cancel"))
                {
                    if (EditorUtility.DisplayDialog("老晗:", "卧槽，你真要全删啊？删了就真无法恢复了啊", "确定", "取消"))
                    {
                        m_treeList.Clear();
                    }
                }
            }
            public void AddTree(DependenceNode tree)
            {
                if (m_treeList.Contains(tree)) return;
                tree.ChangeLayer(0);
                tree.deleteHandler = DeleteTree;
                m_treeList.Add(tree);
                tree.ChangeIndex(m_treeList.IndexOf(tree));
            }
            public void DeleteTree(DependenceNode tree)
            {
                m_treeList.Remove(tree);
            }
            public bool HasSameNodeName(DependenceNode node,string nodeName)
            {
                bool has=false;
                foreach(var tree in m_treeList)
                {
                    if (tree.HasSameName(node, nodeName))
                    {
                        has = true;
                        break;
                    }
                }
                return has;
            }
            public JsonObject ToJson()
            {
                var json = new JsonObject();
                json["groupName"] = new JsonString(m_groupName);
                json["editorOpen"] = new JsonString(editorOpen.ToString());
                var list = new JsonArray();
                for (int i = 0; i < m_treeList.Count; i++)
                {
                    list.Add(m_treeList[i].ToJson());
                }
                json["treeList"] = list;
                return json;
            }
            private string CombinePath(params string[] pathArr)
            {
                if (pathArr.Length < 1) return "";
                string path = pathArr[0];
                for (int i = 1; i < pathArr.Length; i++)
                {
                    path = Path.Combine(path, pathArr[i]);
                }
                return path;
            }
            private JsonObject CheckUnusedFile(string configPath,JsonObject json)
            {
                FileInfo fileInfo=new FileInfo(configPath);
                DirectoryInfo directoryInfo =fileInfo.Directory;
                FileInfo[] files = directoryInfo.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo info = files[i];
                    if (info.Name.Equals(fileInfo.Name)) continue;
                    if (info.Name.Contains(".meta"))
                    {
                        files[i] = null;
                        continue;
                    }
                    string extension = files[i].Extension;
                    string fileName = files[i].Name;
                    fileName = fileName.Replace(extension, "");
                    if (!json.ContainsKey(fileName))
                        info.Delete();
                }
                List<string> waitRemove = new List<string>();
                foreach (var item in json)
                {
                    bool has=false;
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] == null) continue;
                        string extension=files[i].Extension;
                        string fileName=files[i].Name;
                        if (!string.IsNullOrEmpty(extension))
                             fileName =fileName.Replace(extension, "");
                        if (fileName.Equals(item.Key))
                        {
                            has = true;
                            break;
                        }
                    }
                    if(!has)
                    {
                        waitRemove.Add(item.Key);
                    }
                }
                foreach (var item in waitRemove)
                {
                    json.Remove(item);
                }
                return json;
            }
            #endregion
        }

        #region public static property
        public static List<BundleGroup> bundleGroup
        {
            get
            {
                return m_instance.m_bundleGroup;
            }
        }
        public static BuildParameter buildParameter
        {
            get { return m_instance.m_buildParameter; }
            set{m_instance.m_buildParameter = value;}
        }
        
        #endregion

        #region private member
        private List<BundleGroup> m_bundleGroup;
        private BuildParameter m_buildParameter;
        private string configPath = Application.dataPath + "/lhToolkit/BundleManager/Sources/BundleConfig";
        #endregion

        private static lhBundleBuilder m_instance;
        public static lhBundleBuilder GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhBundleBuilder();
        }
        lhBundleBuilder()
        {
            m_bundleGroup = new List<BundleGroup>();
            Initialize();
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public void CreateGroup()
        {
            string groupName = "new group" + m_instance.m_bundleGroup.Count;
            m_bundleGroup.Add(new BundleGroup(groupName));
        }
        public void DeleteGroup(BundleGroup group)
        {
            m_bundleGroup.Remove(group);
        }
        public void Apply()
        {
            var json = new JsonObject();
            json["buildParameter"] = m_buildParameter.ToJson();
            var list = new JsonArray();
            for (int i = 0; i < m_bundleGroup.Count; i++)
            {
                list.Add(m_bundleGroup[i].ToJson());
            }
            json["bundleGroup"] = list;
            File.WriteAllText(configPath, lhJson.PrettyPrint(json.ToString()));
            EditorUtility.DisplayDialog("Information", "Save success", "Ok");
        }
        public void Revert()
        {
            m_bundleGroup.Clear();
            m_buildParameter = null;
            Initialize();
            EditorUtility.DisplayDialog("Information", "Rever success", "Ok");
        }
        public void BuildAll()
        {
            for (int i = 0; i < m_bundleGroup.Count; i++)
            {
                m_bundleGroup[i].BuildAll();
            }
        }
        public void Clear()
        {
            bundleGroup.Clear();
        }
        public bool HasSameGroupName(BundleGroup group,string groupName)
        {
            bool has=false;
            foreach(var g in bundleGroup)
            {
                if(g.groupName==groupName && g!=group)
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        private void Initialize()
        {
            if (File.Exists(configPath))
            {
                string config = File.ReadAllText(configPath);
                var json = lhJson.Parse(config) as JsonObject;
                m_buildParameter = new BuildParameter(json["buildParameter"].AsDict());
                var bundleGroupJson = json["bundleGroup"].AsList();
                for (int i = 0; i < bundleGroupJson.Count; i++)
                {
                    m_bundleGroup.Add(new BundleGroup(bundleGroupJson[i].AsDict()));
                }
            }
            else
            {
                m_buildParameter = new BuildParameter()
                {
                    applicationPath = ApplicationPath.StreamingAssetsPath,
                    stuff = ".assetbundle",
                    outputRootFolder = "lhBundle",
                    crc = 0,
                    buildAssetBundleOptions = BuildAssetBundleOptions.CollectDependencies,
                    buildOptions = BuildOptions.Development,
                    buildTarget = BuildTarget.Android
                };
            }
        }
        public static string GetApplicationPath()
        {
            switch (m_instance.m_buildParameter.applicationPath)
            {
                case ApplicationPath.DataPath:
                    return Application.dataPath;
                case ApplicationPath.PersistentDataPath:
                    return Application.persistentDataPath;
                case ApplicationPath.StreamingAssetsPath:
                    return Application.streamingAssetsPath;
                case ApplicationPath.TemporaryCachePath:
                    return Application.temporaryCachePath;
                default:
                    return Application.streamingAssetsPath;
            }
        }
    }
}
