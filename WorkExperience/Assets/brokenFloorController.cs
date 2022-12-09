using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brokenFloorController : MonoBehaviour
{
    bool isBroken = false;
    public bool preBroken = false;

    [SerializeField]
    private Collider2D boxCol;

    void Start()
    {
        preBroken = GameObject.Find("LoadManager").GetComponent<saveController>().floorBroken;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && !isBroken && !preBroken)
        {
            StartCoroutine(BreakFloor());
        }
    }
    
    IEnumerator BreakFloor()
    {
        yield return new WaitForSeconds(0.5f);

        GetComponent<Animator>().SetBool("broken", true);
        isBroken = true;
        boxCol.enabled = false;
        GameObject.Find("LoadManager").GetComponent<saveController>().floorBroken = true;
    }
}
