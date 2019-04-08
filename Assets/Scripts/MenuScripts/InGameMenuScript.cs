using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuScript : MonoBehaviour
{

    static InGameMenuScript inst = null;
    [SerializeField]
    GameObject endingPanel;
    [SerializeField]
    GameObject blackPanel;
    [SerializeField]
    GameObject inGameMenu;
    [SerializeField]
    GameObject optionsPanel;
    Slider sensitivitySlider = null;
    Text sensitivityText = null;
    bool panelVisible = false;
    [SerializeField]
    CameraMovement cameraMovementScript = null;
    //GameObject playerBoy = null;
    // GameObject playerGirl = null;
    //GameObject[] checkPoint = new GameObject[5];


    public static InGameMenuScript GetInstance()
    {
        if (inst != null)
        {
            return inst;
        }
        else
        {
            throw new System.Exception("An instance of object -InGameMenu- has not been created but a component is trying to access it");
        }

    }

    private void Awake()
    {

        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        /*
        checkPoint[0] = GameObject.FindGameObjectWithTag("Startcp");
        checkPoint[1] = GameObject.FindGameObjectWithTag("TableScp");
        checkPoint[2] = GameObject.FindGameObjectWithTag("Bedcp");
        checkPoint[3] = GameObject.FindGameObjectWithTag("TableBcp");
        checkPoint[4] = GameObject.FindGameObjectWithTag("Endcp");
        */
    }
    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sensitivitySlider = optionsPanel.GetComponentInChildren<Slider>();
        sensitivitySlider.value = cameraMovementScript.GetSensitivity();
        sensitivityText = sensitivitySlider.GetComponentInChildren<Text>();
        sensitivityText.text = sensitivitySlider.value.ToString();
        //PlayerMovement.GetInput = true;
        //playerBoy = GameObject.FindGameObjectWithTag("PlayerBoy");
        
    }

    // Update is called once per frame
    void Update()
    {

        //Cheats();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panelVisible)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                //PlayerMovement.GetInput = false;
                Time.timeScale = 0;
                inGameMenu.SetActive(true);
                panelVisible = true;
            }
            else
            {
                ResumeGame();
            } 
        }
        if (optionsPanel.activeInHierarchy) {
            cameraMovementScript.SetSensitivity(sensitivitySlider.value);
        }
    }
    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //PlayerMovement.GetInput = true;
        inGameMenu.SetActive(false);
        panelVisible = false;
        Time.timeScale = 1;
        
    }
    public void Finish()
    {
        Time.timeScale = 0;
        endingPanel.SetActive(true);
        Cursor.visible = true;
    }

    public void GoToCredits()
    {
        endingPanel.SetActive(false);
        StartCoroutine(LoadNewScene(2));
    }
    public void ReturnToMainMenu()
    {
        inGameMenu.SetActive(false);
        endingPanel.SetActive(false);
        StartCoroutine(LoadNewScene(0));
    }
    public void ShowOptions() {
        optionsPanel.SetActive(true);
    }
    public void CloseOptionsButton() {
        optionsPanel.SetActive(false);
    }
    IEnumerator LoadNewScene(int scene)
    {
        blackPanel.SetActive(true);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {

            yield return null;
        }


    }
    public void SetSensitivityText(float value) {
        sensitivityText.text = ((int)(sensitivitySlider.value)).ToString();
    }
    /*
    private void Cheats()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerBoy.transform.position = checkPoint[0].transform.position;
            if (playerGirl != null)
            {
                playerGirl.transform.position = checkPoint[0].transform.position;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerBoy.transform.position = checkPoint[1].transform.position;
            if (playerGirl != null)
            {
                playerGirl.transform.position = checkPoint[1].transform.position;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerBoy.transform.position = checkPoint[2].transform.position;
            if (playerGirl != null)
            {
                playerGirl.transform.position = checkPoint[2].transform.position;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerBoy.transform.position = checkPoint[3].transform.position;
            if (playerGirl != null)
            {
                playerGirl.transform.position = checkPoint[3].transform.position;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerBoy.transform.position = checkPoint[4].transform.position;
            if (playerGirl != null)
            {
                playerGirl.transform.position = checkPoint[4].transform.position;
            }
        }
    }
    */
}