using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelController : MonoBehaviour {

    public GameObject uiFailed;
    public GameObject uiFinished;

    private LoadManager loadManager;

    private void Start() {
		loadManager = transform.GetComponent<LoadManager>();
    }

    public void ShowFailed() {
        uiFailed.SetActive(true);
    }

    public void HideFailed()
    {
        uiFailed.SetActive(false);
    }

    public void ShowFinished() {
        uiFinished.SetActive(true);
    }

    public void BackToMenu() {
        loadManager.LoadScene("SpelMenu");
    }

//    public void PlayAgain(string sceneName) {
//        loadManager.LoadScene(sceneName);
//    }
}
