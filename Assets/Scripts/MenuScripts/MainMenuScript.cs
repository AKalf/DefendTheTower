using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    GameObject blackPanel;

    [SerializeField]
    GameObject levelDescriptiom;

    //[SerializeField]
    //AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            Cursor.visible = true;
        }
        else {
            Cursor.visible = false;
        }   
    }

    
    public void StartGame()
    {
        if (levelDescriptiom != null)
        {
            levelDescriptiom.SetActive(true);
        }
        //audioSource.Play();
        StartCoroutine(LoadNewScene(1));
    }
    public void GoToCredits()
    {
        //audioSource.Play();
        StartCoroutine(LoadNewScene(2));
    }
    public void ReturnToMainMenu()
    {
        //audioSource.Play();
        StartCoroutine(LoadNewScene(0));
    }

    IEnumerator LoadNewScene(int scene)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        blackPanel.SetActive(true);
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {

            yield return null;
        }
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}