using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelMenuPlayerController : MonoBehaviour {

	private static SpelMenuPlayerController instance = null;
	public static SpelMenuPlayerController Instance {
		get {
			return instance;
		}
	}

	public const int SpelMenuSize = 4;
	public GroundTile groundTile;
	public Transform currentGroundTile;
	public int currentGroundTileId;
	public Transform spelMenuFloors;
	public Transform player;

	public Vector3 desiredPosition;
	public GroundTile destinationTile;
	public bool isMoving;
	public int speed = 20;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van SpelMenuPlayerController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

    private void Start() {
        player = this.transform;

        if (DataHolder.Instance != null) {
            player.position = DataHolder.Instance.playerPosition;
            currentGroundTileId = DataHolder.Instance.tileId;
            groundTile = TileManager.Instance.getTileById(currentGroundTileId);
        }
		// Code for debugging without DataHolder
        else {
            groundTile = currentGroundTile.GetComponent<GroundTile>();
            currentGroundTileId = groundTile.groundTileId;
			player.position = TileManager.Instance.getTileById(2).transform.position;
        }

		desiredPosition = player.position;
    }

	private void Update() {
		if (transform.position != desiredPosition) {
			transform.position = Vector3.Lerp (transform.position, desiredPosition, Time.deltaTime * speed);

			// Already reached destination?
			if (transform.position == desiredPosition) {
				SetFloorInfo (destinationTile.transform);
				ShowFloorInfo ();

				if (DataHolder.Instance == null) {
					Debug.Log ("Er is op dit moment geen DataHolder in de game | Start de game vanaf het startscherm voor een dataHolder");
				} 
				else {
					UpdateDataHolderStoryId ();
					UpdateDataHolderLevelSystemMazeId ();
					UpdateDataholderGroundTileId ();
					UpdateDataholderMaxCoins ();
					UpdateDataholderAchivementTimeInSec ();
					UpdateDataholderPlayerPosition ();
					UpdateLevelKey();
				}

				isMoving = false;
			}
		}
	}

	public int GetMenuSize() {
		return SpelMenuSize;
	}

	public void CheckForValidMove(string direction) {
		int nextTileId;
		bool isInfoPanelAnimPlaying = InfoPanelController.Instance.anim.isPlaying;

		if (direction == "upwards" && !isMoving && !isInfoPanelAnimPlaying) {
			if (groundTile.isTopOpen) {
				nextTileId = currentGroundTileId - SpelMenuSize;
				GroundTile tile = TileManager.Instance.getTileById (nextTileId);
				if (tile.isPlayable) {
					isMoving = true;
					InfoPanelController.Instance.StartLevelChangeAnimation ();
					desiredPosition = tile.transform.position;
					destinationTile = tile;
				}
			}
		}
		else if (direction == "rightwards" && !isMoving && !isInfoPanelAnimPlaying) {
			if (groundTile.isRightSideOpen) {
				nextTileId = currentGroundTileId + 1;
				GroundTile tile = TileManager.Instance.getTileById (nextTileId);
				if (tile.isPlayable) {
					isMoving = true;
					InfoPanelController.Instance.StartLevelChangeAnimation ();
					desiredPosition = tile.transform.position;
					destinationTile = tile;
				}
			}
		}
		else if (direction == "downwards" && !isMoving && !isInfoPanelAnimPlaying) {
			if (groundTile.isBottomOpen) {
				nextTileId = currentGroundTileId + SpelMenuSize;
				GroundTile tile = TileManager.Instance.getTileById (nextTileId);
				if (tile.isPlayable) {
					isMoving = true;
					InfoPanelController.Instance.StartLevelChangeAnimation ();
					desiredPosition = tile.transform.position;
					destinationTile = tile;
				}
			}
		}
		else if (direction == "leftwards" && !isMoving && !isInfoPanelAnimPlaying) {
			if (groundTile.isLeftSideOpen) {
				nextTileId = currentGroundTileId - 1;
				GroundTile tile = TileManager.Instance.getTileById (nextTileId);
				if (tile.isPlayable) {
					isMoving = true;
					InfoPanelController.Instance.StartLevelChangeAnimation ();
					desiredPosition = tile.transform.position;
					destinationTile = tile;
				}
			}
		}
	}

	public void SetPlayerDestination(GroundTile tile) {
		desiredPosition = tile.transform.position;
		destinationTile = tile;
	}

    private void SetFloorInfo(Transform floor) {
		currentGroundTile = floor.transform;
		groundTile = currentGroundTile.GetComponent<GroundTile> ();
		currentGroundTileId = groundTile.groundTileId;
	}

	private void ShowFloorInfo() {
		InfoPanelController.Instance.ShowEarnedMedals(groundTile);
	}

	private void UpdateDataHolderStoryId() {
		DataHolder.Instance.SetStoryId (groundTile.storyId);
	}

	private void UpdateDataHolderLevelSystemMazeId() {
		DataHolder.Instance.SetLevelSceneName (groundTile.levelSceneName);
	}

    private void UpdateDataholderGroundTileId() {
        DataHolder.Instance.SetTileId(groundTile.groundTileId);
    }

    private void UpdateDataholderMaxCoins() {
        DataHolder.Instance.SetMaxCoins(groundTile.maxCoins);
    }

    private void UpdateDataholderAchivementTimeInSec() {
        DataHolder.Instance.SetAchivementTimeInSec(groundTile.achivementTimeInSec);
    }

    private void UpdateAchivements() {
        DataHolder.Instance.SetAchivements(groundTile.achivements);
    }

    private void UpdateDataholderPlayerPosition() {
        DataHolder.Instance.SetPlayerPosition(player.position);
    }

    private void UpdateLevelKey()
    {
        DataHolder.Instance.SetLevelKey(groundTile.levelKey);
    }
}