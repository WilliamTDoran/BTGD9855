using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public int defaultSortingOrder = 20;
    public int triggeredSortingOrder = -1;
    public int propSortingOrder = 10;

    private int triggerCount = 0;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = defaultSortingOrder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpriteTrigger"))
        {
            triggerCount++;
            if (triggerCount > 0)
            {
                spriteRenderer.sortingOrder = triggeredSortingOrder;
            }
        }

        if (other.CompareTag("PropSpriteTrigger"))
        {
            triggerCount++;
            if (triggerCount > 0)
            {
                spriteRenderer.sortingOrder = propSortingOrder;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpriteTrigger") || other.CompareTag("PropSpriteTrigger"))
        {
            triggerCount--;
            if (triggerCount <= 0)
            {
            spriteRenderer.sortingOrder = defaultSortingOrder;
            }
        }
    }


}
