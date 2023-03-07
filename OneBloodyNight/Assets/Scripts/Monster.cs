using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

    private const float AI_TICK_TIME = 0.25f;
    private MonsterControllerAI aiController;

    private bool chasing;
    public bool Chasing { set { chasing = value; } }
    private int nextPoint;
    private NavMeshPath path;

    private float distanceToPlayer;

    private IEnumerator hoverDistanceSwapCoroutine;
    private float hoverDistanceScale = 0.5f;

    private IEnumerator shuffleDirectionSwapCoroutine;
    private int shuffleDirection = 1;

    private IEnumerator AITickCoroutine;
    private IEnumerator attackCycleCoroutine;

    /* Exposed Variables */
    [Tooltip("A reference to the monster's basic attack object")]
    [SerializeField]
    protected Attack basicAttack;

    [Tooltip("The amount of blood the player recovers when killing the enemy")]
    [SerializeField]
    private float bloodOnKill = 50.0f;
    public float BloodOnKill { get { return bloodOnKill; } }

    [Tooltip("How frequently the monster attacks (specifically, the time in seconds between each attack)")]
    [SerializeField]
    private float attackTimer = 2.0f;
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

        StartAITick();
        StartShuffleDirectionSwap();
    }

    /// <summary>
    /// Standard Update function. Drives death.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        //Debug.Log(gameObject.name + " " + curHitPoints);

        distanceToPlayer = (player.Rb.position - rb.position).magnitude;

        if (CurHitPoints <= 0 && !dead)
        {
            Spawner.enemKilled();
            Bloodmeter.instance.bloodmeter.value += bloodOnKill;

            dead = true;
            animator.SetTrigger("Die");
            StopAllCoroutines();
            canMove = false;
            canAttack = false;
            Stunned = true;
            col.enabled = false;
            basicAttack.StopAllCoroutines();
            basicAttack.gameObject.SetActive(false);
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
                //aiController.CurrentState.Act(player.transform, gameObject.transform);
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

    internal void Aggress(float engagementDistance, float attackRange, float avoidanceRange)
    {
        if (!Stunned && player.Visible)
        {
            if (canMove)
            {
                AttackShuffle(engagementDistance, attackRange, avoidanceRange);
            }

            if (distanceToPlayer <= engagementDistance && canAttack)
            {
                StartAttackCycle();
            }
        }
    }

    private void AttackShuffle(float engagementDistance, float attackRange, float avoidanceRange)
    {
        Vector3 direction = Vector3.zero;

        float usedRange = attackRange + hoverDistanceScale;
        Vector3 proPlayer = player.Rb.position - rb.position;
        Vector3 retroPlayer = rb.position - player.Rb.position;
        float distanceToIdeal = distanceToPlayer - usedRange;

        if (distanceToIdeal > 0.5f)
        {
            direction += proPlayer;
        }
        else if (distanceToIdeal < -0.5f)
        {
            direction += retroPlayer;
        }

        if (Math.Abs(distanceToPlayer - attackRange) < 5)
        {
            direction += Vector3.Cross(proPlayer, Vector3.up) * shuffleDirection;
        }

        Debug.DrawRay(rb.position, direction.normalized * 10);
        rb.AddForce(direction.normalized * speed * 0.8f, ForceMode.Force);
    }

    private IEnumerator ShuffleDirectionSwap()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 5.0f));
            shuffleDirection *= -1;
        }
    }

    private IEnumerator HoverDistanceSwap()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 5.0f));
        hoverDistanceScale += UnityEngine.Random.Range(-0.2f, 0.2f);
        hoverDistanceScale = Math.Clamp(hoverDistanceScale, -0.50f, 1.25f);
    }

    /// <summary>
    /// Drives the monster's attack cycle based on a paramaterized time.
    /// </summary>
    /// <returns>Functional IEnumerator return</returns>
    private IEnumerator AttackCycle()
    {
        facingAngle = Vector3.SignedAngle(Vector3.right, (player.Rb.position - rb.position), Vector3.up);
        facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;

        animator.SetTrigger("Fire");

        if (basicAttack.ForceStill) //Some attacks force the attacker to stand still
        {
            canMove = false;
            rb.velocity = Vector3.zero; //need this otherwise you tokyo drift from momentum
        }

        canAttack = false;

        yield return new WaitForSeconds(attackTimer);
        canAttack = true;
    }

    public void MeleeUse()
    {
        basicAttack.StartSwing(refCode1);
    }

    /// <summary>
    /// An event-driven function to handle things that happen at the end of an attack. Used for resetting canAttack
    /// </summary>
    /// <param name="code">the granted code to identify which attack is ending</param>
    internal override void OnAttackEnd(string code)
    {
        base.OnAttackEnd(code);

        Debug.Log(gameObject.name + " Swing Done");
    }

    internal override void OnReceiveHit()
    {
        base.OnReceiveHit();

        animator.SetTrigger("Owie");
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

    private void StartAttackCycle()
    {
        attackCycleCoroutine = AttackCycle();
        StartCoroutine(attackCycleCoroutine);
    }

    private void StopAttackCycle()
    {
        StopCoroutine(attackCycleCoroutine);
        attackCycleCoroutine = null;
    }

    private void StartShuffleDirectionSwap()
    {
        shuffleDirectionSwapCoroutine = ShuffleDirectionSwap();
        StartCoroutine(shuffleDirectionSwapCoroutine);
    }

    private void StopShuffleDirectionSwap()
    {
        StopCoroutine(shuffleDirectionSwapCoroutine);
        shuffleDirectionSwapCoroutine = null;
    }

    private void StartHoverDistanceSwap()
    {
        hoverDistanceSwapCoroutine = HoverDistanceSwap();
        StartCoroutine(hoverDistanceSwapCoroutine);
    }

    private void StopHoverDistanceSwap()
    {
        StopCoroutine(hoverDistanceSwapCoroutine);
        hoverDistanceSwapCoroutine = null;
    }
}