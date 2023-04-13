using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTotemHUD : MonoBehaviour
{
    internal static UpgradeTotemHUD instance;
    //[SerializeField]
    //GameObject upgradeHUD;
    [Header("Upgrade Variables")]
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
    [SerializeField]
    internal bool disableable;

    [Header("Costs")]
    [SerializeField]
    private float bloodUsageCost;
    [SerializeField]
    private float attackDmgCost;
    [SerializeField]
    private float bloodRegenCost;
    [SerializeField]
    private float moveSpeedCost;
    [SerializeField]
    private float topCost;
    [SerializeField]
    private float rightCost;
    [SerializeField]
    private float bottomCost;
    [SerializeField]
    private float leftCost;

    [SerializeField]
    private Button bloodUsage;
    [SerializeField]
    private Button attackDmg;
    [SerializeField]
    private Button bloodRegen;
    [SerializeField]
    private Button moveSpeed;
    [SerializeField]
    private Button top;
    [SerializeField]
    private Button right;
    [SerializeField]
    private Button bottom;
    [SerializeField]
    private Button left;

    private bool discount = false;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (disableable)
            {
                gameObject.SetActive(false);
            }
        }
    }

    internal void upgradeBloodUsage()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - bloodUsageCost)
        {
            Bloodmeter.instance.changeBlood(-bloodUsageCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((bloodUsageUpgrade.upgraded == 0 || bloodUsageUpgrade.upgraded == 1) && paid)
        {
            bloodUsageUpgrade.upgraded++;
            Bloodmeter.instance.bloodLossRate *= bloodUsageUpgrade.multiplier;
        }
        if(bloodUsageUpgrade.upgraded == 2)
        {
            bloodUsage.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    private void disableHUD()
    {
        Time.timeScale = 1f;
        ToolTipManager._instance.HideToolTip();
        gameObject.SetActive(false);
        Player.plr.Stunned = false;
    }

    public void upgradeAttackDmg()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - attackDmgCost)
        {
            Bloodmeter.instance.changeBlood(-attackDmgCost);
            paid = true;
        }
        else if (discount) paid = true;
        if (attackDmgUpgrade.upgraded <= 1)
        {
            attackDmgUpgrade.upgraded++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradeAttacks(attackDmgUpgrade.multiplier, true);
            } else {
                Debug.Log("Cost not implemented for this character");
            }
        }
        if (attackDmgUpgrade.upgraded == 2)
        {
            attackDmg.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    public void upgradeBloodRegen()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - bloodRegenCost)
        {
            Bloodmeter.instance.changeBlood(-bloodRegenCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((bloodRegenUpgrade.upgraded <= 1) && paid)
        {
            bloodRegenUpgrade.upgraded++;
            Player.plr.bloodRegainMult *= bloodRegenUpgrade.multiplier;
        }
        if (bloodUsageUpgrade.upgraded == 2)
        {
            bloodRegen.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    public void upgradeMoveSpeed()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - moveSpeedCost)
        {
            Bloodmeter.instance.changeBlood(-moveSpeedCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((moveSpeedUpgrade.upgraded <= 1) && paid)
        {
            moveSpeedUpgrade.upgraded++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().BaseSpeed = Player.plr.GetComponent<PlayerStrigoi>().BaseSpeed * moveSpeedUpgrade.multiplier;
                Player.plr.GetComponent<PlayerStrigoi>().increaseSpeed(moveSpeedUpgrade.multiplier);
                //moveSpeed.interactable = false;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        if (moveSpeedUpgrade.upgraded == 2)
        {
            moveSpeed.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    public void upgradeTopSpecial()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - topCost)
        {
            Bloodmeter.instance.changeBlood(-topCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((topUpgrade == 0) && paid)
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
        } else if ((topUpgrade == 1) && paid)
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
        if (topUpgrade == 2)
        {
            top.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }
    public void upgradeBottomSpecial()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - bottomCost)
        {
            Bloodmeter.instance.changeBlood(-bottomCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((bottomUpgrade <= 1) && paid)
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
        if (bottomUpgrade == 2)
        {
            bottom.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    public void upgradeRightSpecial()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - rightCost)
        {
            Bloodmeter.instance.changeBlood(-rightCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((rightUpgrade == 0) && paid)
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
        } else if ((rightUpgrade == 1) && paid)
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
        if (rightUpgrade == 2)
        {
            right.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    public void upgradeLeftSpecial()
    {
        bool paid = false;
        if (!discount && 0 < Bloodmeter.instance.bloodmeter.value - leftCost)
        {
            Bloodmeter.instance.changeBlood(-leftCost);
            paid = true;
        }
        else if (discount) paid = true;
        if ((leftUpgrade == 0) && paid)
        {
            leftUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().upgradedFrenzy = true;
                Player.plr.GetComponent<PlayerStrigoi>().frenzyRegainHit = 1;
                
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        } else if ((leftUpgrade == 1) && paid)
        {
            leftUpgrade++;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null)
            {
                Player.plr.GetComponent<PlayerStrigoi>().frenzyRegainHit = 2;
                left.interactable = false;
            }
            else
            {
                Debug.Log("Upgrade not implemented for this character");
            }
        }
        if (leftUpgrade == 2)
        {
            left.interactable = false;
        }
        if (disableable)
        {
            disableHUD();
        }
    }

    void Update()
    {
        if ((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonDown("Interact")) && gameObject.activeSelf && disableable)
        {
            disableHUD();
        }
    }

    internal void Saver()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.saveGame(bloodUsageUpgrade.upgraded, attackDmgUpgrade.upgraded, bloodRegenUpgrade.upgraded, moveSpeedUpgrade.upgraded, topUpgrade, leftUpgrade, bottomUpgrade, rightUpgrade);
        }
    }

    /*
     bloodUsage,
        attackDmg,
        bloodRegain,
        movementSpeed,
        topSpecial,
        leftSpecial,
        bottomSpecial,
        rightSpecial,
     
     */
    internal void loadSavedUpgrades(int bloodUsage, int attackDmg, int bloodRegain, int movementSpeed, int topSpecial, int leftSpecial, int bottomSpecial, int rightSpecial)
    {
        discount = true;
        for (int i=0; i< bloodUsage; i++)
        {
            upgradeBloodUsage();
        }
        for (int i = 0; i < attackDmg; i++)
        {
            upgradeAttackDmg();
        }
        for (int i = 0; i < bloodRegain; i++)
        {
            upgradeBloodRegen();
        }
        for (int i = 0; i < movementSpeed; i++)
        {
            upgradeMoveSpeed();
        }
        for (int i = 0; i < topSpecial; i++)
        {
            upgradeTopSpecial();
        }
        for (int i = 0; i < leftSpecial; i++)
        {
            upgradeLeftSpecial();
        }
        for (int i = 0; i < bottomSpecial; i++)
        {
            upgradeBottomSpecial();
        }
        for (int i = 0; i < rightSpecial; i++)
        {
            upgradeRightSpecial();
        }
        discount = false;
    }

}

[System.Serializable]
internal class UpgradeVars
{
    public int upgraded = 0;
    public float multiplier;
}