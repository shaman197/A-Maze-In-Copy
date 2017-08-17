using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[Serializable]
public class PlayerData {

    public string playerName;
	public int age;
	public Gender gender;

    public PlayerData(string playerName, int age, Gender gender) {
        this.playerName = playerName;
        this.age = age;
        this.gender = gender;
    }

    public PlayerData() {
        this.playerName = null;
        this.age = 0;
        this.gender = Gender.Unknown;
    }
}

[Serializable]
public class JsonPlayerDataList {
    public List<PlayerData> list;

    public JsonPlayerDataList(PlayerData player) {
        list = new List<PlayerData>();
        list.Add(player);
    }
}