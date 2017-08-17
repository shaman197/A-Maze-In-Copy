using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTutorialController : MonoBehaviour {

    private static MenuTutorialController instance = null;
    public static MenuTutorialController Instance {
		get {
			return instance;
		}
	}

    public static string TutorialsName = "Tutorials";

    public GameObject[] panels;
    public int[] showNextPanelAtElement;
    public GameObject clickToCloseText;

    private Dictionary<int, MenuTutorial> tutorials;
    private bool active;
    private MenuTutorial currentTutorial;
    private GameObject currentPanel;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van MenuTutorialController (No Singleton)");
		} 
		else {
			instance = this;
		}
	}

	private void Start() {
		tutorials = new Dictionary<int, MenuTutorial> ();
		StartCoroutine (FillSeenTutorials ());
	}

    private void FixedUpdate () {
		if (Input.anyKeyDown && active && currentTutorial.showNextTutorial) {
			currentPanel.SetActive (false);
			ShowTutorialPart (currentTutorial.tutorialId + 1);
		} 
		else if (Input.anyKeyDown && active) {
			currentPanel.SetActive (false);
			clickToCloseText.SetActive (false);
			active = false;
		}
	}

    private IEnumerator FillSeenTutorials() {
		if (PlayerPrefs.HasKey (TutorialsName)) {
			string json = PlayerPrefs.GetString (TutorialsName);
			JsonMenuTutorialList jsonList = JsonUtility.FromJson<JsonMenuTutorialList> (json);
			foreach (MenuTutorial item in jsonList.list) {
				tutorials.Add (item.tutorialId, item);
			}
		} 
		else {
			int count = 0;
			foreach (GameObject panel in panels) {
				if (panel != null) {
					bool showNextPanel = NextPanelException (count);

					tutorials.Add (count, new MenuTutorial (count, false, panel.name, showNextPanel));
					count++;
				}
			}

			tutorials [0].alreadySeen = true;
			panels [0].SetActive (true);
			UpdateTutorialLocal ();
		}

		yield return null;
	}

    public void ShowTutorialPart(int partCount) {
		if (tutorials.ContainsKey (partCount)) {
			currentTutorial = tutorials [partCount];
			if (!currentTutorial.alreadySeen) {
				tutorials [partCount].alreadySeen = true;
				ShowPanel (currentTutorial.panelName);
				clickToCloseText.SetActive (true);
				active = true;

				UpdateTutorialLocal ();
			}
		}
	}

    private void ShowPanel(string showPanel) {
		foreach (GameObject panel in panels) {
			if (panel != null) {
				if (showPanel == panel.name) {
					currentPanel = panel;
					currentPanel.SetActive (true);
				}
			}
		}
	}

    private bool NextPanelException(int id) {
		foreach (int elementNumber in showNextPanelAtElement) {
			if (elementNumber != 0) {
				if (elementNumber == id) {
					return true;
				}
			}
		}

		return false;
	}

    public void UpdateTutorialLocal() {
        JsonMenuTutorialList jsonList = new JsonMenuTutorialList(tutorials);
        string json = JsonUtility.ToJson(jsonList);
        PlayerPrefs.SetString(TutorialsName, json);
    }
}