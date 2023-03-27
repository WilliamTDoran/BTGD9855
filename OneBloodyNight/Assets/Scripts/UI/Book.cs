using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private int bookNum;
    // Start is called before the first frame update
    void Start()
    {
        bookNum = PlayerPrefs.GetInt("loreBooks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Contact");
        if (col.gameObject.tag == "Player")
        {
            bookNum++;
            PlayerPrefs.SetInt("LoreBooks", bookNum);
            Destroy(gameObject);
        }
        
    }
}
