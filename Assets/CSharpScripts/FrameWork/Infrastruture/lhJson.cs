using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace LaoHan.Infrastruture
{
    public enum EJsonType
    {
        Number,
        String,
        Array,
        Object,
    }
    public interface IJsonNode
    {
        EJsonType type
        {
            get;

        }
        void ConvertToString(StringBuilder sb);

        void ConvertToStringPhp(StringBuilder sb);

        void ConvertToStringWithFormat(StringBuilder sb, int spacesub);

        void ConvertToStringPhpWithFormat(StringBuilder sb, int spacesub);

        void Scan(lhJson.ScanObj scan);

        IJsonNode Get(string path);

        //增加大量快速访问方法
        IJsonNode GetArrayItem(int index);
        IJsonNode GetDictItem(string key);

        void AddArrayValue(IJsonNode node);
        void AddArrayValue(double value);
        void AddArrayValue(bool value);
        void AddArrayValue(string value);

        void SetArrayValue(int index, IJsonNode node);
        void SetArrayValue(int index, double value);
        void SetArrayValue(int index, bool value);
        void SetArrayValue(int index, string value);

        void SetDictValue(string key, IJsonNode node);
        void SetDictValue(string key, double value);
        void SetDictValue(string key, bool value);
        void SetDictValue(string key, string value);

        void SetValue(double value);
        void SetValue(string value);
        void SetValue(bool value);

        double AsDouble();
        int AsInt();
        bool AsBool();

        bool IsNull();
        String AsString();
        IList<IJsonNode> AsList();
        IDictionary<string, IJsonNode> AsDict();

        bool HaveDictItem(string key);

        int GetListCount();
    }
    public class JsonNumber : IJsonNode
    {
        public JsonNumber()
        {

        }
        public JsonNumber(double value)
        {
            this.value = value;
            this.isBool = false;
        }
        public JsonNumber(bool value)
        {
            this.value = value ? 1 : 0;
            this.isBool = true;
        }
        public double value
        {
            get;
            set;
        }
        public bool isBool
        {
            get;
            private set;
        }
        public bool isNull
        {
            get;
            private set;
        }
        public void SetNull()
        {
            this.isNull = true;
            this.isBool = false;
        }
        public void SetBool(bool v)
        {
            this.value = v ? 1 : 0;
            this.isBool = true;
        }
        public override string ToString()
        {
            if (isBool)
            {
                return ((bool)this) ? "true" : "false";
            }
            else if (isNull)
            {
                return "null";
            }
            else
            {
                return value.ToString();
            }
        }

        public EJsonType type
        {
            get
            {
                return EJsonType.Number;
            }
        }
        public void ConvertToString(StringBuilder sb)
        {
            sb.Append(ToString());
        }
        public void ConvertToStringWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int i = 0; i < space; i++)
            //    sb.Append(' ');
            ConvertToString(sb);
        }
        public void ConvertToStringPhp(StringBuilder sb)
        {

            sb.Append(ToString());
        }
        public void ConvertToStringPhpWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int i = 0; i < space; i++)
            //    sb.Append(' ');
            ConvertToStringPhp(sb);
        }
        public void Scan(lhJson.ScanObj scan)
        {
            string number = "";
            for (int i = scan.seed; i < scan.json.Length; i++)
            {
                char c = scan.json[i];
                if (c != ',' && c != ']' && c != '}' && c != ' ')
                {
                    if (c != '\n')
                        number += c;
                }
                else
                {
                    scan.seed = i;
                    break;
                }
            }

            if (number.ToLower().TrimEnd('\t').TrimStart('\t') == "true")
            {
                value = 1;
                isBool = true;
            }
            else if (number.ToLower().TrimEnd('\t').TrimStart('\t') == "false")
            {
                value = 0;
                isBool = true;
            }
            else if (number.ToLower().TrimEnd('\t').TrimStart('\t') == "null")
            {
                value = 0;
                isNull = true;
            }
            else
            {
                value = double.Parse(number);
                isBool = false;
            }
        }
        public static implicit operator double (JsonNumber m)
        {
            return m.value;
        }
        public static implicit operator float (JsonNumber m)
        {
            return (float)m.value;
        }
        public static implicit operator int (JsonNumber m)
        {
            return (int)m.value;
        }
        public static implicit operator uint (JsonNumber m)
        {
            return (uint)m.value;
        }

        public static implicit operator bool (JsonNumber m)
        {
            return (uint)m.value != 0;
        }


        public IJsonNode Get(string path)
        {
            if (string.IsNullOrEmpty(path)) return this;

            return null;
        }

        public IJsonNode GetArrayItem(int index)
        {
            throw new NotImplementedException();
        }

        public IJsonNode GetDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(double value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(bool value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, double value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, string value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, double value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(double value)
        {
            this.value = value;
            this.isBool = false;
            this.isNull = false;
        }

        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(bool value)
        {
            this.value = value ? 1 : 0;
            this.isBool = true;
            this.isNull = false;
        }

        public double AsDouble()
        {
            if (!this.isNull && !this.isBool)
                return this.value;
            throw new Exception("Value type 不同");
        }

        public int AsInt()
        {
            if (!this.isNull && !this.isBool)
                return (int)this.value;
            throw new Exception("Value type 不同");
        }

        public bool AsBool()
        {
            if (this.isBool)
            {
                return (uint)value != 0;
            }
            throw new Exception("Value type 不同");
        }

        public bool IsNull()
        {
            return isNull;
        }

        public string AsString()
        {
            if (!this.isNull && !this.isBool)
                return this.value.ToString();
            throw new NotImplementedException();
        }

        public IList<IJsonNode> AsList()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IJsonNode> AsDict()
        {
            throw new NotImplementedException();
        }


        public bool HaveDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public int GetListCount()
        {
            throw new NotImplementedException();
        }
    }
    public class JsonString : IJsonNode
    {
        public JsonString()
        {

        }
        public JsonString(string value)
        {
            this.value = value;
        }
        public string value
        {
            get;
            set;
        }
        public override string ToString()
        {
            return value;
        }

        public EJsonType type
        {
            get
            {
                return EJsonType.String;
            }
        }
        public void ConvertToString(StringBuilder sb)
        {
            sb.Append('\"');
            if (value != null)
            {
                string v = value.Replace("\\", "\\\\");
                v = v.Replace("\"", "\\\"");
                sb.Append(v);
            }
            sb.Append('\"');
        }
        public void ConvertToStringWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int i = 0; i < space; i++)
            //    sb.Append(' ');
            ConvertToString(sb);
        }
        public void ConvertToStringPhp(StringBuilder sb)
        {
            sb.Append('\"');
            if (value != null)
            {
                string v = value.Replace("\\", "\\\\");
                v = v.Replace("\"", "\\\"");
                sb.Append(v);
            }
            sb.Append('\"');
        }
        public void ConvertToStringPhpWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int i = 0; i < space; i++)
            //    sb.Append(' ');
            ConvertToStringPhp(sb);
        }
        public void Scan(lhJson.ScanObj scan)
        {
            string _value = "";
            for (int i = scan.seed + 1; i < scan.json.Length; i++)
            {
                char c = scan.json[i];
                if (c == '\\')
                {
                    i++;
                    c = scan.json[i];
                    _value += c;
                }

                else if (c != '\"')
                {

                    _value += c;
                }

                else
                {
                    scan.seed = i + 1;
                    break;
                }
            }
            value = _value;
        }

        public static implicit operator string (JsonString m)
        {
            return m.value;
        }



        public IJsonNode Get(string path)
        {
            if (string.IsNullOrEmpty(path)) return this;

            return null;
        }


        public IJsonNode GetArrayItem(int index)
        {
            throw new NotImplementedException();
        }

        public IJsonNode GetDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(double value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(bool value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, double value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, string value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, double value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(double value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string value)
        {
            this.value = value;
        }

        public void SetValue(bool value)
        {
            throw new NotImplementedException();
        }

        public double AsDouble()
        {
            throw new NotImplementedException();
        }

        public int AsInt()
        {
            throw new NotImplementedException();
        }

        public bool AsBool()
        {
            throw new NotImplementedException();
        }

        public bool IsNull()
        {
            return false;
        }

        public string AsString()
        {
            return value;
        }

        public IList<IJsonNode> AsList()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IJsonNode> AsDict()
        {
            throw new NotImplementedException();
        }


        public bool HaveDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public int GetListCount()
        {
            throw new NotImplementedException();
        }
    }
    public class JsonArray : List<IJsonNode>, IJsonNode
    {
        public EJsonType type
        {
            get { return EJsonType.Array; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ConvertToString(sb);
            return sb.ToString();
        }
        public void ConvertToString(StringBuilder sb)
        {
            sb.Append('[');
            for (int i = 0; i < this.Count; i++)
            {
                this[i].ConvertToString(sb);
                if (i != this.Count - 1)
                    sb.Append(',');
            }
            sb.Append(']');
        }
        public void ConvertToStringWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            sb.Append("[\n");
            for (int i = 0; i < this.Count; i++)
            {
                for (int _i = 0; _i < spacesub; _i++)
                    sb.Append(' ');

                this[i].ConvertToStringWithFormat(sb, spacesub + 4);
                if (i != this.Count - 1)
                    sb.Append(',');
                sb.Append('\n');
            }
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            for (int _i = 0; _i < spacesub; _i++)
                sb.Append(' ');
            sb.Append(']');
        }
        public void ConvertToStringPhp(StringBuilder sb)
        {
            sb.Append("Array(");
            for (int i = 0; i < this.Count; i++)
            {
                this[i].ConvertToStringPhp(sb);
                if (i != this.Count - 1)
                    sb.Append(',');
            }
            sb.Append(')');
        }
        public void ConvertToStringPhpWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            sb.Append("Array(\n");
            for (int i = 0; i < this.Count; i++)
            {
                for (int _i = 0; _i < spacesub; _i++)
                    sb.Append(' ');

                this[i].ConvertToStringPhpWithFormat(sb, spacesub + 4);
                if (i != this.Count - 1)
                    sb.Append(',');
                sb.Append('\n');
            }
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            for (int _i = 0; _i < spacesub; _i++)
                sb.Append(' ');
            sb.Append(')');
        }
        public void Scan(lhJson.ScanObj scan)
        {
            for (int i = scan.seed + 1; i < scan.json.Length; i++)
            {
                char c = scan.json[i];
                if (c == ',')
                    continue;
                if (c == ']')
                {
                    scan.seed = i + 1;
                    break;
                }
                IJsonNode node = lhJson.ScanFirst(c);
                if (node != null)
                {
                    scan.seed = i;
                    node.Scan(scan);
                    i = scan.seed - 1;
                    this.Add(node);
                }

            }
        }

        public int GetFirstKey02(string path, int start, out string nextpath)
        {
            int _path = -1;
            for (int i = start + 1; i < path.Length; i++)
            {
                if (path[i] == '[')
                {
                    _path = GetFirstKey02(path, i, out nextpath);
                }
                if (path[i] == ']')
                {
                    nextpath = path.Substring(i + 1);
                    if (_path == -1)
                    {
                        _path = int.Parse(path.Substring(start + 1, i - start - 1));
                    }
                    return _path;
                }
            }
            nextpath = null;
            return -1;
        }

        public int GetFirstKey(string path, out string nextpath)
        {
            nextpath = null;
            int istart = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '.' || path[i] == ' ')
                {
                    istart++;
                    continue;
                }
                if (path[i] == '[')
                {
                    return GetFirstKey02(path, i, out nextpath);
                }

            }

            return -1;


        }


        public IJsonNode Get(string path)
        {
            if (path.Length == 0) return this;
            string nextpath;
            int key = GetFirstKey(path, out nextpath);
            if (key >= 0 && key < this.Count)
            {
                return this[key];
            }
            else
            {
                return null;
            }
        }

        public IJsonNode GetArrayItem(int index)
        {
            return this[index];
        }

        public IJsonNode GetDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(IJsonNode node)
        {
            this.Add(node);
        }

        public void AddArrayValue(double value)
        {
            this.Add(new JsonNumber(value));
        }

        public void AddArrayValue(bool value)
        {
            this.Add(new JsonNumber(value));
        }

        public void AddArrayValue(string value)
        {
            this.Add(new JsonString(value));
        }

        public void SetArrayValue(int index, IJsonNode node)
        {
            this[index] = node;
        }

        public void SetArrayValue(int index, double value)
        {
            this[index] = new JsonNumber(value);
        }

        public void SetArrayValue(int index, bool value)
        {
            this[index] = new JsonNumber(value);
        }

        public void SetArrayValue(int index, string value)
        {
            this[index] = new JsonString(value);
        }

        public void SetDictValue(string key, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, double value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(double value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(bool value)
        {
            throw new NotImplementedException();
        }

        public double AsDouble()
        {
            throw new NotImplementedException();
        }

        public int AsInt()
        {
            throw new NotImplementedException();
        }

        public bool AsBool()
        {
            throw new NotImplementedException();
        }

        public bool IsNull()
        {
            return false;
        }

        public string AsString()
        {
            throw new NotImplementedException();
        }

        public IList<IJsonNode> AsList()
        {
            return this;
        }

        public IDictionary<string, IJsonNode> AsDict()
        {
            throw new NotImplementedException();
        }


        public bool HaveDictItem(string key)
        {
            throw new NotImplementedException();
        }

        public int GetListCount()
        {
            return this.Count;
        }
    }
    public class JsonObject : Dictionary<string, IJsonNode>, IJsonNode
    {
        public EJsonType type
        {
            get { return EJsonType.Object; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            ConvertToString(sb);
            return sb.ToString();
        }
        public void ConvertToString(StringBuilder sb)
        {
            sb.Append('{');
            int i = Count;
            foreach (var item in this)
            {
                sb.Append('\"');
                sb.Append(item.Key);
                sb.Append("\":");
                item.Value.ConvertToString(sb);
                i--;
                if (i != 0) sb.Append(',');
            }
            sb.Append('}');
        }
        public void ConvertToStringWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            sb.Append("{\n");
            int i = Count;
            foreach (var item in this)
            {
                for (int _i = 0; _i < spacesub; _i++)
                    sb.Append(' ');

                sb.Append('\"');
                sb.Append(item.Key);
                sb.Append("\":");
                item.Value.ConvertToStringWithFormat(sb, spacesub + 4);
                i--;
                if (i != 0) sb.Append(',');
                sb.Append('\n');
            }
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            for (int _i = 0; _i < spacesub; _i++)
                sb.Append(' ');
            sb.Append('}');
        }
        public void ConvertToStringPhp(StringBuilder sb)
        {
            sb.Append("Array(");
            int i = Count;
            foreach (var item in this)
            {
                sb.Append('\"');
                sb.Append(item.Key);
                sb.Append("\"=>");
                item.Value.ConvertToStringPhp(sb);
                i--;
                if (i != 0) sb.Append(',');
            }
            sb.Append(')');
        }
        public void ConvertToStringPhpWithFormat(StringBuilder sb, int spacesub)
        {
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            sb.Append("Array(\n");
            int i = Count;
            foreach (var item in this)
            {
                for (int _i = 0; _i < spacesub; _i++)
                    sb.Append(' ');

                sb.Append('\"');
                sb.Append(item.Key);
                sb.Append("\"=>");
                item.Value.ConvertToStringPhpWithFormat(sb, spacesub + 4);
                i--;
                if (i != 0) sb.Append(',');
                sb.Append('\n');
            }
            //for (int _i = 0; _i < space; _i++)
            //    sb.Append(' ');
            for (int _i = 0; _i < spacesub; _i++)
                sb.Append(' ');
            sb.Append(')');
        }
        //public IJsonNode  this[string key]
        //{
        //    get
        //    {
        //        if (this.ContainsKey(key))
        //        {
        //            return base[key];
        //        }

        //        throw new Exception("key not exist");

        //    }
        //    set
        //    {
        //        if (value == null)
        //        {

        //            throw new Exception("value is null. key:"+key);
        //        }
        //        base[key] = value;
        //    }
        //}

        public void Scan(lhJson.ScanObj scan)
        {
            string key = null;
            int keystate = 0;//0 nokey 1scankey 2gotkey
            for (int i = scan.seed + 1; i < scan.json.Length; i++)
            {
                char c = scan.json[i];
                if (keystate != 1 && (c == ',' || c == ':'))
                    continue;
                if (c == '}')
                {
                    scan.seed = i + 1;
                    break;
                }
                if (keystate == 0)
                {
                    if (c == '\"')
                    {
                        keystate = 1;
                        key = "";
                    }
                }
                else if (keystate == 1)
                {
                    if (c == '\"')
                    {
                        keystate = 2;
                        //scan.seed = i + 1;
                        continue;
                    }
                    else
                    {
                        key += c;
                    }
                }
                else
                {
                    IJsonNode node = lhJson.ScanFirst(c);
                    if (node != null)
                    {
                        scan.seed = i;
                        node.Scan(scan);
                        i = scan.seed - 1;
                        this.Add(key, node);
                        keystate = 0;
                    }
                }

            }
        }
        public string GetFirstKey01(string path, int start, out string nextpath)
        {
            for (int i = start + 1; i < path.Length; i++)
            {
                if (path[i] == '\\') continue;
                if (path[i] == '\"')
                {
                    nextpath = path.Substring(i + 1);
                    var _path = path.Substring(start + 1, i - start - 1);
                    return _path;
                }
            }
            nextpath = null;
            return null;
        }
        public string GetFirstKey02(string path, int start, out string nextpath)
        {
            string _path = null;
            for (int i = start + 1; i < path.Length; i++)
            {
                if (path[i] == '[')
                {
                    _path = GetFirstKey02(path, i, out nextpath);
                }
                if (path[i] == '\"')
                {
                    _path = GetFirstKey01(path, i, out nextpath);
                    i += _path.Length + 2;
                }
                if (path[i] == ']')
                {
                    nextpath = path.Substring(i + 1);
                    if (_path == null)
                    {
                        _path = path.Substring(start + 1, i - start - 1);
                    }
                    return _path;
                }
            }
            nextpath = null;
            return null;
        }
        public string GetFirstKey(string path, out string nextpath)
        {
            nextpath = null;
            int istart = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '.' || path[i] == ' ')
                {
                    istart++;
                    continue;
                }
                if (path[i] == '[')
                {
                    return GetFirstKey02(path, i, out nextpath);
                }
                else if (path[i] == '\"')
                {
                    return GetFirstKey01(path, i, out nextpath);
                }
                else
                {

                    int iend1 = path.IndexOf('[', i + 1);
                    if (iend1 == -1) iend1 = path.Length;
                    int iend2 = path.IndexOf('.', i + 1);
                    if (iend2 == -1) iend2 = path.Length;
                    int iss = Math.Min(iend1, iend2);

                    var _path = path.Substring(istart, iss - istart);
                    nextpath = path.Substring(iss);
                    return _path;
                }

            }

            return null;


        }
        public IJsonNode Get(string path)
        {
            if (path.Length == 0) return this;
            string nextpath;
            string key = GetFirstKey(path, out nextpath);
            if (this.ContainsKey(key))
            {
                return this[key].Get(nextpath);
            }
            else
            {
                return null;
            }

        }

        public IJsonNode GetArrayItem(int index)
        {
            throw new NotImplementedException();
        }

        public IJsonNode GetDictItem(string key)
        {
            return this[key];
        }

        public void AddArrayValue(IJsonNode node)
        {
            if (node.type == EJsonType.Object)
            {
                try
                {
                    var dic = node as IDictionary<string, IJsonNode>;
                    foreach (var item in dic)
                    {
                        if (this.ContainsKey(item.Key))
                            Remove(item.Key);
                        Add(item.Key, item.Value);
                    }
                }
                catch
                {
                    throw new NotImplementedException();
                }
            }
            else
                throw new NotImplementedException();
        }

        public void AddArrayValue(double value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(bool value)
        {
            throw new NotImplementedException();
        }

        public void AddArrayValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, IJsonNode node)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, double value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetArrayValue(int index, string value)
        {
            throw new NotImplementedException();
        }

        public void SetDictValue(string key, IJsonNode node)
        {
            this[key] = node;
        }

        public void SetDictValue(string key, double value)
        {
            this[key] = new JsonNumber(value);
        }

        public void SetDictValue(string key, bool value)
        {
            this[key] = new JsonNumber(value);
        }

        public void SetDictValue(string key, string value)
        {
            this[key] = new JsonString(value);
        }

        public void SetValue(double value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        public void SetValue(bool value)
        {
            throw new NotImplementedException();
        }

        public double AsDouble()
        {
            throw new NotImplementedException();
        }

        public int AsInt()
        {
            throw new NotImplementedException();
        }

        public bool AsBool()
        {
            throw new NotImplementedException();
        }

        public bool IsNull()
        {
            return false;
        }

        public string AsString()
        {
            throw new NotImplementedException();
        }

        public IList<IJsonNode> AsList()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IJsonNode> AsDict()
        {
            return this;
        }


        public bool HaveDictItem(string key)
        {
            return ContainsKey(key);
        }

        public int GetListCount()
        {
            throw new NotImplementedException();
        }
    }
    public class lhJson
    {
        #region Class
        public class ScanObj
        {
            public string json;
            public int seed;
        }
        #endregion

        #region members
        private const string m_space = " ";
        private const int m_defaultIndent = 0;
        private const string m_indent = m_space + m_space + m_space + m_space;
        private const string m_newLine = "\n";

        private static bool m_inDoubleString = false;
        private static bool m_inSingleString = false;
        private static bool m_inVariableAssignment = false;
        private static char m_prevChar = '\0';
        private static Stack<JsonContextType> m_context = new Stack<JsonContextType>();

        private enum JsonContextType
        {
            Object, Array
        }

        #endregion

        #region interface
        #endregion

        #region enum
        #endregion

        #region public static
        public static object Parse(Type type,string json)
        {
            if (type.IsGenericType)
                return ToGeneric(type, Parse(json));
            else
                return ToObject(type, Parse(json).AsDict());
        }
        public static IJsonNode Parse(string json)
        {
            try
            {
                ScanObj obj = new ScanObj();
                obj.json = json;
                obj.seed = 0;
                IJsonNode node = Scan(obj);
                return node;
            }
            catch (Exception err)
            {
                throw new Exception("parse err:" + json, err);
            }
        }
        public static void WriteStrDict(System.IO.Stream stream, IList<string> dict)
        {
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            WriteUIntSingle(stream, dict.Count);
            foreach (var d in dict)
            {
                var strdata = System.Text.Encoding.UTF8.GetBytes(d);
                WriteUIntSingle(stream, strdata.Length);
                stream.Write(strdata, 0, strdata.Length);
            }
            //return ms.ToArray();
        }
        public static List<string> ReadStrDict(System.IO.Stream stream)
        {
            List<string> list = new List<string>();
            int c = ReadIntSingle(stream);
            for (int i = 0; i < c; i++)
            {
                int slen = ReadIntSingle(stream);
                byte[] buf = new byte[slen];
                stream.Read(buf, 0, slen);
                string str = System.Text.Encoding.UTF8.GetString(buf, 0, buf.Length);
                list.Add(str);
            }
            return list;
        }
        /// <summary>
        /// 从json写入二进制流
        /// </summary>
        /// <param name="stream">二进制流</param>
        /// <param name="node">json</param>
        /// <param name="pubdict">一个字符串字典(可选)如果字典里有的字符串，保存只记录一个索引，但是需要字典才能读出来</param>
        /// <param name="riseDictByKey">是否把key添加到字典中（默认为false）</param>
        /// <param name="riseDictByString">是否把String值添加到字典中（默认为false）</param>
        public static void Write(System.IO.Stream stream, IJsonNode node, IList<string> pubdict = null, bool riseDictByKey = false, bool riseDictByString = false)
        {
            List<string> localdict = new List<string>();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            PackJson(ms, node, pubdict, localdict, riseDictByKey, riseDictByString);
            byte[] data = ms.ToArray();
            ms.Close();
            WriteStrDict(stream, localdict);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
        /// <summary>
        /// 从二进制流读取Json
        /// </summary>
        /// <param name="stream">二进制流</param>
        /// <param name="pubdict">一个字符串字典(可选),如果保存的时候用了字典，解压就得用那个字典</param>
        /// <returns></returns>
        public static IJsonNode Read(System.IO.Stream stream, IList<string> pubdict = null)
        {
            var list = ReadStrDict(stream);
            return UnPackJson(stream, pubdict, list);
        }
        public static string PrettyPrint(string input)
        {
            // Clear all states
            m_inDoubleString = false;
            m_inSingleString = false;
            m_inVariableAssignment = false;
            m_prevChar = '\0';
            m_context.Clear();

            var output = new StringBuilder(input.Length * 2);
            char c;

            for (int i = 0; i < input.Length; i++)
            {
                c = input[i];

                switch (c)
                {
                    case '[':
                    case '{':
                        if (!InString())
                        {
                            if (m_inVariableAssignment || (m_context.Count > 0 && m_context.Peek() != JsonContextType.Array))
                            {
                                output.Append(m_newLine);
                                BuildIndents(m_context.Count, output);
                            }
                            output.Append(c);
                            m_context.Push(JsonContextType.Object);
                            output.Append(m_newLine);
                            BuildIndents(m_context.Count, output);
                        }
                        else
                            output.Append(c);

                        break;

                    case ']':
                    case '}':
                        if (!InString())
                        {
                            output.Append(m_newLine);
                            m_context.Pop();
                            BuildIndents(m_context.Count, output);
                            output.Append(c);
                        }
                        else
                            output.Append(c);

                        break;
                    case '=':
                        output.Append(c);
                        break;

                    case ',':
                        output.Append(c);

                        if (!InString())
                        {
                            BuildIndents(m_context.Count, output);
                            output.Append(m_newLine);
                            BuildIndents(m_context.Count, output);
                            m_inVariableAssignment = false;
                        }

                        break;

                    case '\'':
                        if (!m_inDoubleString && m_prevChar != '\\')
                            m_inSingleString = !m_inSingleString;

                        output.Append(c);
                        break;

                    case ':':
                        if (!InString())
                        {
                            m_inVariableAssignment = true;
                            output.Append(m_space);
                            output.Append(c);
                            output.Append(m_space);
                        }
                        else
                            output.Append(c);

                        break;

                    case '"':
                        if (!m_inSingleString && m_prevChar != '\\')
                            m_inDoubleString = !m_inDoubleString;

                        output.Append(c);
                        break;
                    case ' ':
                        if (InString())
                            output.Append(c);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
                m_prevChar = c;
            }

            return output.ToString();
        }
        #endregion

        #region private static methods
        static int GetFreeKey(IList<string> dict)
        {
            return dict.Count;
        }
        static int GetKey(IList<string> dict, string value)
        {
            if (dict == null) return -1;
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i] == value)
                    return i;
            }
            return -1;

        }
        static byte MakeStringTag(bool inDict, bool isPubDict, int keylength)
        {
            byte tag = 128 | 64;//stringtag
            if (inDict)
                tag |= 32;
            if (isPubDict)
                tag |= 16;

            tag |= (byte)(keylength);

            return tag;
        }
        static byte MakeNumberTag(bool isFloat, bool isBool, bool isNull, bool isNeg, int datalength)
        {
            byte tag = 128 | 0;//numbertag
            if (isFloat)
                tag |= 32;
            if (isBool)
                tag |= 16;
            if (isNull)
                tag |= 8;
            if (isNeg)
                tag |= 4;
            if (isFloat)
                tag |= (byte)(4 - 1);
            else if (!isBool && !isNull)
                tag |= (byte)(datalength - 1);
            return tag;
        }
        static byte MakeArrayTag(int arraycount, int bytelen)
        {
            byte tag = 0 | 0;//arraytag
            if (arraycount < 32)
            {
                tag |= 32;
                tag |= (byte)arraycount;
            }
            else
            {
                tag |= (byte)(bytelen - 1);
            }
            return tag;
        }
        static byte MakeObjectTag(int arraycount, int bytelen)
        {
            byte tag = 0 | 64;//objecttag
            if (arraycount < 32)
            {
                tag |= 32;
                tag |= (byte)arraycount;
            }
            else
            {
                tag |= (byte)(bytelen - 1);
            }
            return tag;
        }

        static void WriteStringDataDirect(System.IO.Stream stream, string str)
        {
            byte[] sdata = System.Text.Encoding.UTF8.GetBytes(str);
            byte tag = MakeStringTag(false, false, sdata.Length);
            stream.WriteByte(tag);
            stream.Write(sdata, 0, sdata.Length);
        }
        static void WriteStringDataDict(System.IO.Stream stream, bool isPubDict, int pid)
        {
            int bytelen = 1;
            int c = pid;
            while (c >= 0x100)
            {
                c /= 0x100;
                bytelen++;
            }
            byte tag = MakeStringTag(true, isPubDict, bytelen);
            stream.WriteByte(tag);
            byte[] buf = new byte[8];
            buf = BitConverter.GetBytes(pid);
            stream.Write(buf, 0, bytelen);
        }

        static string ReadString(System.IO.Stream stream, byte tagfirst, IList<string> pubdict, IList<string> localdict)
        {
            bool inDict = (tagfirst & 32) > 0;
            bool isPubDict = (tagfirst & 16) > 0;
            int keylength = tagfirst % 16;
            if (inDict)
            {
                byte[] buf = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    buf[i] = 0;
                }
                stream.Read(buf, 0, keylength);
                int id = BitConverter.ToInt32(buf, 0);
                if (isPubDict)
                {
                    return pubdict[id];
                }
                else
                {
                    return localdict[id];
                }
            }
            else
            {
                byte[] buf = new byte[keylength];
                stream.Read(buf, 0, keylength);
                return System.Text.Encoding.UTF8.GetString(buf, 0, buf.Length);
            }
        }
        static void WriteFloatData(System.IO.Stream stream, float number)
        {
            stream.WriteByte(MakeNumberTag(true, false, false, false, 4));
            byte[] buf = BitConverter.GetBytes(number);
            stream.Write(buf, 0, 4);
        }
        static void WriteIntData(System.IO.Stream stream, int number)
        {
            int bytelen = 1;
            int sc = number;
            if (number < 0)
                sc *= -1;
            int c = sc;
            while (c >= 0x100)
            {
                c /= 0x100;
                bytelen++;
            }
            stream.WriteByte(MakeNumberTag(false, false, false, (number < 0), bytelen));
            byte[] buf = BitConverter.GetBytes(sc);
            stream.Write(buf, 0, bytelen);
        }


        static void WriteUIntSingle(System.IO.Stream stream, int number)
        {
            int bytelen = 1;
            int c = number;
            while (c >= 0x100)
            {
                c /= 0x100;
                bytelen++;
            }
            if (number < 128)
            {
                stream.WriteByte((byte)number);
            }
            else if (number < 31 * 256)
            {
                int high = number / 256;
                int low = number % 256;
                stream.WriteByte((byte)(128 | (byte)high));
                stream.WriteByte((byte)low);
            }
            else if (number < 15 * 256 * 256)
            {
                int high = number / 256 / 256;
                int midle = (number / 256) % 256;
                int low = (number % 256);

                stream.WriteByte((byte)(128 | 64 | (byte)high));
                stream.WriteByte((byte)midle);
                stream.WriteByte((byte)low);

            }
        }

        static int ReadIntSingle(System.IO.Stream stream)
        {
            byte t = (byte)stream.ReadByte();
            if ((t & 128) > 0)
            {
                if ((t & 64) > 0)
                {
                    if ((t & 32) == 0)
                    {
                        byte h = (byte)(t % 32);
                        byte m = (byte)stream.ReadByte();
                        byte l = (byte)stream.ReadByte();
                        return h * 256 * 256 + m * 256 + l;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                else
                {
                    byte low = (byte)stream.ReadByte();
                    return t % 64 * 256 + low;
                }
            }
            else
            {
                return t;
            }
        }
        static void WriteArrayCountHead(System.IO.Stream stream, int arraycount)
        {
            int bytelen = 1;
            int c = arraycount;
            while (c >= 0x100)
            {
                c /= 0x100;
                bytelen++;
            }
            stream.WriteByte(MakeArrayTag(arraycount, bytelen));
            if (arraycount >= 32)
            {
                byte[] buf = BitConverter.GetBytes(arraycount);
                stream.Write(buf, 0, bytelen);
            }
        }
        static int ReadCountHead(System.IO.Stream stream, byte tagfirst)
        {
            bool b32 = (tagfirst & 32) > 0;
            if (!b32)
            {
                int blen = tagfirst % 32 + 1;
                byte[] buf = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    buf[i] = 0;
                }
                stream.Read(buf, 0, blen);
                return BitConverter.ToInt32(buf, 0);
            }
            else
            {
                return tagfirst % 32;
            }
        }
        static void WriteObjectCountHead(System.IO.Stream stream, int arraycount)
        {
            int bytelen = 1;
            int c = arraycount;
            while (c >= 0x100)
            {
                c /= 0x100;
                bytelen++;
            }
            stream.WriteByte(MakeObjectTag(arraycount, bytelen));
            if (arraycount >= 32)
            {
                byte[] buf = BitConverter.GetBytes(arraycount);
                stream.Write(buf, 0, bytelen);
            }
        }
        static void PackJsonString(System.IO.Stream stream, string str, IList<string> pubdict, IList<string> localdict, bool riseDictByString)
        {
            if (str.Length < 2)
            {//直接写入
                WriteStringDataDirect(stream, str);
            }
            else
            {
                int pid = GetKey(pubdict, str);
                if (pid >= 0)//公共字典
                {
                    WriteStringDataDict(stream, true, pid);
                }
                else //本地字典
                {
                    if (localdict.Contains(str) == false)
                    {
                        localdict.Add(str);
                    }
                    pid = GetKey(localdict, str);
                    WriteStringDataDict(stream, false, pid);
                }
            }
        }
        static void PackJsonNumber(System.IO.Stream stream, JsonNumber number)
        {
            if (number.isNull)
            {
                stream.WriteByte(MakeNumberTag(false, false, true, false, 0));
            }
            else if (number.isBool)
            {
                stream.WriteByte(MakeNumberTag(false, true, number.AsBool(), false, 0));
            }
            else
            {
                string numstr = number.ToString();
                if (numstr.Contains(".") || numstr.Contains("e") || numstr.Contains("E"))
                {
                    WriteFloatData(stream, (float)number.AsDouble());
                }
                else
                {
                    WriteIntData(stream, number.AsInt());
                }
            }
        }

        static void PackJsonArray(System.IO.Stream stream, JsonArray array, IList<string> pubdict, IList<string> localdict, bool riseDictByKey, bool riseDictByString)
        {
            WriteArrayCountHead(stream, array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                PackJson(stream, array[i], pubdict, localdict, riseDictByKey, riseDictByString);
            }
        }
        static void PackJsonObject(System.IO.Stream stream, JsonObject _object, IList<string> pubdict, IList<string> localdict, bool riseDictByKey, bool riseDictByString)
        {
            WriteObjectCountHead(stream, _object.Count);
            foreach (string key in _object.Keys)
            {
                if (key.Length < 2)
                {
                    WriteStringDataDirect(stream, key);
                }
                else
                {
                    int pid = GetKey(pubdict, key);
                    if (pid >= 0)//公共字典
                    {
                        WriteStringDataDict(stream, true, pid);
                    }
                    else //本地字典
                    {
                        if (riseDictByKey)
                        {
                            pid = GetFreeKey(pubdict);
                            pubdict.Add(key);
                            WriteStringDataDict(stream, true, pid);
                        }
                        else
                        {
                            if (localdict.Contains(key) == false)
                            {
                                localdict.Add(key);
                            }
                            pid = GetKey(localdict, key);
                            WriteStringDataDict(stream, false, pid);
                        }
                    }

                }
            }
            foreach (var item in _object)
            {
                PackJson(stream, item.Value, pubdict, localdict, riseDictByKey, riseDictByString);
            }
        }
        static void PackJson(System.IO.Stream stream, IJsonNode node, IList<string> pubdict, IList<string> localdict, bool riseDictByKey, bool riseDictByString)
        {
            if (node is JsonString)
            {
                string v = node.AsString();
                if (riseDictByString && v != null && v.Length > 1 && pubdict.Contains(v) == false)
                {
                    pubdict.Add(v);
                }
                PackJsonString(stream, v, pubdict, localdict, riseDictByString);
            }
            else if (node is JsonNumber)
            {
                PackJsonNumber(stream, node as JsonNumber);
            }
            else if (node is JsonArray)
            {
                PackJsonArray(stream, node as JsonArray, pubdict, localdict, riseDictByKey, riseDictByString);
            }
            else if (node is JsonObject)
            {
                PackJsonObject(stream, node as JsonObject, pubdict, localdict, riseDictByKey, riseDictByString);
            }
        }

        static IJsonNode UnPackJsonNumber(System.IO.Stream stream, byte tagfirst)
        {
            JsonNumber number = new JsonNumber();
            bool isFloat = (tagfirst & 32) > 0;
            bool isBool = (tagfirst & 16) > 0;
            bool isNull = (tagfirst & 8) > 0;
            bool isNeg = (tagfirst & 4) > 0;
            int blen = tagfirst % 4 + 1;
            byte[] buf = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                buf[i] = 0;
            }
            if (isBool)
            {
                number.SetBool(isNull);
            }
            else if (isNull)
            {
                number.SetNull();
            }
            else if (isFloat)
            {
                stream.Read(buf, 0, blen);
                number.value = BitConverter.ToSingle(buf, 0);
            }
            else
            {
                stream.Read(buf, 0, blen);
                int v = BitConverter.ToInt32(buf, 0);
                number.value = isNeg ? -v : v;
            }
            stream.Close();
            return number;
        }
        static IJsonNode UnPackJsonString(System.IO.Stream stream, byte tagfirst, IList<string> pubdict, IList<string> localdict)
        {
            JsonString str = new JsonString();
            str.value = ReadString(stream, tagfirst, pubdict, localdict);
            return str;
        }
        static IJsonNode UnPackJsonArray(System.IO.Stream stream, byte tagfirst, IList<string> pubdict, IList<string> localdict)
        {
            JsonArray array = new JsonArray();
            int count = ReadCountHead(stream, tagfirst);
            for (int i = 0; i < count; i++)
            {
                array.Add(UnPackJson(stream, pubdict, localdict));
            }
            return array;
        }
        static IJsonNode UnPackJsonObject(System.IO.Stream stream, byte tagfirst, IList<string> pubdict, IList<string> localdict)
        {
            JsonObject _object = new JsonObject();
            int count = ReadCountHead(stream, tagfirst);
            List<string> keys = new List<string>();
            for (int i = 0; i < count; i++)
            {
                byte ft = (byte)stream.ReadByte();
                keys.Add(ReadString(stream, ft, pubdict, localdict));
            }
            for (int i = 0; i < count; i++)
            {
                _object.Add(keys[i], UnPackJson(stream, pubdict, localdict));
            }
            return _object;
        }
        static IJsonNode UnPackJson(System.IO.Stream stream, IList<string> pubdict, IList<string> localdict)
        {
            byte b = (byte)stream.ReadByte();
            bool t1 = (b & 128) > 0;
            bool t2 = (b & 64) > 0;

            if (t1 && !t2)//number
            {
                return UnPackJsonNumber(stream, b);
            }
            else if (t1 && t2)//string
            {
                return UnPackJsonString(stream, b, pubdict, localdict);
            }
            else if (!t1 && !t2)//array
            {
                return UnPackJsonArray(stream, b, pubdict, localdict);
            }
            else//object
            {
                return UnPackJsonObject(stream, b, pubdict, localdict);
            }
        }
        static object ToGeneric(Type type,IJsonNode json)
        {
            Type[] arguments = type.GetGenericArguments();
            if (arguments.Length == 1)
            {
                var list = json.AsList();
                var makeme = typeof(List<>).MakeGenericType(arguments);
                var listObj = Activator.CreateInstance(makeme);
                MethodInfo method = listObj.GetType().GetMethod("Add");
                //object[] objarr = new object[list.Count];
                foreach (var item in list)
                {
                    object o = null;
                    if (item.type == EJsonType.Object)
                    {
                        if (arguments[0].IsGenericType)
                        {
                            o = ToGeneric(arguments[0], (IJsonNode)item.AsDict());
                        }
                        else
                            o = ToObject(arguments[0], item.AsDict());
                    }
                    else if (item.type == EJsonType.String)
                        o = item.AsString();
                    else if (item.type == EJsonType.Number)
                        o = item.AsDouble();
                    else if (item.type == EJsonType.Array)
                        lhDebug.LogError((object)"LaoHan: array is has Array");
                    method.Invoke(listObj, new object[] { o });
                }
                return listObj;
            }
            else if (arguments.Length == 2)
            {
                var content = json.AsDict();
                var makeme = typeof(Dictionary<,>).MakeGenericType(arguments);
                var dicObj = Activator.CreateInstance(makeme);
                MethodInfo method = dicObj.GetType().GetMethod("Add");
                foreach (var item in content)
                {
                    object o = null;
                    if(item.Value.type==EJsonType.Object)
                        o = ToObject(arguments[1], item.Value.AsDict());
                    else if (item.Value.type == EJsonType.String)
                        o = item.Value.AsString();
                    else if (item.Value.type == EJsonType.Number)
                        o = item.Value.AsDouble();
                    else if (item.Value.type == EJsonType.Array)
                        lhDebug.LogError((object)"LaoHan: array is has Array");
                    method.Invoke(dicObj, new object[] { item.Key, o });
                }
                return dicObj;
            }
            else
            {
                lhDebug.LogError((object)("LaoHan:propertyType IsGenericType but arguments is :  " + arguments.Length));
                return null;
            }
        }
        static object ToObject(Type type,IDictionary<string,IJsonNode> dic)
        {
            object obj = Activator.CreateInstance(type);
            var propertyInfos = type.GetProperties();
            foreach (var info in propertyInfos)
            {
                if (!dic.ContainsKey(info.Name))
                    continue;
                Type propertyType = info.PropertyType;
                if (propertyType == typeof(System.Double))
                    info.SetValue(obj, Convert.ToDouble(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.Single))
                    info.SetValue(obj, Convert.ToSingle(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.UInt16))
                    info.SetValue(obj, Convert.ToUInt16(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.Int16))
                    info.SetValue(obj, Convert.ToInt16(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.Int32))
                    info.SetValue(obj, Convert.ToInt32(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.Int64))
                    info.SetValue(obj, Convert.ToInt64(dic[info.Name].AsString()), null);
                else if (propertyType == typeof(System.String))
                    info.SetValue(obj, dic[info.Name].AsString(), null);
                else if (propertyType == typeof(System.Boolean))
                    info.SetValue(obj, Convert.ToBoolean(dic[info.Name].AsString()), null);
                else
                {
                    if (propertyType.IsGenericType)
                    {
                        if(dic.ContainsKey(info.Name))
                        {
                            info.SetValue(obj, ToGeneric(propertyType, dic[info.Name]), null);
                        }
                    }
                    else if(propertyType.IsNestedPublic)
                    {
                        info.SetValue(obj, ToObject(propertyType, dic[info.Name].AsDict()), null);
                    }
                     //lhDebug.Log(info.Name + "  "+ propertyType.IsAnsiClass
                     //    + "  " + propertyType.IsClass
                     //    + "  " + propertyType.IsNested
                     //    + "  " + propertyType.IsNestedPublic
                     //    + "  " + propertyType.IsPointer
                     //    + "  " + propertyType.IsPrimitive
                     //    + "  " + propertyType.IsSealed
                     //    + "  " + propertyType.IsSpecialName
                     //    );

                }
            }
            return obj;
        }

        private static void BuildIndents(int indents, StringBuilder output)
        {
            indents += m_defaultIndent;
            for (; indents > 0; indents--)
                output.Append(m_indent);
        }

        private static bool InString()
        {
            return m_inDoubleString || m_inSingleString;
        }

        #endregion

        #region internal static
        internal static IJsonNode ScanFirst(char c)
        {
            if (c == ' ' || c == '\n' || c == '\r' || c == '\t')
            {
                return null;
            }
            if (c == '{')
            {
                return new JsonObject();
            }
            else if (c == '[')
            {
                return new JsonArray();
            }
            else if (c == '"')
            {
                return new JsonString();
            }
            else
            {
                return new JsonNumber();
            }
        }
        internal static IJsonNode Scan(ScanObj scan)
        {
            for (int i = 0; i < scan.json.Length; i++)
            {
                IJsonNode node = ScanFirst(scan.json[i]);
                if (node != null)
                {
                    scan.seed = i;
                    node.Scan(scan);
                    return node;
                }
            }
            return null;
        }
        #endregion
    }
}