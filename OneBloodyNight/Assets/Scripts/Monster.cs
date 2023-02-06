using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for all monster types. Some monsters can be driven just from this, complex ones inherit their own classes
/// 
/// Version 1.0 (1/20/23), William Doran
/// </summary>
public class Monster : GameActor
{
    private Player player;

    private int curHitPoints;
    public int CurHitPoints { get { return curHitPoints; } set { curHitPoints = value; } }

    /* Exposed Variables */
    [Tooltip("The Monster's maximum possible hit points (also the hit points it spawns with)")]
    [SerializeField]
    private int maxHitPoints;
    public int MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = value; } }

    [Tooltip("A reference to the monster's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Tooltip("The amount of blood the player recovers when killing the enemy")]
    [SerializeField]
    public float bloodOnKill = 50.0f;
    public float BloodOnKill { get { return bloodOnKill; } }

    [SerializeField]
    private bool debugFollow;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        curHitPoints = maxHitPoints;
        canAttack = true;
        canMove = true;
        facingAngle = 0;

        StartCoroutine(DebugAttackCycle());

        player = Player.plr;
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log(gameObject.name + " " + curHitPoints);

        facingAngle = Vector3.SignedAngle(Vector3.right, controller.IntendedDirection, Vector3.forward);
        facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;

        if (curHitPoints <= 0)
        {
            Bloodmeter.instance.bloodmeter.value += bloodOnKill;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (debugFollow)
        {
            DebugCharge();
        }
    }

    private IEnumerator DebugAttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            basicAttack.StartSwing();

            Debug.Log("Werewolf Swing Done");
        }
    }

    private void DebugCharge()
    {
        if (canMove && !player.Immune)
        {
            Vector3 direction = player.Rb.position - rb.position;
            direction.Normalize();

            rb.AddForce(direction * speed, ForceMode.Force);
        }
    }
}