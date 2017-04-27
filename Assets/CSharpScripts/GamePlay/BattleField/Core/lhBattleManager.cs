using UnityEngine;
using System.Collections;

public class lhBattleManager  {

    private static lhBattleManager m_instance;
    public static lhBattleManager GetInstance()
    {
        if (m_instance != null) return null;
        return m_instance = new lhBattleManager();
    }
    lhBattleManager()
    {

    }
}
