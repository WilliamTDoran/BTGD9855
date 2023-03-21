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
    internal static GameManager instance; //static reference

    private bool debugTextStatus = false;
    public bool DebugTextStatus { get { return debugTextStatus; } }

    private static bool globalCooldownVar = false; //whether the global cooldown is active. true means it is active
    private IEnumerator gcdCoroutine; //coroutine to drive gcd

    /* Exposed Variables */
    [Tooltip("The duration in seconds of the global cooldown")]
    [SerializeField]
    private float globalCooldownDuration = 0.1f;

    [Tooltip("The canvas containing debug text")]
    [SerializeField]
    private GameObject debugCanvas;

    [Tooltip("The debug text for indicating gcd activity")]
    [SerializeField]
    private TextMeshProUGUI gcdDebugText;

    [Tooltip("An impundulu boss raincloud")]
    [SerializeField]
    private Raincloud raincloud;
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard awake, just used for creating a static reference.
    /// </summary>
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

    /// <summary>
    /// Standard Update. Handles debug stuff.
    /// </summary>
    private void Update()
    {
        gcdDebugText.text = globalCooldownVar + "";

        if (Input.GetKeyDown(KeyCode.F1)) //debug text is toggleable
        {
            debugCanvas.SetActive(!debugCanvas.activeInHierarchy);
            debugTextStatus = !debugTextStatus;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (raincloud != null)
            {
                raincloud.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Application.LoadLevel("ImpunduluBossRoom");
        }
    }

    /// <summary>
    /// Runs the global cooldown countdown. Doesn't actually *do* anything on its own other than 'be a timer'
    /// </summary>
    /// <returns>Functional IEnumerator return</returns>
    private IEnumerator GlobalCooldown()
    {
        globalCooldownVar = true;
        yield return new WaitForSeconds(globalCooldownDuration);
        globalCooldownVar = false;
    }

    /// <summary>
    /// Accessible call to the GCD, which also runs it if called for.
    /// </summary>
    /// <param name="start">Whether this call should activate the GCD if it's not already active. True is yes it should</param>
    /// <returns>Whether the GCD is active at call-time (so if wasn't active, and this function starts it, it still returns false)</returns>
    internal bool GCD(bool start)
    {
        if ((!globalCooldownVar) && start)
        {
            StartGlobalCooldown(); //If called for, this function both starts the GCD and returns its call-time state.
            return false;
        }

        return globalCooldownVar;
    }

    /// <summary>
    /// Currently not used, but this is here in case anything is added which clears the GCD time.
    /// </summary>
    internal void ResetGCD()
    {
        StopGlobalCooldown();

        globalCooldownVar = false;
    }


    //~~The Coroutine Section~~
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
