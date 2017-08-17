using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	//public GameObject loadingImage;

	public StoryBoardController storyBoard;
	public DataHolder dataHolder;

	private void Start() {
        storyBoard = GameObject.Find ("InGameCanvas/StoryPanel").GetComponent<StoryBoardController>();
		dataHolder = GameObject.Find ("DataHolder").GetComponent<DataHolder> ();

		StartNewGame ();
	}

	public void StartNewGame() {

        // TODO: Animatie topview camera naar FPV camera toevoegen (@Tim)

        storyBoard.LoadStory (dataHolder.storyId);

        storyBoard.gameObject.SetActive(true);
	}
}
