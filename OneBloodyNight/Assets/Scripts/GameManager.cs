using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/// <summary>
/// The game manager, running a variety of miscellaneous functions and holding various references for ease of access
/// 
/// Version 1.0 (2/6/2023), Will Doran
/// </summary>
public class GameManager : MonoBehaviour
{
    internal static GameManager instance;

    private static bool globalCooldownVar = false;
    private IEnumerator gcdCoroutine;

    /* Exposed Variables */
    [Tooltip("The duration in seconds of the global cooldown")]
    [SerializeField]
    private float globalCooldownDuration = 0.1f;

    [SerializeField]
    private GameObject debugCanvas;
    [SerializeField]
    private TextMeshProUGUI gcdDebugText;
    /*~~~~~~~~~~~~~~~~~~~*/

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        gcdDebugText.text = globalCooldownVar + "";

        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugCanvas.SetActive(!debugCanvas.activeInHierarchy);
        }
    }

    private IEnumerator GlobalCooldown()
    {
        globalCooldownVar = true;
        yield return new WaitForSeconds(globalCooldownDuration);
        globalCooldownVar = false;
    }

    internal bool GCD(bool start)
    {
        if ((!globalCooldownVar) && start)
        {
            StartGlobalCooldown();
            return false;
        }

        return globalCooldownVar;
    }

    internal void ResetGCD()
    {
        StopGlobalCooldown();

        globalCooldownVar = false;
    }



    private void StartGlobalCooldown()
    {
        gcdCoroutine = GlobalCooldown();
        StartCoroutine(gcdCoroutine);
    }

    private void StopGlobalCooldown()
    {
        StopCoroutine(gcdCoroutine);
        gcdCoroutine= null;
    }
}
