using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class TileManager : MonoBehaviour {

    private static TileManager instance = null;
    public static TileManager Instance
    {
        get
        {
            return instance;
        }
    }

    public static string Tiles = "Tiles";

    public Transform Floor;

	private GroundTile tempGroundTile;
	private int menuSize;
    private Dictionary<int, GroundTile> tiles;
	private List<GroundTile> groundTilesToOpen;
	public float timeBeforeMoving = 2f;

	private void Awake() {
        if (instance != null && instance != this)
        {
            Debug.Log("Error - Er zijn op dit moment meerdere instanties van TileManager (No Singleton)");
        }
        else
        {
            instance = this;
        }

        tiles = new Dictionary<int, GroundTile>();
		groundTilesToOpen = new List<GroundTile> (); 

		StartCoroutine(FillDictionary());
    }

    private void Start()
    {
        if (DataHolder.Instance != null)
        {
            if (DataHolder.Instance.finished)
                UpdateTileOnFinish();
        }

		menuSize = SpelMenuPlayerController.Instance.GetMenuSize ();
    }

    private IEnumerator FillDictionary()
    {
        foreach (Transform child in Floor)
        {
            GroundTile tile = child.GetComponent<GroundTile>();
            if (tile.groundTileId != 0)
            {
                tiles.Add(tile.groundTileId, tile);
            }
        }

		if (PlayerPrefs.HasKey (Tiles)) 
		{
			string json = PlayerPrefs.GetString (Tiles);
			JsonGroundTileList jsonList = JsonUtility.FromJson<JsonGroundTileList> (json);
			foreach (Transform child in Floor) 
			{
				GroundTile tile = child.GetComponent<GroundTile> ();

				foreach (JsonGroundTile jsonTile in jsonList.list) 
				{
					if (jsonTile.groundTileId == tile.groundTileId) 
					{
						tile.JsonToNormal (jsonTile);

						if (tile.achivements != null)
							ChangeSuperBadgeState (tile);
					}
				}
			}
		}

        yield return null;
    }

    public GroundTile getTileById(int id)
    {
        return (tiles.ContainsKey(id)) ? tiles[id] : null;
    }

    public void UpdateTileDoor(GroundTile tile)
    {
        tiles[tile.groundTileId] = tile;
        UpdateTilesLocal();
    }

    private void UpdateTileOnFinish()
    {
        // Change position in refactor moment
        MenuTutorialController.Instance.ShowTutorialPart(DataHolder.Instance.GetShowMenuTutorial());

        GroundTile tile = tiles[DataHolder.Instance.tileId];
        Achivement achivement = DataHolder.Instance.achivement;
  
        DataHolder.Instance.setFinished(false);

        tile.coins = (tile.coins < DataHolder.Instance.coins) ? DataHolder.Instance.coins : tile.coins;
        tile.quickestTime = (String.Compare(tile.quickestTime, DataHolder.Instance.time) > 0) ? DataHolder.Instance.time : tile.quickestTime;

        SpelMenuItemController.Instance.AddAmountOfRangerBadges(DataHolder.Instance.coins);
        tile.achivements = ChangeAchivements(achivement, tile.achivements);

        ChangeSuperBadgeState(tile);

		DataHolder.Instance.SetLocalIndexNumber ();

		AddNeigbourLevelsToList ();

		StartCoroutine (RemoveFogAtNewLevels ());

        UpdateTilesLocal();
        UpdateAnalytics(tile);
    }

    private IEnumerator RemoveFogAtNewLevels() {

		foreach (GroundTile tile in groundTilesToOpen) {
			if (!tile.isPlayable) {
				tile.CallRemoveFogFunction ();
				tile.Setplayable ();
				yield return new WaitForSecondsRealtime (timeBeforeMoving);

				SpelMenuPlayerController.Instance.SetPlayerDestination(tile);
            }
        }

		UpdateTilesLocal();

		yield return null;
	}

	private void AddNeigbourLevelsToList() {
		int groundTileId = DataHolder.Instance.GetGroundTileId ();
		GroundTile groundTile = tiles [groundTileId];
		int menuSize = SpelMenuPlayerController.Instance.GetMenuSize ();

		int tempTileId;

		if (groundTile.isTopOpen) {
			tempTileId = groundTileId - menuSize;
			groundTilesToOpen.Add(tiles[tempTileId]);
		}

		if (groundTile.isRightSideOpen) {
			tempTileId = groundTileId + 1;
			groundTilesToOpen.Add(tiles[tempTileId]);
		}

		if (groundTile.isBottomOpen) {
			tempTileId = groundTileId + menuSize;
			groundTilesToOpen.Add(tiles[tempTileId]);
		}

		if (groundTile.isLeftSideOpen) {
			tempTileId = groundTileId - 1;
			groundTilesToOpen.Add(tiles[tempTileId]);
		}
	}

    public void SetLevelKey(string levelKey)
    {
        GroundTile tile = tiles[DataHolder.Instance.tileId];
        tile.levelKey = levelKey;
    }

    public void LinkGroundTileWithScene() {
		GroundTile tile = tiles[DataHolder.Instance.tileId];

		if (tile.levelSceneName == "") {
			tile.levelSceneName = DataHolder.Instance.GetLevelSystemSceneName ();
		}

		UpdateTilesLocal ();
	}

    private void ChangeSuperBadgeState(GroundTile tile)
    {
        Achivement achivement = tile.achivements;

		// TODO: Foreach did not work --> find solution
		int counter = 0;
		if (achivement.mission1_accomplished == true) {
			counter++;
		}
		if (achivement.mission2_accomplished == true) {
			counter++;
		}
		if (achivement.mission3_accomplished == true) {
			counter++;
		}
			
		if (counter == 1)
        {
			tile.transform.GetChild(0).GetComponent<SuperBadge>().SwitchToBronze();
        }
		else if (counter == 2)
        {
			tile.transform.GetChild(0).GetComponent<SuperBadge>().SwitchToSilver();
        }
		else if (counter == 3)
        {
			tile.transform.GetChild(0).GetComponent<SuperBadge>().SwitchToGold();
        }
    }

    private Achivement ChangeAchivements(Achivement currentPlaythrough, Achivement tileAchivement)
    {
		if(tileAchivement.mission1_accomplished == false && currentPlaythrough.mission1_accomplished == true)
        {
			tileAchivement.mission1_accomplished = currentPlaythrough.mission1_accomplished;
        }

		if (tileAchivement.mission2_accomplished == false && currentPlaythrough.mission2_accomplished == true)
        {
			tileAchivement.mission2_accomplished = currentPlaythrough.mission2_accomplished;
        }

		if (tileAchivement.mission3_accomplished == false && currentPlaythrough.mission3_accomplished == true)
        {
			tileAchivement.mission3_accomplished = currentPlaythrough.mission3_accomplished;
        }

        return tileAchivement;
    }

    public void UpdateTilesLocal()
    {
        JsonGroundTileList jsonList = new JsonGroundTileList(tiles);
        string json = JsonUtility.ToJson(jsonList);
        PlayerPrefs.SetString(Tiles, json);
    }

    private void UpdateAnalytics(GroundTile tile)
    {
        Analytics.CustomEvent("FinishLevel", new Dictionary<string, object>
        {
            { "PlayerId", SystemInfo.deviceUniqueIdentifier},
            { "Level", tile.levelSceneName },
			{ "AchivementFinish", tile.achivements.mission1_accomplished },
			{ "AchivementTime", tile.achivements.mission2_accomplished },
			{ "AchivementCoins", tile.achivements.mission3_accomplished },
            { "Coins", tile.coins },
            { "MaxCoins", tile.maxCoins },
            { "QuickestTime", tile.quickestTime }
        });
    }
}
