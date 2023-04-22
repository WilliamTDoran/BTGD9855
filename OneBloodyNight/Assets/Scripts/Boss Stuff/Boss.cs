using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : GameActor
{
    protected IEnumerator randomBehaviorCoroutine;
    protected IEnumerator randomAttackingCoroutine;

    protected Vector3 faceDirection;

    protected Vector3[] cardinals = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

    protected System.Random rnd;
    protected int rndCap = 3;

    protected float timeModifier = 1.0f;
    public float TimeModifier { get { return timeModifier; } }

    internal static Boss instance;

    /* Exposed Variables */
    [Header("Boss")]
    [Tooltip("The number of phases. The value of each should be the health percent when the boss will ENTER the phase")]
    [SerializeField]
    protected float[] phases;

    [Tooltip("Current phase (Only change manually for testing, always change back afterward)")]
    [SerializeField]
    protected int currentPhase = 0;

    [Tooltip("Multiplier applied to timings per phase")]
    [SerializeField]
    protected float timeShred = 0.8f;

    [Tooltip("How long is too long to hide in the sauce")]
    [SerializeField]
    protected float tooLong = 1.5f;
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

        timeModifier = Mathf.Pow(timeShred, currentPhase);
    }

    private void FixedUpdate()
    {
        if (canMove && !stunned)
        {
            rb.AddForce(faceDirection, ForceMode.Force);
        }
    }

    private IEnumerator RandomBehavior()
    {
        StartRandomAttacking();

        while (true)
        {
            canMove = true;
            yield return new WaitForFixedUpdate();
            faceDirection = PickDirection() * speed / timeModifier;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.8f, 1.5f));
            canMove = false;
            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.8f));
        }
    }

    protected virtual IEnumerator RandomAttacking() { yield return new WaitForEndOfFrame(); }

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
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Destroy(gameObject);
    }

    protected Vector3 PickDirection()
    {
        Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));

        foreach (Vector3 i in cardinals)
        {
            float distToWall = WallMeasurement(i);
            if (distToWall > 10.0f)
            {
                direction += i * 2;
            }
        }

        direction.Normalize();
        return direction;
    }

    private float WallMeasurement(Vector3 direction)
    {
        int layerMask = 1 << 6;
        RaycastHit info = new RaycastHit();

        if (Physics.Raycast(rb.position, direction, out info, 1000, layerMask))
        {
            return info.distance;
        }

        return 1000f;
    }


    protected void StartRandomBehavior()
    {
        randomBehaviorCoroutine = RandomBehavior();
        StartCoroutine(randomBehaviorCoroutine);
    }

    protected void StopRandomBehavior()
    {
        StopCoroutine(randomBehaviorCoroutine);
        randomBehaviorCoroutine = null;
    }

    protected void StartRandomAttacking()
    {
        randomAttackingCoroutine = RandomAttacking();
        StartCoroutine(randomAttackingCoroutine);
    }

    protected void StopRandomAttacking()
    {
        StopCoroutine(randomAttackingCoroutine);
        randomAttackingCoroutine = null;
    }
}
