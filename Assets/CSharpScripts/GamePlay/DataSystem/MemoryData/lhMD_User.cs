using UnityEngine;
using System.Collections;

namespace LaoHan.Data
{
    public partial class lhMemoryData
    {
        public UserData user=new UserData();
        public class UserData
        {
            public string account;//玩家账号
            public string password;//玩家密码
            public string userName;//玩家用户名
            public int userId;//玩家的ID
        }
        public static UserData User { get { return m_instance.user; } }
    }
}