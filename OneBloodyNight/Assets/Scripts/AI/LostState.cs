using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LostState : FSMState
{
    private UniversalAIProperties universalAIProperties;
    private LostAIProperties lostAIProperties;
    private MonsterControllerAI monster;

    public LostState(MonsterControllerAI controller, UniversalAIProperties universalAIProperties, LostAIProperties lostAIProperties)
    {
        this.universalAIProperties = universalAIProperties;
        this.lostAIProperties =      lostAIProperties;
        monster = controller;
        stateID = FSMStateID.Lost;
    }

    public override void EnterStateInit(Transform player, Transform npc)
    {
        //chasing is deliberately NOT changed here; it's done by the LoseSight coroutine
        lostAIProperties.lost = false;
        monster.StartLoseSight();
    }

    public override void Reason(Transform player, Transform npc)
    {
        Debug.Log(npc.gameObject.name + " has lost sight of " + player.gameObject.name);

        if (Player.plr.Visible && !universalAIProperties.host.WallCheck())
        {
            monster.StopLoseSight();
            monster.PerformTransition(Transition.Spot);
            return;
        }

        if (lostAIProperties.lost)
        {
            monster.StopLoseSight();
            monster.PerformTransition(Transition.GiveUp);
            return;
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        
    }
}
