using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossMeter : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider bloodmeter;
    public GameObject BossText;
    public GameObject Bossblood;
    public GameObject PlrBlood;

    public GameObject BossPortal;
    public int counter;
    void Awake()
    {
        
    }
    void Start()
    {
        Bossblood.SetActive(false);
        BossText.SetActive(false);
        PlrBlood.SetActive(false);
        StartCoroutine("Starter");
        BossPortal.SetActive(false);
        bloodmeter.value = bloodmeter.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        bloodmeter.value = Boss.instance.CurHitPoints;
        if (Boss.instance.CurHitPoints <= 0)
        {
            StartCoroutine("waiter");
            //BossPortal.SetActive(true);

            
            
        }
    }
    
    private IEnumerator Starter()
    {
        
        yield return new WaitForSeconds(6);
        Bossblood.SetActive(true);
        BossText.SetActive(true);
        PlrBlood.SetActive(true);
    }

    private IEnumerator waiter()
    {

        yield return new WaitForSeconds(2);
        BossPortal.SetActive(true);
    }
}
