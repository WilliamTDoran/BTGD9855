using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockrock : Attack
{
    private const float FLOAT_DIST = 9;
    private Vector3 direction;

    /* Exposed Variables */
    [SerializeField]
    private YaraBoss root;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            transform.Translate(direction * Time.deltaTime);
        }
    }

    internal override void Fire() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">0 is -z, counting upward clockwise</param>
    /// <param name="timeScale"></param>
    internal void Fire(int position, float timeScale)
    {
        Vector3 fireLocale = Positioner(position);
        transform.position = fireLocale;

        direction *= speed / timeScale;

        canMove = true;
    }

    private Vector3 Positioner(int position)
    {
        Vector3 fireLocale = root.Rb.position;

        switch(position)
        {
            case 0:
                fireLocale += new Vector3(0, 0, -FLOAT_DIST);
                direction = new Vector3(0, 0, -1);
                break;
            case 1:
                fireLocale += new Vector3(-1, 0, -1).normalized * FLOAT_DIST;
                direction = new Vector3(-1, 0, -1).normalized;
                break;
            case 2:
                fireLocale += new Vector3(-FLOAT_DIST, 0, 0);
                direction = new Vector3(-1, 0, 0);
                break;
            case 3:
                fireLocale += new Vector3(-1, 0, 1).normalized * FLOAT_DIST;
                direction = new Vector3(-1, 0, 1).normalized;
                break;
            case 4:
                fireLocale += new Vector3(0, 0, FLOAT_DIST);
                direction = new Vector3(0, 0, 1);
                break;
            case 5:
                fireLocale += new Vector3(1, 0, 1).normalized * FLOAT_DIST;
                direction = new Vector3(1, 0, 1).normalized;
                break;
            case 6:
                fireLocale += new Vector3(FLOAT_DIST, 0, 0);
                direction = new Vector3(1, 0, 0);
                break;
            case 7:
                fireLocale += new Vector3(1, 0, -1).normalized * FLOAT_DIST;
                direction = new Vector3(1, 0, -1);
                break;
        }

        return fireLocale;
    }

    internal override void OnHitWall()
    {
        base.OnHitWall();

        FinishFlight();
    }

    internal void FinishFlight()
    {
        EndSwing();
        canMove = false;
        gameObject.SetActive(false);
    }
}
