﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider slider;
	public Text progressText;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsyncOpration(sceneIndex));
    }

    IEnumerator LoadAsyncOpration(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		
		loadingScreen.SetActive(true);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			
			slider.value = progress;
			progressText.text = progress * 100f + "%";

			yield return null;
		}
    }
}
