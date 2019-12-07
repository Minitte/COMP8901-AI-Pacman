using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRuleBasedController : MonoBehaviour
{
    private const string RULE_FILE_WEBGL =  "my_ammo = 0 & my_energy = 0 ? reload\n" +
                                            "my_ammo > 0 & enemy_ammo = 0 & enemy_energy = 0 ? shoot\n" +
                                            "my_ammo > 0 my_health > 1 & enemy_energy < 2 & enemy_health = 1 ? shoot\n" +
                                            "my_ammo = 0 & enemy_ammo = 0 ? reload\n" +
                                            "my_health > 1 & my_ammo > 0 ? shoot\n" +
                                            "my_energy > 0 & enemy_ammo > 0 ? dodge\n" +
                                            "my_ammo = 0 & my_health = 1 & my_energy < 2 ? reload\n" +
                                            "my_ammo = 1 & enemy_energy > 1 ? reload\n" +
                                            "my_ammo > 0 & my_energy = 0 & my_health = 1 & enemy_ammo > 0 ? shoot\n" +
                                            "my_ammo > 1 & enemy_ammo = 0 & enemy_energy < 2 & enemy_health = 2 ? shoot\n" +
                                            "my_ammo > 0 & my_energy < 3 & my_ammo < 3 & enemy_ammo = 0 & enemy_energy > 1 & enemy_health = 1 ? reload";


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
        RuleSystem.Rule[] rules = null;

#if UNITY_WEBGL
        rules = RuleSystem.RuleSetReader.ReadRuleString(RULE_FILE_WEBGL);
#else
        rules = RuleSystem.RuleSetReader.ReadRuleFile("assets/shooter.rules");
#endif

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
