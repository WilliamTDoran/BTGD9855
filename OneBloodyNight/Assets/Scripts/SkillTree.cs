using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField]
    private CombatManager combat;

    [SerializeField]
    private PlayerStrigoi Strigoi;

    [SerializeField]
    private Monster Enemy;

    private int increase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BloodRegen()
    {
        Enemy.bloodOnKill += 10;
    }

    private void BloodUse()
    {

    }

    private void PlrSpeed()
    {
        Strigoi.baseSpeed += 3;
    }

    private void AttackPwr()
    {

    }
}
