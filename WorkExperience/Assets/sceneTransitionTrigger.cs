using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameObject.Find("LoadManager").GetComponent<wipeController>().LoadScene(sceneToLoad, "none");
        }
    }
}
