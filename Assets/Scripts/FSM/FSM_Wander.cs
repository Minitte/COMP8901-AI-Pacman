using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Wander : FSM_State
{
    private LevelInfo m_levelInfo;

    private GameObject m_self;

    public FSM_Wander(LevelInfo levelInfo, GameObject self)
    {
        m_levelInfo = levelInfo;
        m_self = self;
    }

    public void CleanUp()
    {
        m_levelInfo = null;
        m_self = null;
    }

    public void Handle()
    {
        
    }
}
