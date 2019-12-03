using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Chase : FSM_State
{
    private LevelInfo m_levelInfo;

    private GameObject m_self;

    private GameObject m_chaseTarget;

    public FSM_Chase(LevelInfo levelInfo, GameObject self, GameObject chaseTarget)
    {
        m_levelInfo = levelInfo;
        m_self = self;
        m_chaseTarget = chaseTarget;
    }

    public void CleanUp()
    {
        m_levelInfo = null;
        m_self = null;
        m_chaseTarget = null;
    }

    public void Handle()
    {
        
    }
}
