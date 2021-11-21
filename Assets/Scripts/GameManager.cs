using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This controls the game logic
// The game is first initialized with a game map, player location,
// boxes locations and storage locations
// and then is controlled by game logic
// In game logic, player can move in the map
// and check the winning condition after each move
// There are two more functions, display game and display winning messages

// Game Starts here
// There will be a man pushing boxes in order to
// Put them back in the right place
// There will be different boxes, pushable or not
public class GameManager : MonoBehaviour
{
    // Fields
    public MapBuilder mapBuilder;
    public GameObject nextButton;
    public GameObject menu;
    public GameObject winningText;
    public GameObject losingText;
    public GameObject resetButton;
    private Player player;
    private bool input;
    private Map gamemap;
    private int stepCountLv1;
    private int stepCountLv2;
    private int stepCountLv3;
    private int mapSelected;
    private static bool won, lost;
    public Text tempText;
    public Text stepCount;

    public GameManager(){}
    
    private void initialization()
    {
        won = false;
        lost = false;
        stepCountLv1 = 1;
        stepCountLv2 = 8;
        stepCountLv3 = 150;
        winningText.SetActive(false);
        losingText.SetActive(false);
        nextButton.SetActive(false);
        resetButton.SetActive(false);
    }
    // Game menu starts in Start()
    // Start is called before the first frame update
    private void Start()
    {
        game_menu();
    }

    public void game_menu()
    {
        initialization();
        StartCoroutine(SelectionPage(5));
        if (tempText)
        {
            Destroy(tempText);
        }
    }

    private void reset()
    {
        menu.SetActive(false);
        won = false;
        lost = false;
        stepCountLv1 = 1;
        stepCountLv2 = 8;
        stepCountLv3 = 150;
        winningText.SetActive(false);
        losingText.SetActive(false);
        //nextButton.SetActive(false);
    }

    public IEnumerator SelectionPage(int selection) 
    {
        reset();
        if (SceneManager.sceneCount > 1 && selection != 3 && selection != 4)
        {
            AsyncOperation asyncUnloadLevel = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncUnloadLevel.isDone)
            {
                yield return null;
                Debug.Log("UnLoading...");
            }
            Debug.Log("Unload Done");
            Resources.UnloadUnusedAssets();
        }
        // selection 0 is menu page
        if (selection == 5)
        {
            menu.SetActive(true);
            Debug.Log("Menu loaded");
        // selection 1 to 3 is for Map 1 to 3
        }else if(selection >= 0 && selection < 3)
        {
            if(selection == 0)
            {
                showSteps(stepCountLv1);
            }else if(selection == 1)
            {
                showSteps(stepCountLv2);
            }
            else if (selection == 2)
            {
                showSteps(stepCountLv3);
            }else if (selection >= 3)
            {
                selection = 0;
            }
            nextButton.SetActive(false);
            resetButton.SetActive(true);
            mapSelected = selection;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
                Debug.Log("Loading...");
            } 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
            mapBuilder.Build(selection);
            player = FindObjectOfType<Player>();
            Debug.Log("Map loaded");
        // selection 3 is winning Text
        }else if(selection == 3)
        {
            winningText.SetActive(true);
            won = true;
            Debug.Log("Winning condition acheived");
        }
        // selection 4 is losing Text
        else if (selection == 4)
        {
            losingText.SetActive(true);
            lost = true;
            Debug.Log("Losing condition acheived");
        }
    }

    private bool detect_game_end()
    {
        if(won || lost)
        {
            return true;
        }
        return false;
    }

    private bool win() 
    {
        if (IsLevelComplete())
        {
            if (mapBuilder.selected == 0 || mapBuilder.selected == 1 || mapBuilder.selected == 2)
            {
                return true;
            }
        }
        return false;
    }

    private bool lose() 
    {
        if(mapBuilder.selected == 0)
        {
            if(stepCountLv1 == 0)
            {
                return true;
            }
        }else if (mapBuilder.selected == 1)
        {
            if (stepCountLv2 == 0)
            {
                return true;
            }
        }
        else if (mapBuilder.selected == 2)
        {
            if (stepCountLv3 == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void toMap1()
    {
        StartCoroutine(SelectionPage(0));
    }

    public void toMap2()
    {
        StartCoroutine(SelectionPage(1));
    }

    public void toMap3()
    {
        StartCoroutine(SelectionPage(2));
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
         if(moveInput.sqrMagnitude > 0.5)
        {
            // For Discrete Movement, once at a time
            if (input && !detect_game_end())
            {
                input = false;
                player.move(moveInput);
                if(mapSelected == 0)
                {
                    showSteps(--stepCountLv1);
                    Debug.Log(stepCountLv1);
                }else if (mapSelected == 1)
                {
                    showSteps(--stepCountLv2);
                    Debug.Log(stepCountLv2);
                }
                else if (mapSelected == 2)
                {
                    showSteps(--stepCountLv3);
                    Debug.Log(stepCountLv3);
                }
                nextButton.SetActive(IsLevelComplete());
                // Determine if this step causes winning condition
                if (win())
                {
                    StartCoroutine(SelectionPage(3));
                }
                else if(lose())
                {
                    StartCoroutine(SelectionPage(4));
                }
            }
        }
        else
         {
            input = true;
        }
    }

    void showSteps(int steps)
    {
        if (tempText)
        {
            Destroy(tempText);
        }
        GameObject canvas = GameObject.Find("Canvas");
        tempText = (Text)Instantiate(stepCount);
        tempText.fontSize = 66;
        tempText.transform.position = new Vector3(1400, 662, 0);
        tempText.transform.SetParent(canvas.transform, false);
        tempText.text = steps.ToString();
    }

    public void Nextlevel()
    {
        nextButton.SetActive(false);
        ++mapSelected;
        if(mapSelected > 2)
        {
            mapSelected = 0;
        }
        StartCoroutine(SelectionPage(mapSelected));
    }

    public void reset_all()
    {
        StartCoroutine(SelectionPage(mapSelected));
    }

    public bool IsLevelComplete()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach(var box in boxes)
        {
            if (!box._onCross)
            {
                return false;
            }
        }
        nextButton.SetActive(true);
        return true;
    }
}
