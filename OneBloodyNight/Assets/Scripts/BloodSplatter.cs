using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 DeathPos;
    public GameObject Blood;
    private bool StartDeath = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ded()
    {
        StartDeath = true;
        DeathPos = gameObject.transform.position;
        Instantiate(Blood, DeathPos, Quaternion.identity);
    }
}
