using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterPriest : Monster
{
    [Header("Priest stuff")]
    [SerializeField]
    private LayerMask maskydoo;

    [SerializeField]
    private float range;

    [SerializeField]
    private float buffDuration;

    public override void MeleeUse()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, maskydoo);

        foreach (Collider i in hits)
        {
            if (i.tag == "Monster" && i.transform != transform)
            {
                i.gameObject.GetComponent<Monster>().StartPriestBuff(buffDuration);
            }
        }
    }
}
