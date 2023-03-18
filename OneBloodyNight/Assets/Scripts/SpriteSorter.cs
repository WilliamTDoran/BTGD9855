using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public int defaultSortingOrder = 1;
    public int triggeredSortingOrder = -1;

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpriteTrigger"))
        {
            triggerCount--;
            if (triggerCount <= 0)
            {
            spriteRenderer.sortingOrder = defaultSortingOrder;
            }
        }
    }
}
