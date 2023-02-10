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
    private Player player; //reference to the player. kind of antequated since its from before Player.cs had static reference, but whatever

    private string refCode1 = "basic"; //See Attack comments on attackerGrantedCode

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

    /// <summary>
    /// Standard Start function. Initializes important values and gets references.
    /// Currently also starts the shitty basic attack timing that's just used for testing.
    /// </summary>
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

    /// <summary>
    /// Standard Update function. Drives death.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        //Debug.Log(gameObject.name + " " + curHitPoints);

        if (CurHitPoints <= 0)
        {
            Bloodmeter.instance.bloodmeter.value += bloodOnKill;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Standard FixedUpdate function. Drives monster movement
    /// </summary>
    private void FixedUpdate()
    {
        if (debugFollow && player.Visible)
        {
            DebugCharge();
        }
    }

    /// <summary>
    /// A called-to function to drive things that happen at the end of an attack. Used for resetting canAttack
    /// </summary>
    /// <param name="code">the granted code to identify which attack is ending</param>
    internal override void OnAttackEnd(string code)
    {
        base.OnAttackEnd(code);

        canAttack = true;
        Debug.Log("Werewolf Swing Done");
    }

    /// <summary>
    /// A temporary function for a rudimentary attack cycle. Will be replaced.
    /// </summary>
    /// <returns>Functional IEnumerator return</returns>
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

    /// <summary>
    /// A temporary function for a rudimentary monster chasing ai. Will be replaced.
    /// </summary>
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