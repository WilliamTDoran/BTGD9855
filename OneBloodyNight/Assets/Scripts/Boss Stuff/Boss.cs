using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : GameActor
{
    


    protected System.Random rnd;
    protected int rndCap = 3;

    internal static Boss instance;

    /* Exposed Variables */
    [Header("Boss")]
    [Tooltip("The number of phases. The value of each should be the health percent when the boss will ENTER the phase")]
    [SerializeField]
    protected float[] phases;

    [Tooltip("Current phase (Only change manually for testing, always change back afterward)")]
    [SerializeField]
    protected int currentPhase = 0;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Awake()
    {
        base.Awake();
        
        CurHitPoints = MaxHitPoints;

        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    protected override void Start()
    {
        base.Start();

        rnd = new System.Random();
        StartCoroutine(PhaseCheck());
    }

    protected override void Update()
    {
        base.Update();
        
        if (CurHitPoints <= 0)
        {
            //StartCoroutine("Ded");
            
        }
    }

    private IEnumerator PhaseCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float healthPercent = (float)CurHitPoints / (float)MaxHitPoints;
            if (currentPhase + 1 < phases.Length && healthPercent <= phases[currentPhase + 1])
            {
                currentPhase++;
            }
        }
    }

    private IEnumerator Ded()
    {
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("Die");
        Destroy(gameObject);
    }


}
