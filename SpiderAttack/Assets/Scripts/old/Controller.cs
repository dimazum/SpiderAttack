using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {
    public GameObject ButtonLoad;
    public GameObject SliderLoad;
    public Slider Slider;


    public void ExitButton()
    {
        Application.Quit();
    }
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));

        
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        ButtonLoad.SetActive(false);
        SliderLoad.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Slider.value = progress;
            yield return null;
        }
    }
}

