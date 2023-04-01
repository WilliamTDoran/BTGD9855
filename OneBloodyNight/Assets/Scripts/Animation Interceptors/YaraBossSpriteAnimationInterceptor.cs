using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class YaraBossSpriteAnimationInterceptor : MonoBehaviour
{
    [SerializeField] private YaraBoss root;
    [SerializeField] private GurgeyProjectile gurgectile;
    [SerializeField] private RemoteAttack leftHand;
    [SerializeField] private RemoteAttack rightHand;

    public void FireRocks()
    {
        root.ShockwaveFire();
    }

    public void EndSlam()
    {
        root.ShockSlamRunning = false;
    }

    public void GurgeyItUp()
    {
        gurgectile.gameObject.SetActive(true);
        gurgectile.Shoot();
    }

    public void EndGurgey()
    {
        root.GurgeyRunning = false;
    }

    public void StartPounding()
    {
        root.PoundBegin = true;
    }

    public void DiveTime()
    {
        root.PoundDive = true;
    }

    public void PoundDone()
    {
        root.GroundPoundRunning = false;
    }

    public void HandsDown()
    {
        if (UnityEngine.Random.Range(0,2) == 0)
        {
            leftHand.gameObject.SetActive(true);

            Animator leftie = leftHand.transform.GetChild(0).GetComponent<Animator>();
            leftie.Rebind();
            leftie.Update(0f);

            leftHand.InitiateConditional(Player.plr.Rb.position);
        }
        else
        {
            rightHand.gameObject.SetActive(true);

            Animator rightie = rightHand.transform.GetChild(0).GetComponent<Animator>();
            rightie.Rebind();
            rightie.Update(0f);

            rightHand.InitiateConditional(Player.plr.Rb.position);
        }
    }

    public void SwipeOver()
    {
        root.HandSwipeRunning = false;
    }
}
