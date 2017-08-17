using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundTile : MonoBehaviour {

	public int groundTileId;
	public int storyId;
	public string levelSceneName = "";
	public string levelKey;
	public int coins;
	public int maxCoins;
    public float achivementTimeInSec;
	public string quickestTime;
    public Achivement achivements;
	public GameObject fogCloud;
	public SpelMenuCloud cloudOnTile;
    public bool isTopOpen;
	public bool isRightSideOpen;
	public bool isBottomOpen;
	public bool isLeftSideOpen;
	public bool isPlayable;

	private void Start() {
		// StartLevel has no Fog
		if (transform.name != "StartLevel") {
			fogCloud = transform.FindChild ("FogCloud").gameObject;
			cloudOnTile = fogCloud.GetComponent<SpelMenuCloud> ();

			if (isPlayable) {
				fogCloud.SetActive (false);
			}
		}
	}

	public void Setplayable() {
		isPlayable = true;
	}

	public void CallRemoveFogFunction() {
		cloudOnTile.RemoveFog();
	}

    public void JsonToNormal(JsonGroundTile json)
    {
        this.groundTileId = json.groundTileId;
		this.levelSceneName = json.levelSceneName;
        this.levelKey = json.levelKey;
        this.coins = json.coins;
        this.quickestTime = json.quickestTime;
        this.achivements = json.achivements;
        this.isTopOpen = json.isTopOpen;
        this.isRightSideOpen = json.isRightSideOpen;
        this.isBottomOpen = json.isBottomOpen;
        this.isLeftSideOpen = json.isLeftSideOpen;
		this.isPlayable = json.isPlayable;
    }
}

[Serializable]
public class JsonGroundTile
{
    public int groundTileId;
    public int storyId;
	public string levelSceneName;
    public string levelKey;
    public int coins;
    public int maxCoins;
    public float achivementTimeInSec;
    public string quickestTime;
    public Achivement achivements;

    public bool isTopOpen;
    public bool isRightSideOpen;
    public bool isBottomOpen;
    public bool isLeftSideOpen;

	public bool isPlayable;

	public JsonGroundTile(int groundTileId, int storyId, string levelSceneName, string levelKey, int coins, int maxCoins, float achivementTimeInSec, string quickestTime, Achivement achivements, bool isTopOpen, bool isRightSideOpen, bool isBottomOpen, bool isLeftSideOpen, bool isPlayable)
    {
        this.groundTileId = groundTileId;
        this.storyId = storyId;
		this.levelSceneName = levelSceneName;
        this.levelKey = levelKey;
        this.coins = coins;
        this.maxCoins = maxCoins;
        this.achivementTimeInSec = achivementTimeInSec;
        this.quickestTime = quickestTime;
        this.achivements = achivements;
        this.isTopOpen = isTopOpen;
        this.isRightSideOpen = isRightSideOpen;
        this.isBottomOpen = isBottomOpen;
        this.isLeftSideOpen = isLeftSideOpen;
		this.isPlayable = isPlayable;
    }
}

[Serializable]
public class JsonGroundTileList
{
    public List<JsonGroundTile> list;

    public JsonGroundTileList(Dictionary<int, GroundTile> list)
    {
        List<JsonGroundTile> tempList = new List<JsonGroundTile>();
        foreach (KeyValuePair<int, GroundTile> tile in list)
        {
            tempList.Add(new JsonGroundTile(
                list[tile.Key].groundTileId,
                list[tile.Key].storyId,
				list[tile.Key].levelSceneName,
                list[tile.Key].levelKey,
                list[tile.Key].coins,
                list[tile.Key].maxCoins,
                list[tile.Key].achivementTimeInSec,
                list[tile.Key].quickestTime,
                list[tile.Key].achivements,
                list[tile.Key].isTopOpen,
                list[tile.Key].isRightSideOpen,
                list[tile.Key].isBottomOpen,
                list[tile.Key].isLeftSideOpen,
				list[tile.Key].isPlayable)
            );
        }

        this.list = tempList;
    }
}