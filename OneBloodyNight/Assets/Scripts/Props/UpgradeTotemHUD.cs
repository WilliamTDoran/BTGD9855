using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTotemHUD : MonoBehaviour
{
    //[SerializeField]
    //GameObject upgradeHUD;
    [SerializeField]
    internal UpgradeVars bloodUsageUpgrade;
    [SerializeField]
    internal UpgradeVars attackDmgUpgrade;
    [SerializeField]
    internal UpgradeVars bloodRegenUpgrade;
    [SerializeField]
    internal UpgradeVars moveSpeedUpgrade;
    internal bool topUpgrade;
    internal bool rightUpgrade;
    internal bool bottomUpgrade;
    internal bool leftUpgrade;
    /*
    enum upgrades
    {
        bloodUsage,
        attackDmg,
        bloodRegain,
        movementSpeed,
        topSpecial,
        leftSpecial,
        bottomSpecial,
        rightSpecial,
        LENGTH
    }*/

    internal void upgradeBloodUsage()
    {
        if (!bloodUsageUpgrade.upgraded)
        {
            bloodUsageUpgrade.upgraded = true;
            Bloodmeter.instance.bloodLossRate *= bloodUsageUpgrade.multiplier;
        }
        gameObject.SetActive(false);
    }

    public void upgradeAttackDmg()
    {
        if (!attackDmgUpgrade.upgraded)
        {
            attackDmgUpgrade.upgraded = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradeAttacks(attackDmgUpgrade.multiplier, true);
            } else {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }

    public void upgradeBloodRegen()
    {
        if (!bloodRegenUpgrade.upgraded)
        {
            bloodRegenUpgrade.upgraded = true;
            Player.plr.bloodRegainMult *= bloodRegenUpgrade.multiplier;
        }
        gameObject.SetActive(false);
    }

    public void upgradeMoveSpeed()
    {
        if (!moveSpeedUpgrade.upgraded)
        {
            moveSpeedUpgrade.upgraded = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().BaseSpeed = Player.plr.GetComponent<PlayerStrigoi>().BaseSpeed * moveSpeedUpgrade.multiplier;
                Player.plr.GetComponent<PlayerStrigoi>().increaseSpeed(moveSpeedUpgrade.multiplier);
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }

    public void upgradeTopSpecial()
    {
        if (!topUpgrade)
        {
            topUpgrade = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedSwarm = true;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }
    public void upgradeBottomSpecial()
    {
        if (!bottomUpgrade)
        {
            bottomUpgrade = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedBackstab = true;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }

    public void upgradeRightSpecial()
    {
        if (!rightUpgrade)
        {
            rightUpgrade = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.abilityOneCost = 0;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }

    public void upgradeLeftSpecial()
    {
        if (!leftUpgrade)
        {
            leftUpgrade = true;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                //Player.plr.abilityOneCost = 0;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        gameObject.SetActive(false);
    }
}

[System.Serializable]
internal class UpgradeVars
{
    public bool upgraded = false;
    public float multiplier;
}
