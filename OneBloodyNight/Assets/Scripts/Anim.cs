using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    public GameActor actor;
    [SerializeField]
    public Attack atk;
    [SerializeField]
    public Monster monster;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
