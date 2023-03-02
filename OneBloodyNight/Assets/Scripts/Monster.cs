using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

/// <summary>
/// Parent class for all monster types. Some monsters can be driven just from this, complex ones inherit their own classes
/// 
/// Version 1.0 (1/20/23), William Doran
/// </summary>
public class Monster : GameActor
{
    private Player player; //reference to the player. kind of antequated since its from before Player.cs had static reference, but whatever

    private string refCode1 = "basic"; //See Attack comments on attackerGrantedCode

    private bool chasing;
    public bool Chasing { set { chasing = value; } }
    private int nextPoint;
    private NavMeshPath path;
    private IEnumerator AITickCoroutine;
    private const float AI_TICK_TIME = 0.25f;
    private MonsterControllerAI aiController;

    /* Exposed Variables */
    [Tooltip("A reference to the monster's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Tooltip("The amount of blood the player recovers when killing the enemy")]
    [SerializeField]
    public float bloodOnKill = 50.0f;
    public float BloodOnKill { get { return bloodOnKill; } }
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard Start function. Initializes important values and gets references.
    /// </summary>
    protected override void Start()
    {
        aiController = GetComponent<MonsterControllerAI>();

        base.Start();

        player = Player.plr;

        CurHitPoints = MaxHitPoints;
        canAttack = true;
        canMove = true;
        facingAngle = 0;

        path = new NavMeshPath();

        StartCoroutine(DebugAttackCycle());
        StartAITick();
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
        if (player.Visible && chasing)
        {
            //animator.SetTrigger("Walk");
            
            Chase();
        }
    }

    private IEnumerator AITick()
    { 
        while (true)
        {
            yield return new WaitForSeconds(AI_TICK_TIME);

            if (aiController.CurrentState != null)
            {
                aiController.CurrentState.Reason(player.transform, gameObject.transform);
            }

            if (player.Visible && chasing)
            {
                if (NavMesh.CalculatePath(rb.position, player.Rb.position, -1, path))
                {
                    nextPoint = 1;
                }
                else
                {
                    Debug.Log(gameObject.name + " can't find player!");
                }
            }
        }
    }

    /// <summary>
    /// Simple helper function that uses a raycast to check if there's a wall between the monster and the player.
    /// </summary>
    /// <returns>True if a wall blocks them, false otherwise</returns>
    public bool WallCheck()
    {
        Vector3 direction = player.Rb.position - rb.position;

        int layerMask = 1 << 6;

        return Physics.Raycast(rb.position, direction, direction.magnitude, layerMask);
    }

    /// <summary>
    /// Drives the monster's attack cycle based on a paramaterized time.
    /// </summary>
    /// <returns>Functional IEnumerator return</returns>
    private IEnumerator DebugAttackCycle()
    {
        while (true)
        {
            if (Vector3.Distance(player.Rb.position, rb.position) < 5)
            {
                facingAngle = Vector3.SignedAngle(Vector3.right, (player.Rb.position - rb.position), Vector3.up);
                facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;

                if (canAttack && !Stunned)
                {
                    basicAttack.StartSwing(refCode1);
                    canAttack = false;
                }

                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForSeconds(AI_TICK_TIME);
            }
        }
    }

    /// <summary>
    /// An event-driven function to handle things that happen at the end of an attack. Used for resetting canAttack
    /// </summary>
    /// <param name="code">the granted code to identify which attack is ending</param>
    internal override void OnAttackEnd(string code)
    {
        base.OnAttackEnd(code);

        canAttack = true;
        Debug.Log(gameObject.name + " Swing Done");
    }

    /// <summary>
    /// Drives the monster following the player based on the NavMesh determined path
    /// </summary>
    private void Chase()
    {
        if (path.corners.Length > 0 && nextPoint < path.corners.Length)
        {
            if (canMove && !player.Immune && !Stunned)
            {
                Vector3 direction = path.corners[nextPoint] - rb.position;
                direction.Normalize();

                rb.AddForce(direction * speed, ForceMode.Force);
            }

            if ((path.corners[nextPoint] - rb.position).magnitude <= 2)
            {
                nextPoint++;
            }
        }
    }


    //~~The Coroutine Section~~
    private void StartAITick()
    {
        AITickCoroutine = AITick();
        StartCoroutine(AITickCoroutine);
    }

    private void StopAITick()
    {
        StopCoroutine(AITickCoroutine);
        AITickCoroutine = null;
    }
}