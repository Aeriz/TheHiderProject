using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCardDraw : MonoBehaviour {

    public Button[] buttons;
    public Vector3[] keyGuideLocs;
    public GameObject keyGuide;
    public int currentSelection;
    public string navigationKey;
    public string submitKey;
    public string playerNumber;
    bool canMove;

    // Use this for initialization
    void Start()
    {
        canMove = true;
        currentSelection = 0;
        buttons[currentSelection].Select();
    }

    // Update is called once per frame
    void Update()   
    {
        keyGuide.transform.localPosition = keyGuideLocs[currentSelection];

        buttons[currentSelection].Select();

        if (Input.GetAxisRaw(navigationKey) > 0 && canMove)
        {
            canMove = false;
            StartCoroutine("MoveRight");
        }

        if (Input.GetAxisRaw(navigationKey) < 0 && canMove)
        {
            canMove = false;
            StartCoroutine("MoveLeft");
        }

        if (Input.GetButtonDown(submitKey))
        {
            buttons[currentSelection].onClick.Invoke();
        }
    }

    IEnumerator MoveRight()
    {
        if (currentSelection < 2)
        {
            currentSelection += 1;
            buttons[currentSelection].Select();
        }
        else
        {
            currentSelection = 2;
            buttons[currentSelection].Select();
        }


        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    IEnumerator MoveLeft()
    {
        if (currentSelection > 0)
        {
            currentSelection -= 1;
            buttons[currentSelection].Select();
        }
        else
        {
            currentSelection = 0;
            buttons[currentSelection].Select();
        }

        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    public void AbilityCardChosen()
    {
        GameObject.Find("Hider " + playerNumber).GetComponent<Hiders>().AbilityRoll();
        gameObject.SetActive(false);
    }

    public void RoomCardChosen()
    {
        GameObject.Find("Hider " + playerNumber).GetComponent<Hiders>().RoomRoll();
        gameObject.SetActive(false);
    }
}
