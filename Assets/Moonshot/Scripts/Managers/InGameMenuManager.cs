using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menuRoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Lock cursor when clicking outside of menu
        //if (!menuRoot.activeSelf && Input.GetMouseButtonDown(0))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuRoot.SetActive(!menuRoot.activeSelf);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (menuRoot.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        //if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
        //    || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        //{
        //    if (controlImage.activeSelf)
        //    {
        //        controlImage.SetActive(false);
        //        return;
        //    }

        //    SetPauseMenuActivation(!menuRoot.activeSelf);

        //}

        //if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
        //{
        //    if (EventSystem.current.currentSelectedGameObject == null)
        //    {
        //        EventSystem.current.SetSelectedGameObject(null);
        //        lookSensitivitySlider.Select();
        //    }
        //}
    }


    public void OpenPauseMenu()
    {

    }

    public void Continue()
    {
        menuRoot.SetActive(false);
    }

    [System.Obsolete]
    public void Restart()
    {
        Assets.Moonshot.Scripts.Spaceship.Ship._ships = new List<Assets.Moonshot.Scripts.Spaceship.Ship>();
        SpaceGenerator.Generators.WorldBuilder._generateAtStart = true;
        Application.LoadLevel(Application.loadedLevel);        
    }

    public void Close()
    {
        Application.Quit();
    }
}
