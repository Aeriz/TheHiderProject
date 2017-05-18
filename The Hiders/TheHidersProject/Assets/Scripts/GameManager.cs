using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    int iterations;
    int i = 0;

    public GameObject numberOfPlayers;
    public GameObject cardSelect;
    public GameObject hider;

    public GameObject barricadeOne;
    public GameObject barricadeTwo;

    Color[] colours = { Color.green, Color.yellow, Color.red, Color.cyan };
    Vector3[] spawnLocs = { new Vector3(-25, 0.5f, -12.5f), new Vector3(25, 0.5f, 12.5f), new Vector3(-25, 0.5f, 12.5f), new Vector3(25, 0.5f, -12.5f) };
    public GameObject[] playerUI;
    public GameObject[] playerCardDrawUI;
    bool cardUIThreeSpawned;
    bool cardUIFourSpawned;

    bool playerSpawned;
    bool seekerSelected;
    public static bool selectCard;

    public static int abilityCount;
    public static bool allAbilitiesChosen = false;
    public static bool gameActive = false;

    public GameObject roomSelect;
    public GameObject[] playerRoomDrawUI;
    public GameObject[] rooms;

    bool roomUIThreeSpawned;
    bool roomUIFourSpawned;
    public static int roomCount;
    public static bool allRoomsChosen = false;

    public static bool hidersHide = false;
    public Text hidersHideUI;
    float hideTime;

    // Use this for initialization
    void Start () {
        numberOfPlayers.SetActive(true);
        cardSelect.SetActive(false);
        playerSpawned = false;
        seekerSelected = false;

        i = 1;
        iterations = 0;

        barricadeOne.SetActive(true);
        barricadeTwo.SetActive(true);

        cardUIThreeSpawned = false; 
        cardUIFourSpawned = false;

        abilityCount = 0;
        allAbilitiesChosen = false;
        gameActive = false;

        roomSelect.SetActive(false);

        roomUIThreeSpawned = false;
        roomUIFourSpawned = false;

        roomCount = 0;
        allRoomsChosen = false;

        hidersHide = false;
        hidersHideUI.gameObject.SetActive(false);
        hideTime = 15;

        foreach (GameObject UI in playerUI)
        {
            UI.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(i <= iterations)
        {
            GameObject newHider = Instantiate(hider, spawnLocs[i - 1], Quaternion.identity) as GameObject;
            newHider.name = "Hider " + i;
            newHider.GetComponent<Hiders>().playerNumber = i;
            newHider.GetComponent<Renderer>().material.color = colours[i - 1];
            playerUI[i - 1].SetActive(true);
            playerSpawned = true;
            i++;
        }

        if (i > iterations && playerSpawned && seekerSelected == false)
        {
            int seekerNumber = Random.Range(1, (iterations));
            Debug.Log(iterations + " & " + seekerNumber);
            GameObject seeker = GameObject.Find("Hider " + seekerNumber);
            seeker.GetComponent<Hiders>().isSeeker = true;
            seekerSelected = true;
        }

        if (i > iterations && playerSpawned && seekerSelected)
        {
            cardSelect.SetActive(true);
            selectCard = true;
        }

        if (i >= 4) barricadeOne.SetActive(false);
        if (i >= 5) barricadeTwo.SetActive(false);
        if (i <= 3)
        {
            playerCardDrawUI[2].SetActive(false);
            playerRoomDrawUI[2].SetActive(false);
        }
        else if (cardUIThreeSpawned == false)
        {
            playerCardDrawUI[2].SetActive(true);
            cardUIThreeSpawned = true;
        }
        else if (roomUIThreeSpawned == false && allAbilitiesChosen)
        {
            playerRoomDrawUI[2].SetActive(true);
            roomUIThreeSpawned = true;
        }
        if (i <= 4)
        {
            playerCardDrawUI[3].SetActive(false);
            playerRoomDrawUI[3].SetActive(false);
        }
        else if (cardUIFourSpawned == false)
        {
            playerCardDrawUI[3].SetActive(true);
            cardUIFourSpawned = true;
        }
        else if (roomUIFourSpawned == false && allAbilitiesChosen)
        {
            playerRoomDrawUI[3].SetActive(true);
            roomUIFourSpawned = true;
        }

        if (abilityCount >= iterations && playerSpawned)
        {
            allAbilitiesChosen = true;
            cardSelect.SetActive(false);
        }

        if (allAbilitiesChosen)
        {
            roomSelect.SetActive(true);
        }

        if (roomCount >= iterations && playerSpawned)
        {
            allRoomsChosen = true;
            roomSelect.SetActive(false);
        }

        if (allRoomsChosen && allAbilitiesChosen && playerSpawned)
        {
            gameActive = true;
        }

        if (gameActive && hidersHide == false)
        {
            hidersHideUI.gameObject.SetActive(true);
            hidersHideUI.text = "Hiders hide: " + (hideTime -= Time.deltaTime).ToString("F1");
            if (hideTime < 0) hidersHide = true;
        }
        if (gameActive && hidersHide)
        {
            hidersHideUI.gameObject.SetActive(false);
        }
    }

    public void TwoPlayers()
    {
        numberOfPlayers.SetActive(false);
        i = 1;
        iterations = 2;
    }

    public void ThreePlayers()
    {
        numberOfPlayers.SetActive(false);
        i = 1;
        iterations = 3;
    }

    public void FourPlayers()
    {
        numberOfPlayers.SetActive(false);
        i = 1;
        iterations = 4;
    }
}
