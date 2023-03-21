using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AIProperties 
{
}

[System.Serializable]
public class UniversalAIProperties : AIProperties
{
    [Tooltip("How far the monster will swap to attacking from")]
    public float engagementDistance;

    [Tooltip("Whether the monster will attempt to locate the player even if it can't see them")]
    public bool seeThroughWalls;

    [Tooltip("Reference to the Monster component")]
    public Monster host;
}

[System.Serializable]
public class PatrolAIProperties : AIProperties
{
    public float spottyDistance;
    public float despawnDistance;
}

[System.Serializable]
public class ChaseAIProperties : AIProperties
{
    public float despawnDistance;
}

[System.Serializable]
public class LostAIProperties : AIProperties
{
    internal bool lost = false;

    [Tooltip("Time in seconds until the monster stops chasing after losing line of sight")]
    public float timeTilLoseSight = 0.5f;
}

[System.Serializable]
public class AttackAIProperties : AIProperties
{
    [Tooltip("How close the monster needs to get to attack")]
    public float attackRange;

    [Tooltip("How far the monster will try to stay away from its comrades")]
    public float avoidanceRange;
}

public class MonsterControllerAI : AdvancedFSM
{
    private IEnumerator loseSightCoroutine;

    [SerializeField]
    private UniversalAIProperties universalAIProperties;
    [SerializeField]
    private PatrolAIProperties patrolAIProperties;
    [SerializeField]
    private ChaseAIProperties chaseAIProperties;
    [SerializeField]
    private LostAIProperties lostAIProperties;
    [SerializeField]
    private AttackAIProperties attackAIProperties;
    
    protected override void Initialize()
    {
        GameObject objPlayer = Player.plr.gameObject;
        playerTransform = objPlayer.transform;
        ConstructFSM();
    }

    protected override void FSMFixedUpdate()
    {

        if (CurrentState != null)
        {
            //CurrentState.Reason(playerTransform, transform);
            CurrentState.Act(playerTransform, transform);
        }
    }

    private void ConstructFSM()
    {
        //Create States

        //Create the Patrol state
        PatrolState patrolState = new PatrolState(this, universalAIProperties, patrolAIProperties);
        //Add transitions OUT of the patrol state
        patrolState.AddTransition(  Transition.Spot,        FSMStateID.Chase);

        //Create the Chase state
        ChaseState chaseState = new ChaseState(this, universalAIProperties, chaseAIProperties);
        //Add transitions OUT of the chase state
        chaseState.AddTransition(   Transition.LoseSight,   FSMStateID.Lost);
        chaseState.AddTransition(   Transition.Reach,       FSMStateID.Attack);

        //Create the Lost state
        LostState lostState = new LostState(this, universalAIProperties, lostAIProperties);
        //Add transitions OUT of the lost state
        lostState.AddTransition(    Transition.GiveUp,      FSMStateID.Patrol);
        lostState.AddTransition(    Transition.Spot,        FSMStateID.Chase);

        //Create the Attack state
        AttackState attackState = new AttackState(this, universalAIProperties, attackAIProperties);
        //Add transitions OUT of the attack state
        attackState.AddTransition(  Transition.TooFar,      FSMStateID.Chase);

        //Add all states to the state list
        AddFSMState(patrolState);
        AddFSMState(chaseState);
        AddFSMState(lostState);
        AddFSMState(attackState);
    }

    private IEnumerator LoseSight()
    {
        yield return new WaitForSeconds(lostAIProperties.timeTilLoseSight);
        universalAIProperties.host.Chasing = false;
        lostAIProperties.lost = true;
    }


    internal void StartLoseSight()
    {
        loseSightCoroutine = LoseSight();
        StartCoroutine(loseSightCoroutine);
    }

    internal void StopLoseSight()
    {
        StopCoroutine(loseSightCoroutine);
        loseSightCoroutine = null;
    }
}
