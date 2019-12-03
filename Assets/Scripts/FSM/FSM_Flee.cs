using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Flee : FSM_State
{
    private LevelInfo m_levelInfo;

    private GameObject m_self;

    private GameObject m_fleeTarget;

    public FSM_Flee(LevelInfo levelInfo, GameObject self, GameObject fleeTarget)
    {
        m_levelInfo = levelInfo;
        m_self = self;
        m_fleeTarget = fleeTarget;
    }

    public void CleanUp()
    {
        m_levelInfo = null;
        m_self = null;
        m_fleeTarget = null;
    }

    public void Handle()
    {
        throw new System.NotImplementedException();
    }
}
