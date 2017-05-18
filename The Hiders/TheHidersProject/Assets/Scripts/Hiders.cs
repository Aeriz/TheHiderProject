using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Hiders : MonoBehaviour {

    public float speed = 10f;

    CharacterController controller;
    GameObject gameManager;

    float moveHorizontal;
    float moveVertical;

    string[] player = { "PlayerOneX", "PlayerOneY", "PlayerTwoX", "PlayerTwoY", "PlayerThreeX", "PlayerThreeY", "PlayerFourX", "PlayerFourY" };
    string[] playerInteract = { "PlayerOneSubmit", "PlayerTwoSubmit", "PlayerThreeSubmit", "PlayerFourSubmit" };

    public int playerNumber;

    public bool[] abilities = { false, false, false, false, false, false };
    string[] abilityNames = { "Null", "Inversion", "Sledgehammer", "Disable torch", "Slowing goop", "Decoy" };
    private int abilityRoll;
    private bool abilitiesChosen = false;
    private int roomRoll;
    private bool roomsChosen = false;
    GameObject abilityText;
    public string currentAbility;
    public string currentRoom;

    Vector3[] roomSpawnLocs = { new Vector3(-25, 50, -12.5f), new Vector3(25, 50, 12.5f), new Vector3(-25, 50, 12.5f), new Vector3(25, 50, -12.5f) };

    private int inputX;
    private int inputY;
    private string interact;

    public bool isSeeker;
    public bool spotted;
    bool checkSpotted;
    public bool caught;
    GameObject caughtUI;

    private Hiders hider;

    // Use this for initialization
    void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        currentAbility = abilityNames[0];
        abilitiesChosen = false;
        roomsChosen = false;
        if (playerNumber == 1) inputX = 0;
        else inputX = playerNumber + (playerNumber - 2);
        inputY = playerNumber + (playerNumber - 1);
        interact = playerInteract[playerNumber - 1];
        abilityText = GameObject.Find("Player" + playerNumber.ToString() + "Ability");
        isSeeker = false;
        gameManager = GameObject.Find("_Manager");
        gameObject.tag = "Hider";
        spotted = false;
        checkSpotted = true;
        caught = false;
        caughtUI = transform.FindChild("Caught").gameObject;
        gameObject.GetComponent<Renderer>().enabled = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (caught) { caughtUI.SetActive(true); spotted = true; }
        else if (caught == false) caughtUI.SetActive(false);

        gameObject.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        moveHorizontal = Input.GetAxisRaw(player[inputX].ToString());
        moveVertical = Input.GetAxisRaw(player[inputY].ToString());

        if (isSeeker)
        {
            gameObject.tag = "Seeker";
            GameObject torch = GameObject.Find("Torch");
            torch.transform.parent = transform;
            torch.transform.localPosition = new Vector3(0, 0, 0.35f);

            if (GameManager.hidersHide == false) gameObject.GetComponent<CharacterController>().enabled = false;
            else if (GameManager.hidersHide) gameObject.GetComponent<CharacterController>().enabled = true;
            if (GameManager.hidersHide) Torch(10f);
        }

        if (GameManager.gameActive && isSeeker == false) CollisionCheck(0.75f);
        if (GameManager.gameActive && isSeeker == false && caught == true) gameObject.GetComponent<CharacterController>().enabled = false;
        else if (GameManager.gameActive && isSeeker == false && caught == false) gameObject.GetComponent<CharacterController>().enabled = true;

        if (checkSpotted) StartCoroutine("ReAppear", 0.25f);

        if (spotted && GameManager.gameActive && isSeeker == false) gameObject.GetComponent<Renderer>().enabled = true;
        else if (GameManager.gameActive && isSeeker == false) gameObject.GetComponent<Renderer>().enabled = false;

        if (Input.GetButtonDown(interact) && GameManager.gameActive)
        {
            Debug.Log("Interact pressed by player " + playerNumber);
            if (isSeeker) SeekerInteract();
            else if (isSeeker == false) HiderInteract();
        }

        if ((moveHorizontal != 0 || moveVertical != 0) && GameManager.gameActive) HandleMovement(gameObject, moveHorizontal, moveVertical);
    }

    void HandleMovement(GameObject hider, float xVelocity, float yVelocity)
    {
        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, v.y, 0f);

        Vector3 moveDirection = new Vector3(xVelocity, 0f, yVelocity);

        if (xVelocity != 0 || yVelocity != 0)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
        }

        float adjSpeed = moveDirection.magnitude;

        controller.Move(moveDirection.normalized * adjSpeed * speed * Time.deltaTime);
    }

    void CollisionCheck(float rayDist)
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, transform.forward, out hit, rayDist) || Physics.Raycast(this.transform.position, -transform.forward, out hit, rayDist) || Physics.Raycast(this.transform.position, transform.right, out hit, rayDist) || Physics.Raycast(this.transform.position, -transform.right, out hit, rayDist))
        {
            if (hit.collider.tag == "Walls" && (moveHorizontal != 0 || moveVertical != 0)) { spotted = true; }
        }     
    }

    void Torch(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, 1 << 8);
        int i = 0;
        while (i < hitColliders.Length)
        {
            RaycastHit torchHit;
            if (Physics.Linecast(this.transform.position, hitColliders[i].transform.position, out torchHit))
            {
                if (torchHit.collider.tag == "Hider")
                {
                    Vector3 targetDir = hitColliders[i].transform.position - transform.position;
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    float forwardsDist = Vector3.Dot(targetDir, transform.forward);
                    targetDir.Normalize();
                    float sidewaysDist = Vector3.Dot(targetDir, transform.right);
                    hider = hitColliders[i].GetComponent<Hiders>();

                    if (sidewaysDist >= -range && sidewaysDist <= range && forwardsDist >= 0 && forwardsDist <= range)
                    {
                        hider = hitColliders[i].GetComponent<Hiders>();
                        hider.spotted = true;
                        break;
                    }
                }
            }
            i++;
        }
    }

    void HiderInteract()
    {

    }

    void SeekerInteract()
    {
        foreach(GameObject hider in GameObject.FindGameObjectsWithTag("Hider"))
        {
            if (Vector3.Distance(transform.position, hider.transform.position) < 2)
            {
                if (hider.GetComponent<Hiders>().spotted) hider.GetComponent<Hiders>().caught = true;
            }
        }      
    }

    IEnumerator ReAppear(float waitTime)
    {
        checkSpotted = false;
        spotted = false;
        yield return new WaitForSeconds(waitTime);
        checkSpotted = true;
    }

    public void AbilityRoll()
    {
        if (abilitiesChosen == false)
        {
            abilityRoll = Random.Range(1, abilities.Length);
            currentAbility = abilityNames[abilityRoll];
            GameManager.abilityCount += 1;
            abilityText.GetComponent<Text>().text = currentAbility;
            abilitiesChosen = true;
        }
    }

    public void RoomRoll()
    {
        if (roomsChosen == false)
        {
            roomRoll = Random.Range(1, gameManager.GetComponent<GameManager>().rooms.Length);
            currentRoom = gameManager.GetComponent<GameManager>().rooms[roomRoll].ToString();
            GameManager.roomCount += 1;
            Instantiate(gameManager.GetComponent<GameManager>().rooms[roomRoll], roomSpawnLocs[playerNumber - 1], Quaternion.identity);
            roomsChosen = true;
        }
    }
}
