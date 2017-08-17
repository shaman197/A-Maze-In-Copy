using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

    public GameObject loadingImage;
    public Slider loadingBar;

    private AsyncOperation async;

    public void LoadScene(string sceneName)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(sceneName));
    }

    public void SaveAndLoadGameScene()
    {
        StartCoroutine(LevelSystemLoadScene());
    }

    private IEnumerator LevelSystemLoadScene()
    {
        string mazeLevelSceneName = DataHolder.Instance.GetLevelSystemSceneName();

        if (mazeLevelSceneName == "")
        {
            int levelMazeIndex = DataHolder.Instance.GetLevelSystemIndexNumber();
            string newLevelSystemSceneName = "Level" + levelMazeIndex;

            DataHolder.Instance.SetLevelSceneName(newLevelSystemSceneName);
            DataHolder.Instance.LevelSystemIndexNumberUp();
            DataHolder.Instance.SetLocalIndexNumber();
            TileManager.Instance.LinkGroundTileWithScene();
            LoadScene(newLevelSystemSceneName);
            yield return null;

        }
        else
        {
            LoadScene(mazeLevelSceneName);
            yield return null;
        }

        UpdateLevelPlayAnalytics();

        yield return null;
    }

    IEnumerator LoadLevelWithBar(string level)
    {
        async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    private void UpdateLevelPlayAnalytics()
    {
        Analytics.CustomEvent("PlayLevel", new Dictionary<string, object>
        {
            { "PlayerId", Analytics.SetUserId(SystemInfo.deviceUniqueIdentifier)},
            { "Level", DataHolder.Instance.levelSceneName }
        });
    }
}