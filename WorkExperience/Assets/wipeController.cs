using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wipeController : MonoBehaviour
{
    string sceneName;
    IEnumerator cor;

    public void LoadScene(string _sceneName, string _ability) // wipes to black and loads another scene
    {
        GetComponent<Animator>().SetBool("Wipe", true);
        sceneName = _sceneName;
        cor = LoadInSeconds(2f, _ability);
        StartCoroutine(cor);
    }

    IEnumerator LoadInSeconds(float _waitSeconds, string _ability)
    {
        yield return new WaitForSeconds(_waitSeconds);
        SceneManager.LoadScene(sceneName);
        GetComponent<Animator>().SetBool("Wipe", false);
        yield return new WaitForSeconds(1f);
        GetComponent<saveController>().ShowDialogue(_ability);
    }
}
