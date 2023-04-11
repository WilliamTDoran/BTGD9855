using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private int bookNum;
    public ParticleSystem effect;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        bookNum = PlayerPrefs.GetInt("loreBooks");
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        { 
            if (bookNum < 6)
            {
            bookNum++;
            PlayerPrefs.SetInt("LoreBooks", bookNum);
            
            //PlayerPrefs.SetInt("LoreBooks", bookNum);
            
            }
            StartCoroutine("booker");

        }
        
    }

    private IEnumerator booker()
    {
        effect.Play();
        audioSource.Play();
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
