using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;


public class Character
{

    public static readonly Color TIER_1_COLOR = UtilsClass.GetColorFromString("FFFFFF");
    public static readonly Color TIER_2_COLOR = UtilsClass.GetColorFromString("1ea4d9");
    public static readonly Color TIER_3_COLOR = UtilsClass.GetColorFromString("bd20ed");

    public const int HEALTH_MAX = 100;
    public const int SHIELD_AMOUNT_PER_SEGMENT = 25;

    public event EventHandler OnHealthShieldChanged;

    public enum BodyArmor
    {
        None,
        Tier_1,
        Tier_2,
        Tier_3
    }

    private BodyArmor bodyArmor;
    private int headlth;
    private int shield;

    public Character()
    {
        bodyArmor = BodyArmor.None;
        headlth = HEALTH_MAX;
    }

    public BodyArmor GetEquippedBodyArmor()
    {
        return bodyArmor;
    }
    public void SetEquippedBodyArmor(BodyArmor bodyArmor)
    {
        this.bodyArmor = bodyArmor;

        switch (bodyArmor)
        {  
            case BodyArmor.None:
                shield = 0;
                break;
            case BodyArmor.Tier_1:
                shield = SHIELD_AMOUNT_PER_SEGMENT * 2;
                break;
            case BodyArmor.Tier_2:
                shield = SHIELD_AMOUNT_PER_SEGMENT * 3;
                break;
            case BodyArmor.Tier_3:
                shield = SHIELD_AMOUNT_PER_SEGMENT * 4;
                break;

        }
    }
    public void Damage(int damageAmout)
    {
        if(damageAmout < shield)
        {
            // Shield absorbs all damage
            shield -= damageAmout;
        }else
        {
            // Shield cannot absorbs all damage
            headlth -= damageAmout + shield;
            shield = 0;
        }
        CMDebug.TextPopupMouse(shield + ", " + headlth);
        if (OnHealthShieldChanged != null) OnHealthShieldChanged(this, EventArgs.Empty);
    }

    public int GetHealth()
    {
        return headlth;
    }
    public int GetShield()
    {
        return shield;
    }

    public int GetShieldMax()
    {
        switch (bodyArmor)
        {
            default:
            case BodyArmor.None: return 0;
            case BodyArmor.Tier_1: return SHIELD_AMOUNT_PER_SEGMENT * 2;
            case BodyArmor.Tier_2: return SHIELD_AMOUNT_PER_SEGMENT * 3;
            case BodyArmor.Tier_3: return SHIELD_AMOUNT_PER_SEGMENT * 4;
        }
    }
    public void HealHealth(int amount)
    {
        headlth += amount;
        headlth = Mathf.Clamp(headlth, 0, HEALTH_MAX);
        if (OnHealthShieldChanged != null) OnHealthShieldChanged(this, EventArgs.Empty);
    }
    public void HealShield(int amount)
    {
        shield += amount;
        shield = Mathf.Clamp(shield, 0, GetShield());
        if (OnHealthShieldChanged != null) OnHealthShieldChanged(this, EventArgs.Empty);
    }
}
