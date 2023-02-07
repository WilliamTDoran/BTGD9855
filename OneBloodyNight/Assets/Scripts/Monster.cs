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

    private string refCode1 = "basic";

    /* Exposed Variables */
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

        CurHitPoints = MaxHitPoints;
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

        if (CurHitPoints <= 0)
        {
            Bloodmeter.instance.bloodmeter.value += bloodOnKill;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (debugFollow && player.Visible)
        {
            DebugCharge();
        }
    }

    internal override void OnAttackEnd(string code)
    {
        base.OnAttackEnd(code);

        canAttack = true;
        Debug.Log("Werewolf Swing Done");
    }

    private IEnumerator DebugAttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            facingAngle = Vector3.SignedAngle(Vector3.right, (player.Rb.position - rb.position), Vector3.forward);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;

            basicAttack.StartSwing(refCode1);
            canAttack = false;
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