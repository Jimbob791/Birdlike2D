using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class saveController : MonoBehaviour
{
    public bool hasDash = false;
    public bool hasDJump = false;
    public bool floorBroken = false;
    
    [SerializeField]
    private GameObject panelObj;

    public static GameObject tested;

    void Awake()
    {
        if (tested == null)
        {
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
            tested = gameObject.transform.parent.gameObject;
            panelObj = GameObject.Find("Dialogue Panel");
            ShowDialogue("start");
        }
        else if (tested != gameObject.transform.parent.gameObject)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void ShowDialogue(string _abToShow)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name != "Village")
        {
            return;
        }

        if(_abToShow == "none")
        {
            return;
        }

        panelObj.SetActive(true);

        if(_abToShow == "start")
        {   
            GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>().text = "What an evening... Where am I?              Arrow Keys to Move                              Escape your memories";
        }

        if(_abToShow == "dash")
        {   
            GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>().text = "Havent I- Have I been here before?          I can now dash using C";
        }

        if(_abToShow == "dJump")
        {   
            GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>().text = "What- what IS this? Why am I here again?    I can now double jump";
        }

        if(_abToShow == "win")
        {   
            GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>().text = "I- I'm free!                                Feel free to explore your memories";
        }
        StartCoroutine(hideText());
    }

    IEnumerator hideText()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.UpArrow));
        panelObj.SetActive(false);
    }
}
