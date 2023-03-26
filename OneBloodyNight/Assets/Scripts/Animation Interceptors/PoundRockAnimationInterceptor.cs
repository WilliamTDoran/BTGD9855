using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoundRockAnimationInterceptor : MonoBehaviour
{
    [SerializeField] private Attack root;

    public void Active()
    {
        root.Col.enabled = true;
    }

    public void Done()
    {
        root.Col.enabled = false;
        root.EndSwing();
        root.gameObject.SetActive(false);
    }
}
