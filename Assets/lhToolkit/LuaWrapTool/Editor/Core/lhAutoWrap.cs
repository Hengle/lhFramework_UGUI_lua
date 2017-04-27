using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Text;
using System.Reflection;
using System.IO;
using LaoHan.Infrastruture;

namespace LaoHan.Tools.AutoWrap
{
    public class TypeNode
    {
        public string assemblyName;
        public Type type;
        public bool hasWrap;
        public string wrapName;

    }
    public class lhAutoWrap
    {
        public enum FilterGroup
        {
            Wrapped,
            NoneWrap,
            All
        }
        public class BindType
        {
            public string className="";                 //类名称
            public string baseClass = "";
            public string wrapName = "";        //产生的wrap文件名字
            public string libName = "";         //注册到lua的名字
            public string nameSpace = "";     //注册到lua的table层级
            public string assemblyName = "";    //所属程序集的名字
            public bool preload = false;        //是否预先加载

            public bool isStatic=false;     //是否是静态的类
            public bool drop = false;           //无需的功能
            public bool isBuilded=false;          //是否已经wrap了
            public bool isDelegate = false;

            public string typeClassify;

            public Type type;
            public BindType(Type t)
            {
                type = t;
                nameSpace = ToLuaExport.GetNameSpace(t, out libName);
                className = ToLuaExport.CombineTypeStr(nameSpace, libName);
                libName = ToLuaExport.ConvertToLibSign(libName);

                if (typeof(System.Delegate).IsAssignableFrom(t) || typeof(System.MulticastDelegate).IsAssignableFrom(t))
                {
                    isDelegate = true;
                    typeClassify = "delegate";
                }
                else
                {

                    if (className == "object")
                    {
                        wrapName = "System_Object";
                        className = "System.Object";
                    }
                    else if (className == "string")
                    {
                        wrapName = "System_String";
                        className = "System.String";
                    }
                    else
                    {
                        wrapName = className.Replace('.', '_');
                        wrapName = ToLuaExport.ConvertToLibSign(wrapName);
                    }
                    wrapName += "Wrap";
                    if (t.BaseType != null && t.BaseType != typeof(ValueType))
                    {
                        baseClass = t.BaseType.AssemblyQualifiedName;
                    }
                
                    if ((t.GetConstructor(Type.EmptyTypes) == null && t.IsAbstract && t.IsSealed))
                    {
                        isStatic = true;
                        baseClass = baseClass == typeof(object).AssemblyQualifiedName ? null : baseClass;
                    }
                    if (t.IsAbstract)
                         typeClassify = "abstract";
                    else if (t.IsInterface)
                        typeClassify = "interface";
                    else if (t.IsEnum)
                        typeClassify = "enum";
                    else if (t.IsClass && isStatic)
                        typeClassify = "static class";
                    else
                        typeClassify = "class";

                }
            }
        }
        public class AssemblyGroup
        {
            public string assemblyName;
            public bool open;
            public List<BindType> allTypeList;
            public List<BindType> searchTypeList;
            public List<BindType> selectedList;

            public List<BindType> wrappedCustomTypeList;
            public List<BindType> wrappedDelegateList;
            public List<BindType> dropedTypeList;
            
            public string search;
            public string oldSearch;
            public FilterGroup filterGroup;


            public AssemblyGroup(Assembly assembly)
            {
                this.assemblyName = assembly.ToString();
                allTypeList=new List<BindType>();
                searchTypeList = new List<BindType>();
                selectedList = new List<BindType>();
                wrappedDelegateList = new List<BindType>();
                dropedTypeList = new List<BindType>();
                wrappedCustomTypeList = new List<BindType>();
                foreach (var type in assembly.GetTypes())
	            {
                    var bindType = new BindType(type);
                    bindType.assemblyName = this.assemblyName;
                    allTypeList.Add(bindType);
	            }
                //allTypeList = Filter(allTypeList);
            }
            public void Deserialize(IJsonNode json)
            {
                if (json == null) return;
                JsonObject jsonObject = json as JsonObject;
                var bindType=allTypeList.Find(p => { return p.nameSpace == jsonObject["nameSpace"].AsString() && p.className == jsonObject["className"].AsString(); });
                if (bindType!=null)
                {
                    bindType.preload = jsonObject["preload"].AsBool();
                    bindType.drop = jsonObject["drop"].AsBool();
                    bindType.wrapName = jsonObject["wrapName"].AsString();
                    bindType.libName = jsonObject["libName"].AsString();
                    bindType.baseClass = jsonObject["baseClass"].AsString();
                    if (!bindType.drop)
                        bindType.isBuilded = true;
                    if (bindType.isDelegate)
                    {
                        if (!wrappedDelegateList.Contains(bindType))
                        {
                            wrappedDelegateList.Add(bindType);
                        }
                    }
                    else if (bindType.drop)
                    {
                        if (!dropedTypeList.Contains(bindType))
                        {
                            dropedTypeList.Add(bindType);
                        }
                    }
                    else
                    {
                        if (!wrappedCustomTypeList.Contains(bindType))
                        {
                            wrappedCustomTypeList.Add(bindType);
                        }
                    }
                }
            }
            public void SearchTarget(string search)
            {
                searchTypeList.Clear();
                for (int i = 0; i < allTypeList.Count; i++)
                {
                    if (allTypeList[i].type.FullName.Contains(search))
                    {
                        searchTypeList.Add(allTypeList[i]);
                    }
                }
            }
            public void DeleteWrap(object userData)
            {
                BindType bindType = (BindType)userData;
                var files=Directory.GetFiles(generateDir, bindType.wrapName+".cs",SearchOption.AllDirectories);
                bindType.isBuilded = false;
                wrappedCustomTypeList.Remove(bindType);
                m_instance.GenDelegate();
                m_instance.GenLuaBinder();
                if (files.Length != 0)
                    File.Delete(files[0]);
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "DeleteWrap success!", "Ok");
                AssetDatabase.Refresh();
            }
            public void GenerateWrap(object userData)
            {
                BindType bindType = (BindType)userData;
                if (!string.IsNullOrEmpty(bindType.baseClass))
                {
                    try
                    {
                        Type.GetType(bindType.baseClass);
                    }
                    catch
                    {
                        EditorUtility.DisplayDialog("Information", "baseClass must use AssemblyQualifiedName", "Ok");
                        return;
                    }
                }
                
                bindType.isBuilded = true;
                if (!wrappedCustomTypeList.Contains(bindType))
                    wrappedCustomTypeList.Add(bindType);
                m_instance.GenLuaFile(bindType);
                m_instance.GenDelegate();
                m_instance.GenLuaBinder();
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "GenerateWrap success!", "Ok");
                AssetDatabase.Refresh();
            }
            public void BuildDelegate(object userData)
            {
                BindType bindType = (BindType)userData; 
                bindType.isBuilded = true;
                if (!wrappedDelegateList.Contains(bindType))
                    wrappedDelegateList.Add(bindType);
                m_instance.GenDelegate();
                m_instance.GenLuaBinder();
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "BuildDelegate success!", "Ok");
                AssetDatabase.Refresh();
            }
            public void DeleteDelegate(object userData)
            {
                BindType bindType = (BindType)userData;
                bindType.isBuilded = false;
                wrappedDelegateList.Remove(bindType);
                m_instance.GenDelegate();
                m_instance.GenLuaBinder();
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "DeleteDelegate success!", "Ok");
                AssetDatabase.Refresh();
            }
            public void DropWrap(object userData)
            {
                BindType bindType = (BindType)userData;
                bindType.drop = true;
                dropedTypeList.Add(bindType);
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "DropWrap success!", "Ok");
                AssetDatabase.Refresh();
            }
            public void UnDropWrap(object userData)
            {
                BindType bindType = (BindType)userData;
                bindType.drop = false;
                dropedTypeList.Remove(bindType);
                m_instance.Save();
                EditorUtility.DisplayDialog("Information", "DropWrap success!", "Ok");
                AssetDatabase.Refresh();
            }
            private List<BindType> Filter(List<BindType> typeArr)
            {
                List<BindType> list = new List<BindType>();
                foreach (var item in typeArr)
                {
                    //if (item.type.FullName.Contains("+")) continue;
                    //if (item.type.FullName.Contains("`") || item.type.FullName.Contains("c__")) continue;
                    if (item.type.FullName.Contains("Wrap")) continue;
                    list.Add(item);
                }
                return list;
            }
        }
        public Dictionary<string,AssemblyGroup> wrapGroupDic;
        private static string generateDir = Application.dataPath + "/lhToolkit/LuaWrapTool/MonoSource/Generate/";
        private static string delegateFactoryFilePath = Application.dataPath + "/lhToolkit/LuaWrapTool/MonoSource/DelegateFactory.cs";
        private static string luaBinderFilePath = Application.dataPath + "/lhToolkit/LuaWrapTool/MonoSource/LuaBinder.cs";
        private static string baseTypePath = Application.dataPath + "/lhToolkit/LuaWrapTool/MonoSource/BaseType";
        private static string configFilePath = Application.dataPath + "/lhToolkit/LuaWrapTool/Sources/wrap.json";
        
        private static lhAutoWrap m_instance;
        public static lhAutoWrap GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhAutoWrap();
        }
        public void Dispose()
        {
            m_instance = null;
        }
        #region public methods
        public bool CreateGroup(string newAssembly)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(newAssembly);
                var assemblyGroup = new AssemblyGroup(assembly);
                wrapGroupDic.Add(assemblyGroup.assemblyName, assemblyGroup);
                return true;
            }
            catch{
                EditorUtility.DisplayDialog("Information", "Assembly.Load(  " + newAssembly + "  ) failed","ok");
                return false;
            }
        }
        public void Settings()
        {

        }
        public void GenDelegate()
        {
            ToLuaExport.Clear();
            ToLuaExport.GenDelegates(GetAllBuildedDelegate().ToArray(), delegateFactoryFilePath);
            ToLuaExport.Clear();
        }
        public void GenLuaFile(BindType bindType)
        {
            ToLuaExport.Clear();
            ToLuaExport.className = bindType.className;
            ToLuaExport.type = bindType.type;
            ToLuaExport.isStaticClass = bindType.isStatic;
            ToLuaExport.baseType = Type.GetType(bindType.baseClass);
            ToLuaExport.wrapClassName = bindType.wrapName;
            ToLuaExport.libClassName = bindType.libName;
            ToLuaExport.customDelegateList = GetAllBuildedDelegate().ToArray();
            ToLuaExport.Generate(generateDir);
            ToLuaExport.Clear();
        }
        public void GenLuaBinder()
        {
            StringBuilder sb = new StringBuilder();
            List<DelegateType> dtList = new List<DelegateType>();

            List<BindType> allTypes = GetAllWrappedCustomType();
            List<DelegateType> delegateList = GetAllBuildedDelegate();
            ToLuaTree<string> tree = InitTree(allTypes, delegateList);
            
            List<BindType> preloadList = GetAllPreloadType();

            List<BindType> backupList = new List<BindType>();
            backupList.AddRange(allTypes);
            
            sb.AppendLine("//this source code was auto-generated by tolua#, do not modify it");
            sb.AppendLine("using System;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using LaoHan.Infrastruture.ulua;");
            sb.AppendLine();
            sb.AppendLine("public static class LuaBinder");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static void Bind(LuaState L)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tfloat t = Time.realtimeSinceStartup;");
            sb.AppendLine("\t\tL.BeginModule(null);");

            for (int i = 0; i < allTypes.Count; i++)
            {
                BindType dt = preloadList.Find((p) => { return allTypes[i].type == p.type; });

                if (dt == null && allTypes[i].nameSpace == null)
                {
                    string str = "\t\t" + allTypes[i].wrapName + ".Register(L);\r\n";
                    sb.Append(str);
                    allTypes.RemoveAt(i--);
                }
            }

            Action<ToLuaNode<string>> begin = (node) =>
            {
                if (node.value == null)
                {
                    return;
                }

                sb.AppendFormat("\t\tL.BeginModule(\"{0}\");\r\n", node.value);
                string space = GetSpaceNameFromTree(node);

                for (int i = 0; i < allTypes.Count; i++)
                {
                    BindType dt = preloadList.Find((p) => { return allTypes[i].type == p.type; });

                    if (dt == null && allTypes[i].nameSpace == space)
                    {
                        string str = "\t\t" + allTypes[i].wrapName + ".Register(L);\r\n";
                        sb.Append(str);
                        allTypes.RemoveAt(i--);
                    }
                }

                string funcName = null;

                for (int i = 0; i < delegateList.Count; i++)
                {
                    DelegateType dt = delegateList[i];
                    Type type = dt.type;
                    string typeSpace = ToLuaExport.GetNameSpace(type, out funcName);

                    if (typeSpace == space)
                    {
                        funcName = ToLuaExport.ConvertToLibSign(funcName);
                        string abr = dt.abr;
                        abr = abr == null ? funcName : abr;
                        sb.AppendFormat("\t\tL.RegFunction(\"{0}\", {1});\r\n", abr, dt.name);
                        dtList.Add(dt);
                    }
                }
            };

            Action<ToLuaNode<string>> end = (node) =>
            {
                if (node.value != null)
                {
                    sb.AppendLine("\t\tL.EndModule();");
                }
            };

            tree.DepthFirstTraversal(begin, end, tree.GetRoot());
            sb.AppendLine("\t\tL.EndModule();");

            if (preloadList.Count > 0)
            {
                sb.AppendLine("\t\tL.BeginPreLoad();");

                for (int i = 0; i < preloadList.Count; i++)
                {
                    BindType t1 = preloadList[i];
                    BindType bt = backupList.Find((p) => { return p.type == t1.type; });
                    sb.AppendFormat("\t\tL.AddPreLoad(\"{0}\", LuaOpen_{1}, typeof({0}));\r\n", bt.className, bt.wrapName);
                }
                sb.AppendLine("\t\tL.EndPreLoad();");
            }

            //sb.AppendLine("\t\tDebugger.Log(\"Register lua type cost time: {0}\", Time.realtimeSinceStartup - t);");
            sb.AppendLine("\t}");

            for (int i = 0; i < dtList.Count; i++)
            {
                ToLuaExport.GenEventFunction(dtList[i].type, sb);
            }

            if (preloadList.Count > 0)
            {
                for (int i = 0; i < preloadList.Count; i++)
                {
                    BindType t = preloadList[i];
                    BindType bt = backupList.Find((p) => { return p.type == t.type; });
                    GenPreLoadFunction(bt, sb);
                }
            }

            sb.AppendLine("}\r\n");
            allTypes.Clear();
            using (StreamWriter textWriter = new StreamWriter(luaBinderFilePath, false, Encoding.UTF8))
            {
                textWriter.Write(sb.ToString());
                textWriter.Flush();
                textWriter.Close();
            }
            
            lhDebug.Log("Generate LuaBinder over !");
        }
        public void Save()
        {
            var json = new JsonObject();
            List<BindType> wrappedList = GetAllWrappedCustomType();
            List<DelegateType> buildedDelegate = GetAllBuildedDelegate();
            List<BindType> droppedList = GetAllDropType();
            Func<BindType, JsonObject> serializeBindType = p =>
            {
                var obj = new JsonObject();
                obj["assemblyName"] = new JsonString(p.assemblyName);
                obj["nameSpace"] = new JsonString(p.nameSpace);
                obj["className"] = new JsonString(p.className);
                obj["baseClass"] = new JsonString(p.baseClass);
                obj["wrapName"] = new JsonString(p.wrapName);
                obj["libName"] = new JsonString(p.libName);
                obj["isStatic"] = new JsonNumber(p.isStatic);
                obj["preload"] = new JsonNumber(p.preload);
                obj["drop"] = new JsonNumber(p.drop);
                return obj;
            };

            var wrappedJson = new JsonArray();
            wrappedList.ForEach(p=> { wrappedJson.Add(serializeBindType(p)); });
            var delegateJson = new JsonArray();
            buildedDelegate.ForEach(p => { delegateJson.Add(serializeBindType(new BindType(p.type) { assemblyName=p.type.Assembly.FullName})); });
            var droppedJson = new JsonArray();
            droppedList.ForEach(p => { droppedJson.Add(serializeBindType(p)); });

            json["wrappedCustomTypeList"] = wrappedJson;
            json["wrappedDelegateList"] = delegateJson;
            json["dropedTypeList"] = droppedJson;

            File.WriteAllText(configFilePath, lhJson.PrettyPrint(json.ToString()));
        }
        public void SelectClear()
        {
            foreach (var item in wrapGroupDic)
            {
                item.Value.selectedList.Clear();
            }
        }
        #endregion

        #region private methods
        lhAutoWrap()
        {
            wrapGroupDic = new Dictionary<string, AssemblyGroup>();
            if (File.Exists(configFilePath))
            {
                var jsonObject = lhJson.Parse(File.ReadAllText(configFilePath)) as JsonObject;
                var jsonArray = new JsonArray();
                jsonArray.AddRange(jsonObject["wrappedCustomTypeList"].AsList());
                jsonArray.AddRange(jsonObject["wrappedDelegateList"].AsList());
                jsonArray.AddRange(jsonObject["dropedTypeList"].AsList());
                foreach (var item in jsonArray)
                {
                    var obj = item as JsonObject;
                    string assemblyName = obj["assemblyName"].AsString();
                    if (wrapGroupDic.ContainsKey(assemblyName))
                    {
                        wrapGroupDic[assemblyName].Deserialize(obj);
                    }
                    else
                    {
                        if (CreateGroup(assemblyName))
                        {
                            wrapGroupDic[assemblyName].Deserialize(obj);
                        }
                    }
                }
            }
        }
        private ToLuaTree<string> InitTree(List<BindType> list,List<DelegateType> delegateList)
        {
            ToLuaTree<string> tree = new ToLuaTree<string>();
            ToLuaNode<string> root = tree.GetRoot();

            for (int i = 0; i < list.Count; i++)
            {
                string space = list[i].nameSpace;
                AddSpaceNameToTree(tree, root, space);
            }
            
            for (int i = 0; i < delegateList.Count; i++)
            {
                string space = delegateList[i].type.Namespace;
                AddSpaceNameToTree(tree, root, space);
            }

            return tree;
        }
        private void AddSpaceNameToTree(ToLuaTree<string> tree, ToLuaNode<string> root, string space)
        {
            if (space == null || space == string.Empty)
            {
                return;
            }

            string[] ns = space.Split(new char[] { '.' });
            ToLuaNode<string> parent = root;

            for (int j = 0; j < ns.Length; j++)
            {
                //pos变量
                ToLuaNode<string> node = tree.Find((_t) => { return _t == ns[j]; }, j);

                if (node == null)
                {
                    node = new ToLuaNode<string>();
                    node.value = ns[j];
                    parent.childs.Add(node);
                    node.parent = parent;
                    //加入pos跟root里的pos比较，只有位置相同才是统一命名空间节点
                    node.pos = j;
                    parent = node;
                }
                else
                {
                    parent = node;
                }
            }
        }
        private string GetSpaceNameFromTree(ToLuaNode<string> node)
        {
            string name = node.value;

            while (node.parent != null && node.parent.value != null)
            {
                node = node.parent;
                name = node.value + "." + name;
            }

            return name;
        }
        private void GenPreLoadFunction(BindType bt, StringBuilder sb)
        {
            string funcName = "LuaOpen_" + bt.wrapName;

            sb.AppendLine("\r\n\t[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]");
            sb.AppendFormat("\tstatic int {0}(IntPtr L)\r\n", funcName);
            sb.AppendLine("\t{");
            sb.AppendLine("\t\ttry");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tint top = LuaDLL.lua_gettop(L);");
            sb.AppendLine("\t\t\tLuaState state = LuaState.Get(L);");
            sb.AppendFormat("\t\t\tint preTop = state.BeginPreModule(\"{0}\");\r\n", bt.nameSpace);
            sb.AppendFormat("\t\t\t{0}.Register(state);\r\n", bt.wrapName);
            sb.AppendLine("\t\t\tstate.EndPreModule(preTop);");
            sb.AppendLine("\t\t\tLuaDLL.lua_settop(L, top);");
            sb.AppendLine("\t\t\treturn 0;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t\tcatch(Exception e)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn LuaDLL.toluaL_exception(L, e);");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
        }
        private List<Type> GetCustomTypeDelegates(BindType bindType)
        {
            List<Type> set = new List<Type>();
            BindingFlags binding = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.Instance;

            Type type = bindType.type;
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.SetField | binding);
            PropertyInfo[] props = type.GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | binding);
            MethodInfo[] methods = null;

            if (type.IsInterface)
            {
                methods = type.GetMethods();
            }
            else
            {
                methods = type.GetMethods(BindingFlags.Instance | binding);
            }

            for (int j = 0; j < fields.Length; j++)
            {
                Type t = fields[j].FieldType;

                if (typeof(System.Delegate).IsAssignableFrom(t))
                {
                    if (!set.Contains(t))
                        set.Add(t);
                }
            }

            for (int j = 0; j < props.Length; j++)
            {
                Type t = props[j].PropertyType;

                if (typeof(System.Delegate).IsAssignableFrom(t))
                {
                    if (!set.Contains(t))
                        set.Add(t);
                }
            }

            for (int j = 0; j < methods.Length; j++)
            {
                MethodInfo m = methods[j];

                if (m.IsGenericMethod)
                {
                    continue;
                }

                ParameterInfo[] pifs = m.GetParameters();

                for (int k = 0; k < pifs.Length; k++)
                {
                    Type t = pifs[k].ParameterType;

                    if (typeof(System.MulticastDelegate).IsAssignableFrom(t))
                    {
                        if (!set.Contains(t))
                            set.Add(t);
                    }
                }
            }

            return set;
        }
        private List<DelegateType> GetAllBuildedDelegate()
        {
            List<Type> list = new List<Type>();
            List<Type> noneGroupList = new List<Type>();
            foreach (var item in wrapGroupDic)
            {
                foreach (var type in item.Value.wrappedCustomTypeList)
                {
                    var customDelegateList = GetCustomTypeDelegates(type);
                    foreach (var custom in customDelegateList)
                    {
                        if (!list.Contains(custom))
                        {
                            list.Add(custom);
                        }
                    }
                }
            }
            foreach (var item in list)
            {
                string assemblyName = item.Assembly.FullName;
                if (!wrapGroupDic.ContainsKey(assemblyName))
                    CreateGroup(assemblyName);
                var target = wrapGroupDic[assemblyName].allTypeList.Find(p => {
                    return p.type == item;
                });
                if (target==null)
                {
                    //Debug.LogError("LaoHan: < " + assemblyName + " > dont has this type:" + item);
                    noneGroupList.Add(item);
                }
                else
                {
                    if (!wrapGroupDic[assemblyName].wrappedDelegateList.Contains(target))
                    {
                        target.isBuilded = true;
                        wrapGroupDic[assemblyName].wrappedDelegateList.Add(target);
                    }
                }
            }

            List<DelegateType> typelist = new List<DelegateType>();
            foreach (var item in wrapGroupDic)
            {
                foreach (var del in item.Value.wrappedDelegateList)
                {
                    typelist.Add(new DelegateType(del.type));
                }
            }
            foreach (var item in noneGroupList)
            {
                typelist.Add(new DelegateType(item));
            }
            return typelist;
        }
        private List<BindType> GetAllWrappedCustomType()
        {
            List<BindType> list = new List<BindType>();
            foreach (var item in wrapGroupDic)
            {
                list.AddRange(item.Value.wrappedCustomTypeList);
            }
            return list;
        }
        private List<BindType> GetAllPreloadType()
        {
            List<BindType> preloadList = new List<BindType>();
            foreach (var item in wrapGroupDic)
            {
                var typeList = item.Value.wrappedCustomTypeList.FindAll(p => { return p.preload == true; });
                if (typeList != null)
                {
                    foreach (var type in typeList)
                    {
                        preloadList.Add(type);
                    }
                }
            }
            return preloadList;
        }
        private List<BindType> GetAllDropType()
        {
            List<BindType> dropList = new List<BindType>();
            foreach (var item in wrapGroupDic)
            {
                dropList.AddRange(item.Value.dropedTypeList);
            }
            return dropList;
        }
        #endregion
    }
}