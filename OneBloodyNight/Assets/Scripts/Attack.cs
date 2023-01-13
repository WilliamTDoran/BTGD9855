using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a parent class that handles all activated attacks and hit-boxed abilities.
/// </summary>
public class Attack : GameActor
{
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
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("Debug")]

    [Tooltip("Hide or show the hitbox of the attack when attacking")]
    [SerializeField]
    private bool showHitbox = false;

    [Tooltip("Reference to the hitbox renderer. Must be not null to show hitbox")]
    [SerializeField]
    private MeshRenderer hitboxMesh;
    /*~~~~~~~~~~~~~~~~~~~~*/

    private void OnEnable()
    {
        hitboxMesh.enabled = showHitbox;
    }
}
