using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MenuTutorial {
    public int tutorialId;
    public bool alreadySeen;
    public string panelName;
    public bool showNextTutorial;

    public MenuTutorial(int tutorialId, bool alreadySeen, string panelName, bool showNextTutorial) {
        this.tutorialId = tutorialId;
        this.alreadySeen = alreadySeen;
        this.panelName = panelName;
        this.showNextTutorial = showNextTutorial;
    }
}

[Serializable]
public class JsonMenuTutorialList {
    public List<MenuTutorial> list;

    public JsonMenuTutorialList(Dictionary<int, MenuTutorial> dictonary) {
		list = new List<MenuTutorial> ();

		foreach (KeyValuePair<int, MenuTutorial> item in dictonary) {
			list.Add (item.Value);
		}
	}
}