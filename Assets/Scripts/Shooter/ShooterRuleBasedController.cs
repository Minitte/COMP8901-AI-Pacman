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
        RuleSystem.Rule[] rules = RuleSystem.RuleSetReader.ReadRuleFile("assets/shooter.rules");
        m_rbs = new RuleSystem.RuleSystem(rules);
    }

    public ShooterChoice GetChoice(ShooterController me, ShooterController enemy)
    {
        // convert to data
        Dictionary<string, int> data = new Dictionary<string, int>();

        data.Add(DataNames.MY_AMMO.ToString().ToLower(), me.ammoCount);
        data.Add(DataNames.MY_ENERGY.ToString().ToLower(), me.energyCount);
        data.Add(DataNames.MY_HEALTH.ToString().ToLower(), me.health);

        data.Add(DataNames.ENEMY_AMMO.ToString().ToLower(), enemy.ammoCount);
        data.Add(DataNames.ENEMY_ENERGY.ToString().ToLower(), enemy.energyCount);
        data.Add(DataNames.ENEMY_HEALTH.ToString().ToLower(), enemy.health);

        // get results
        string output = m_rbs.Eval(data);

        // convert to enum

        // no rule applicable..
        if (output == null) return ShooterChoice.WAITING;

        if (output.Equals(ShooterChoice.SHOOT.ToString().ToLower())) return ShooterChoice.SHOOT;

        if (output.Equals(ShooterChoice.DODGE.ToString().ToLower())) return ShooterChoice.DODGE;

        if (output.Equals(ShooterChoice.RELOAD.ToString().ToLower())) return ShooterChoice.RELOAD;

        Debug.Log("Rule system outputted an unknown value! " + output);

        return ShooterChoice.WAITING;
    }
}
