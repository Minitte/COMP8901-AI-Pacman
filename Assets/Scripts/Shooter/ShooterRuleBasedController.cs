using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRuleBasedController : MonoBehaviour
{
    private enum DataNames
    { 
        MY_AMMO,
        MY_ENERGY,
        MY_HEALTH,

        ENEMY_AMMO,
        ENEMY_ENERGY,
        ENEMY_HEALTH
    }

    private RuleSystem.RuleSystem m_rbs;

    private void Awake()
    {
        RuleSystem.Rule[] rules = RuleSystem.RuleSetReader.ReadRuleFile("shooter.rules");
        m_rbs = new RuleSystem.RuleSystem(rules);
    }

    public ShooterChoice GetChoice(ShooterController me, ShooterController enemy)
    {
        // convert to data
        Dictionary<string, int> data = new Dictionary<string, int>();

        data.Add(DataNames.MY_AMMO.ToString(), me.ammoCount);
        data.Add(DataNames.MY_ENERGY.ToString(), me.energyCount);
        data.Add(DataNames.MY_HEALTH.ToString(), me.health);

        data.Add(DataNames.ENEMY_AMMO.ToString(), enemy.ammoCount);
        data.Add(DataNames.ENEMY_ENERGY.ToString(), enemy.energyCount);
        data.Add(DataNames.ENEMY_HEALTH.ToString(), enemy.health);

        // get results
        string output = m_rbs.Eval(data);

        // convert to enum

        // no rule applicable..
        if (output == null) return ShooterChoice.WAITING;

        if (output == ShooterChoice.SHOOT.ToString()) return ShooterChoice.SHOOT;

        if (output == ShooterChoice.DODGE.ToString()) return ShooterChoice.DODGE;

        if (output == ShooterChoice.RELOAD.ToString()) return ShooterChoice.RELOAD;

        Debug.Log("Rule system outputted an unknown value! " + output);

        return ShooterChoice.WAITING;
    }
}
