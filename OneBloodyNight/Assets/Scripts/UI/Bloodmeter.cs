using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bloodmeter : MonoBehaviour
{
    public Slider bloodmeter;
    private int currentBlood;
    public int AbilityCost;
    public int damage;
    public Slider residual;


    public Animator anim;

    public GameObject gameover;

    [SerializeField]
    internal float bloodLossRate;

    public float bloodGainRate;

    internal static Bloodmeter instance;

    internal float invisLoss = 2;

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

    // Start is called before the first frame update
    private void Start()
    {
        //cost1.SetActive(false);

        bloodmeter.value = bloodmeter.maxValue;
        residual.value = bloodmeter.minValue;
        Debug.Log("Done");
        StartCoroutine("DMG");
        gameover.SetActive(false);
    }

    private void Update()
    {
        


        if (Input.GetKeyDown(KeyCode.I))
        {
            //StopCoroutine("DMG");
            StartCoroutine("Gainer");
        }


    }


    // Update is called once per frame
    /*private void Update()
    {
        if (Input.GetButtonDown("AdvancedFire"))
        {
            bloodmeter.value = bloodmeter.value - AbilityCost;
        }
    }*/

    private IEnumerator DMG()
    {

        while (bloodmeter.value > bloodmeter.minValue)
        {
            float bloodLost = bloodLossRate;
            if (Player.plr.GetComponent<PlayerStrigoi>() != null && Player.plr.abilityOneCost == 0 && !Player.plr.GetComponent<PlayerStrigoi>().Visible)
            {
                bloodLost *= 2;
            }
            bloodmeter.value = bloodmeter.value - bloodLost;//takes 1 blood per second

            yield return new WaitForSeconds(0.03f);//slows down the damage rate

        }
        yield return null;//Player is dead
    }

    private IEnumerator Rez()
    {
        residual.value = bloodmeter.value;

        while (residual.value != bloodmeter.value)
        {
            residual.value = residual.value - 1;
            yield return new WaitForSeconds(0.1f);
        }
        residual.value = bloodmeter.value;

    }

    internal void changeBlood(float difference)
    {
        float targetValue = bloodmeter.value;
        

        targetValue = Math.Clamp(targetValue + difference, 0, bloodmeter.maxValue);

        bloodmeter.value = targetValue;

        //StartCoroutine("Rez");
    }

    private IEnumerator Gain()
    {

        while (bloodmeter.value > bloodmeter.minValue)
        {
            bloodmeter.value = bloodmeter.value + bloodGainRate;//takes 1 blood per second
            yield return new WaitForSeconds(0.03f);//slows down the damage rate
        }
        yield return null;//Player is dead
    }

    private IEnumerator Gainer()
    {

        while (bloodmeter.value < bloodmeter.maxValue)
        {
            bloodmeter.value = bloodmeter.value + 10;//takes 1 blood per second
            yield return new WaitForSeconds(0.03f);//slows down the damage rate
        }

    }
    public void Hort()
    {
        anim.SetTrigger("Hit");
    }


}
