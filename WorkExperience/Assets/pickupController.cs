using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupController : MonoBehaviour
{
    public string ability;
    saveController con;

    void Start()
    {
        con = GameObject.Find("LoadManager").GetComponent<saveController>();
        if(con.hasDJump == true && ability == "dJump")
        {
            Destroy(gameObject);
        }
        if(con.hasDash == true && ability == "dash")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(ability == "dash"){
                col.gameObject.GetComponent<playerMovement>().canDash = true;
                con.hasDash = true;
            }
            else if(ability == "dJump"){
                col.gameObject.GetComponent<playerMovement>().canDJump = true;
                con.hasDJump = true;
            }
            else if(ability == "win"){
                Debug.Log("win");
            }
            GainAbility();
            gameObject.SetActive(false);
        }
    }

    void GainAbility()
    {
        con.gameObject.GetComponent<wipeController>().LoadScene("Village", ability);
    }
}
