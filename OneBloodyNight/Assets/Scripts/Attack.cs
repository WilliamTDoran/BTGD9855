using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a parent class that handles all activated attacks and hit-boxed abilities.
/// </summary>
public class Attack : GameActor
{
    private string swingTrigger = "Swing";

    /* Exposed Variables */
    [Header("Attack Statistics")]

    [Tooltip("The amount of damage dealt by the attack")]
    [SerializeField]
    private int damage; //the amount of damage dealt by the attack
    public int Damage { get { return damage; } set { damage = value; } }

    [Tooltip("The amount of knockback dealt by the attack")]
    [SerializeField]
    private float knockback; //the amount of knockback dealt by the attack
    public float Knockback { get { return knockback; } set { knockback = value; } }

    [Tooltip("The amount of pushback on the attacker")]
    [SerializeField]
    private float pushback; //the amount of pushback on the attacker
    public float Pushback { get { return pushback; } set { pushback = value; } }

    [Tooltip("Multiplier for pushback when hitting a wall instead of a gameactor. 1 is identical.")]
    [SerializeField]
    private float wallPushbackScalar; //multiplier for pushback when hitting a wall instead of a gameactor. 1 is identical.
    public float WallPushbackScalar { get { return wallPushbackScalar; } set { wallPushbackScalar = value; } }

    [Tooltip("Whether the attacker is forced to stand still while using this attack")]
    [SerializeField]
    private bool forceStill; //whether the attacker is forced to stand still while using this attack
    public bool ForceStill { get { return forceStill; } set { forceStill = value; } }

    [Tooltip("Scalar that defines how far from the player the attack hitbox is")]
    [SerializeField]
    private float floatDistance;
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("References")]

    [Tooltip("The GameActor who is making the attack")]
    [SerializeField]
    private GameActor attacker;

    [Tooltip("The attack's own animator")]
    [SerializeField]
    private Animator animator;
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("Debug")]

    [Tooltip("Hide or show the hitbox of the attack when attacking")]
    [SerializeField]
    private bool showHitbox = false;

    [Tooltip("Reference to the hitbox renderer. Must be not null to show hitbox")]
    [SerializeField]
    private MeshRenderer hitboxMesh;
    /*~~~~~~~~~~~~~~~~~~~~*/

    protected override void Update()
    {
        base.Update();

        if (basicAttackDown && attacker.CanAttack)
        {
            StartSwing();
        }
    }

    private void StartSwing()
    {
        PositionAttack();

        if (showHitbox)
        {
            hitboxMesh.enabled = true;
        }

        animator.SetTrigger(swingTrigger);
        attacker.CanAttack = false;

        if (forceStill)
        {
            attacker.CanMove = false;
            attacker.Rb.velocity = Vector3.zero;
        }
    }

    private void PositionAttack()
    {
        Quaternion facingAngleRotation = Quaternion.Euler(0, 0, attacker.FacingAngle + 90f);
        Vector3 v3Facing = facingAngleRotation * Vector3.down; //Adding the 90f is a really silly way to handle this but damn it if it doesn't work...
        Vector3 playerFacingDirection = new Vector2(v3Facing.x, v3Facing.y);
        playerFacingDirection.Normalize();

        transform.localPosition = playerFacingDirection * floatDistance;
        transform.rotation = facingAngleRotation;
    }

    public void EndSwing()
    {
        Debug.Log("End Swing");

        attacker.CanAttack = true;

        if (forceStill)
        {
            attacker.CanMove = true;
        }

        if (showHitbox)
        {
            hitboxMesh.enabled = false;
        }
    }
}
