#define ClassPool

using UnityEngine;
using System;
using System.IO;
using System.Collections;
//using Mono.Data.Sqlite;
//using System.Data;

namespace LaoHan.Infrastruture
{
    public enum JudgeType
    {
        equal,
        greaterThan,
        lessThan,
        greaterAndEqual,
        lessAndEqual
    }
    public class lhDbAccess
    {
        /*
    #if ClassPool
        SqliteConnection m_conn;
        SqliteCommand m_cmd;
        private int m_index;
        public int index
        {
            get
            {
                return m_index;
            }
        }
        public lhDbAccess(string filePath)
        {
            lhDebug.Log("DataBase :" + lhResources.cacheUrl + filePath +"_______"+ System.IO.File.Exists(lhResources.cacheUrl + filePath));
            m_conn = new SqliteConnection(
    #if UNITY_ANDROID && !UNITY_EDITOR
                "URI=file:"
    #else
                "Data Source=" 
    #endif
                +lhResources.cacheUrl + filePath);
            lhDebug.Log(m_conn);
            m_conn.Open();
            m_cmd = m_conn.CreateCommand();
            lhDebug.Log(m_cmd);
        }
        public void SetIndex(int index)
        {
            this.m_index = index;
        }
        public void Close()
        {
            m_conn.Close();
        }
        private void ExecuteNonQuery(string commandText)
        {
            m_cmd.CommandText = commandText;
            m_cmd.ExecuteNonQuery();
        }
        private string ExecuteScalarToString(string commandText)
        {
            string result = null;
            m_cmd.CommandText = commandText;
            result = (string)m_cmd.ExecuteScalar();
            return result;
        }
        private bool ExecuteScalarToBool(string commandText)
        {
            bool result = false;
            m_cmd.CommandText = commandText;
            result = Convert.ToBoolean(m_cmd.ExecuteScalar());
            return result;
        }
        private float ExecuteScalarToFloat(string commandText)
        {
            float result = 0;
            m_cmd.CommandText = commandText;
            result = Convert.ToSingle(m_cmd.ExecuteScalar());
            return result;
        }
        private int ExecuteScalarToInt(string commandText)
        {
            int result = 0;
            m_cmd.CommandText = commandText;
            result = Convert.ToInt32(m_cmd.ExecuteScalar());
            return result;
        }
        private byte[] ExecuteScalarToByte(string commandText)
        {
            byte[] result = null;
            m_cmd.CommandText = commandText;
            using (SqliteDataReader reader = m_cmd.ExecuteReader())
            {
                reader.Read();
                long length = reader.GetBytes(0, 0, null, 0, int.MaxValue);
                result = new byte[length];
                reader.GetBytes(0, 0, result, 0, result.Length);
            }
            return result;
        }
        private DataTable ExecuteScalarToDataTable(string commandText)
        {
            DataTable result = new DataTable();
            m_cmd.CommandText = commandText;
            using (SqliteDataAdapter ada = new SqliteDataAdapter(m_cmd))
            {
                lhDebug.LogWarning("LaoHan: this function is Invalid in IOS");
                ada.Fill(result);
            }
            return result;
        }
    #else
        string m_path = Application.persistentDataPath + "/DataBase.db";
        private void ExecuteNonQuery(string commandText)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string ExecuteScalarToString(string commandText)
        {
            string result = null;
            using(SqliteConnection conn=new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using(SqliteCommand cmd=conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    result = (string)cmd.ExecuteScalar();
                }
            }
            return result;
        }
        private bool ExecuteScalarToBool(string commandText)
        {
            bool result = false;
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    result = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            return result;
        }
        private float ExecuteScalarToFloat(string commandText)
        {
            float result = 0;
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    result = Convert.ToSingle(cmd.ExecuteScalar());
                }
            }
            return result;
        }
        private int ExecuteScalarToInt(string commandText)
        {
            int result = 0;
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return result;
        }
        private byte[] ExecuteScalarToByte(string commandText)
        {
            byte[] result = null;
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        long length = reader.GetBytes(0, 0, null, 0, int.MaxValue);
                        result = new byte[length];
                        reader.GetBytes(0, 0, result, 0, result.Length);
                    }
                }
            }
            return result;
        }
        private DataTable ExecuteScalarToDataTable(string commandText)
        {
            DataTable dataTable = new DataTable();
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + m_path))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    using (SqliteDataAdapter ada = new SqliteDataAdapter(cmd))
                    {
                        lhDebug.LogWarning("LaoHan: this function is Invalid in IOS");
                        ada.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

    #endif
        public string SelectFromWhere_ToString(string tableName,string select,string where,JudgeType judgeType,string value)
        {
            if (judgeType==JudgeType.equal)
                return ExecuteScalarToString("SELECT " + select + " FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToString("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToString("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToString("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToString("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public bool SelectFromWhere_ToBool(string tableName, string select, string where, JudgeType judgeType, string value)
        {
            if (judgeType == JudgeType.equal)
                return ExecuteScalarToBool("SELECT " + select + " FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToBool("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToBool("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToBool("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToBool("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public int SelectFromWhere_ToInt(string tableName, string select, string where, JudgeType judgeType, string value)
        {
            if (judgeType == JudgeType.equal)
                return ExecuteScalarToInt("SELECT " + select + " FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToInt("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToInt("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToInt("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToInt("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public float SelectFromWhere_ToFloat(string tableName, string select, string where, JudgeType judgeType, string value)
        {
            if (judgeType == JudgeType.equal)
                return ExecuteScalarToFloat("SELECT " + select + " FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToFloat("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToFloat("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToFloat("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToFloat("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public byte[] SelectFromWhere_ToByte(string tableName, string select, string where, JudgeType judgeType, string value)
        {
            if (judgeType == JudgeType.equal)
                return ExecuteScalarToByte("SELECT " + select + " FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToByte("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToByte("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToByte("SELECT " + select + " FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToByte("SELECT " + select + " FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public DataTable SelectFromWhere_ToDataTable(string tableName,string where,JudgeType judgeType,string value)
        {
            if (judgeType == JudgeType.equal)
                return ExecuteScalarToDataTable("SELECT * FROM " + tableName + " WHERE " + where + "==" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterThan)
                return ExecuteScalarToDataTable("SELECT * FROM " + tableName + " WHERE " + where + ">" + "\'" + value + "\'");
            else if (judgeType == JudgeType.lessThan)
                return ExecuteScalarToDataTable("SELECT * FROM " + tableName + " WHERE " + where + "<" + "\'" + value + "\'");
            else if (judgeType == JudgeType.greaterAndEqual)
                return ExecuteScalarToDataTable("SELECT * FROM " + tableName + " WHERE " + where + ">=" + "\'" + value + "\'");
            else
                return ExecuteScalarToDataTable("SELECT * FROM " + tableName + " WHERE " + where + "<=" + "\'" + value + "\'");
        }
        public DataTable SelectFromWhere_ToDataTable(string tableName, string select)
        {
            return ExecuteScalarToDataTable("SELECT " + select + " FROM " + tableName );
        }
        public void InsertIntoValue(string tableName,string[] select,string[] value)
        {
            string selectString = null;
            string valueString = null;
            if (select.Length > 1)
            {
                for (int i = 0; i < select.Length - 1; i++)
                {
                    selectString += select[i] + ",";
                }
                selectString += select[select.Length - 1];
            }
            else
                selectString = select[0];

            if (value.Length > 1)
            {
                for (int i = 0; i < value.Length - 1; i++)
                {
                    selectString += "\'"+value[i]+"\'" + ",";
                }
                valueString += "\'"+select[value.Length - 1]+"\'";
            }
            else
                valueString = value[0];
            ExecuteNonQuery("INSERT INTO " + tableName + "(" + selectString + ")" + " Values(" + valueString + ")");
        }
        public void UpdateSetWhere(string tableName,string set,string setValue, string where, JudgeType judgeType, string whereValue)
        {
            switch(judgeType)
            {
                case JudgeType.equal:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set+"="+"\'"+setValue+"\'" + " WHERE " + where + "=" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.greaterThan:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + ">" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.lessThan:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + "<" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.greaterAndEqual:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + ">=" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.lessAndEqual:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + "<=" + "\'" + whereValue + "\'");
                    break;
            }
        }
        public void UpdateSetWhere(string tableName, string set, byte[] setValue, string where, JudgeType judgeType, string whereValue)
        {
            switch (judgeType)
            {
                case JudgeType.equal:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + "=" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.greaterThan:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + ">" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.lessThan:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + "<" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.greaterAndEqual:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + ">=" + "\'" + whereValue + "\'");
                    break;
                case JudgeType.lessAndEqual:
                    ExecuteNonQuery("UPDATE " + tableName + " SET " + set + "=" + "\'" + setValue + "\'" + " WHERE " + where + "<=" + "\'" + whereValue + "\'");
                    break;
            }
        }
        */
    }
}