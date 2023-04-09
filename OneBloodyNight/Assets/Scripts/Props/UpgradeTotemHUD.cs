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
    internal int topUpgrade;
    internal int rightUpgrade;
    internal int bottomUpgrade;
    internal int leftUpgrade;
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
        if (bloodUsageUpgrade.upgraded == 0 || bloodUsageUpgrade.upgraded == 1)
        {
            bloodUsageUpgrade.upgraded++;
            Bloodmeter.instance.bloodLossRate *= bloodUsageUpgrade.multiplier;
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeAttackDmg()
    {
        if (attackDmgUpgrade.upgraded <= 1)
        {
            attackDmgUpgrade.upgraded++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradeAttacks(attackDmgUpgrade.multiplier, true);
            } else {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeBloodRegen()
    {
        if (bloodRegenUpgrade.upgraded <= 1)
        {
            bloodRegenUpgrade.upgraded++;
            Player.plr.bloodRegainMult *= bloodRegenUpgrade.multiplier;
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeMoveSpeed()
    {
        if (moveSpeedUpgrade.upgraded <= 1)
        {
            moveSpeedUpgrade.upgraded++;
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
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeTopSpecial()
    {
        if (topUpgrade == 0)
        {
            topUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedSwarm = true;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        } else if (topUpgrade == 0)
        {
            topUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().increaseSpeed(moveSpeedUpgrade.multiplier);
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    public void upgradeBottomSpecial()
    {
        if (bottomUpgrade <= 1)
        {
            bottomUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedBackstab++;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeRightSpecial()
    {
        if (rightUpgrade == 0)
        {
            rightUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.abilityOneCost = 0;
                Bloodmeter.instance.invisLoss = 2;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        } else if (rightUpgrade == 1)
        {
            rightUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Bloodmeter.instance.invisLoss = 1.5f;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void upgradeLeftSpecial()
    {
        if (leftUpgrade == 0)
        {
            leftUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedFrenzy = true;
                Player.plr.GetComponent<PlayerStrigoi>().frenzyRegainHit = 5;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        } else if (leftUpgrade == 1)
        {
            leftUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().frenzyRegainHit = 10;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Interact")) && gameObject.activeSelf)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }


    internal void loadSavedUpgrades(SavedUpgrades su)
    {
        for (int i=0; i<su.bloodUsageUpgrade; i++)
        {
            upgradeBloodUsage();
        }
        for (int i = 0; i < su.attackDmgUpgrade; i++)
        {
            upgradeAttackDmg();
        }
        for (int i = 0; i < su.bloodRegenUpgrade; i++)
        {
            upgradeBloodRegen();
        }
        for (int i = 0; i < su.moveSpeedUpgrade; i++)
        {
            upgradeMoveSpeed();
        }
        for (int i = 0; i < su.topUpgrade; i++)
        {
            upgradeTopSpecial();
        }
        for (int i = 0; i < su.rightUpgrade; i++)
        {
            upgradeRightSpecial();
        }
        for (int i = 0; i < su.bottomUpgrade; i++)
        {
            upgradeBloodUsage();
        }
    }

}

[System.Serializable]
internal class UpgradeVars
{
    public int upgraded = 0;
    public float multiplier;
}

internal class SavedUpgrades 
{
    internal int bloodUsageUpgrade;
    internal int attackDmgUpgrade;
    internal int bloodRegenUpgrade;
    internal int moveSpeedUpgrade;
    internal int topUpgrade;
    internal int rightUpgrade;
    internal int bottomUpgrade;
    internal int leftUpgrade;
}
