using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// All active GameObjects in scene inherit from this parent class, which holds a number of crucial variables that almost every actor needs
/// This facilitates easy access and communication between discrete objects, and avoids repetition of shared attributes among various entity types.
/// 
/// Version 1.0 (1/10/2023), Will Doran
/// Version 1.1 (2/8/2023),  Will Doran
/// </summary>
public class GameActor : MonoBehaviour
{
    private GameObject controllerObj; //gameObject with the playercontroller script on it
    protected PlayerController controller; //said playercontroller script

    protected bool interactDown;
    protected bool basicAttackDown;
    protected bool advancedAttackDown;
    protected bool load1Down;
    protected bool load2Down;

    protected float facingAngle; //the direction the actor is 'facing' in degrees (has no inherent bearing on the object's actual transform rotation)
    public float FacingAngle { get { return facingAngle; } }

    protected Rigidbody rb; //the rigidbody attached the actor's gameObject
    public Rigidbody Rb { get { return rb; } }
    protected Collider col; //the collider attached to the actor's gameObject

    protected bool canMove; //whether the actor can (willingly) move
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    protected bool canAttack; //whether the actor can initiate attacks
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    protected bool stunned; //whether the actor is affected by hitstun
    public bool Stunned { get { return stunned; } set { stunned = value; } }

    protected bool immune; //whether the actor can take damage
    public bool Immune { get { return immune; } set { immune = value; } }

    private int curHitPoints; //current hit points of the actor, if applicable. Only relevant for monsters and bosses
    public int CurHitPoints { get { return curHitPoints; } set { curHitPoints = value; } }

    /* Exposed Variables */
    [Tooltip("Maximum movement speed - IRRELEVANT FOR NON-MOVING ENTITIES")]
    [SerializeField]
    protected float speed;

    [Tooltip("Duration of immmunity after taking damage - IRRELEVANT FOR NON-ATTACKABLE ENTITIES")]
    [SerializeField]
    protected float immuneDuration;
    public float ImmuneDuration { get { return immuneDuration; } }

    [Tooltip("The Monster's maximum possible hit points (also the hit points it spawns with)")]
    [SerializeField]
    private int maxHitPoints;
    public int MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = value; } }
    /* -~-~-~-~-~-~-~-~- */

    /// <summary>
    /// Standard awake, getting references to key components
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
        Debug.Assert(col != null, "No collider set on: " + gameObject.name);
    }

    /// <summary>
    /// Standard start, getting references to useful objects.
    /// </summary>
    protected virtual void Start()
    {
        controllerObj = GameObject.Find("PlayerController");
        controller = controllerObj.GetComponent<PlayerController>();

        Debug.Assert(controller != null, "No controller set on: " + gameObject.name);
    }

    protected virtual void Update()
    {
        interactDown = controller.InteractDown;
        basicAttackDown = controller.BasicFireDown;
        advancedAttackDown = controller.AdvancedFireDown;
        load1Down = controller.Load1Down;
        load2Down = controller.Load2Down;
    }

    //These are basically just here serving the same function as an interface cause I don't want to double up on both virtual inherited classes and also interfaces
    internal virtual void OnAttackEnd(string code)
    {

    }

    internal virtual void OnSuccessfulAttack(string code)
    {

    }

    internal virtual void OnHitWall()
    {

    }
}
