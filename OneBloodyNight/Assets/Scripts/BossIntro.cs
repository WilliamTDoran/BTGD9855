using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    public GameObject BossCam;
    public GameObject PlrCam;
    private Vector3 PlayerPos;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
      PlayerPos = new Vector3(-85, -1, 15);
        PlrCam.SetActive(false);
        BossCam.SetActive(true);
        StartCoroutine("move");
        StartCoroutine("Starter");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator move()
    {
        while(BossCam.transform.position != PlayerPos) { 
        BossCam.transform.position = Vector3.MoveTowards(transform.position, PlayerPos, Time.deltaTime* speed);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator Starter()
    {
        yield return new WaitForSeconds(5);
        PlrCam.SetActive(true);
        BossCam.SetActive(false);
    }
}
