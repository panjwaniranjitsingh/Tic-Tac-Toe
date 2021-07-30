using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] int noOfButtons;
    [SerializeField] GameObject[] allButtons;
    int playerChances = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(noOfButtons>0)
            ButtonSetUp();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void ButtonSetUp()
    {
        allButtons = new GameObject[noOfButtons];
        for (int i = 0; i < noOfButtons; i++)
        {
            allButtons[i] = Instantiate(buttonPrefab);
        }
        foreach (GameObject button in allButtons)
        {
            button.transform.SetParent(gameObject.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = "";
            button.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(button));
        }
    }

    private void ButtonClicked(GameObject button)
    {
        Debug.Log("Button Clicked",gameObject);
        button.transform.GetChild(0).GetComponent<Text>().text = "X";
        button.GetComponent<Button>().onClick.RemoveListener(() => ButtonClicked(button));
        button.GetComponent<Button>().interactable = false;
        playerChances++;
        bool botShouldPlay = true;
        if (playerChances >= 3)
           botShouldPlay = CheckWin("X");
        if(botShouldPlay && playerChances<5)
            StartCoroutine(BotChance());
    }

    IEnumerator BotChance()
    {
        yield return new WaitForSeconds(2f);
        int buttonToChoose;
        do
        {
            buttonToChoose = UnityEngine.Random.Range(0, allButtons.Length);
        } while (allButtons[buttonToChoose].transform.GetChild(0).GetComponent<Text>().text != "");
        allButtons[buttonToChoose].transform.GetChild(0).GetComponent<Text>().text = "O";
        allButtons[buttonToChoose].GetComponent<Button>().interactable = false;
        bool playerShouldPlay = true;
        if (playerChances >= 3)
            playerShouldPlay = CheckWin("O");
    }

    private bool CheckWin(string player)
    {
        GameObject[] playerBoxes = new GameObject[playerChances];
        int noOfplayer = 0;
        for (int l = 0; l < allButtons.Length; l++)
        {
            if (allButtons[l].transform.GetChild(0).GetComponent<Text>().text == player)
            {
                playerBoxes[noOfplayer] = allButtons[l];
                noOfplayer++;
            }
        }
        int i, j, k;
        if (playerBoxes.Length >= 3)
        {
            for (i = playerBoxes.Length - 1; i >= 2; i--)
                for (j = i - 1; j >= 1; j--)
                    for (k = j - 1; k >= 0; k--)
                    {
                        //Vertical Match
                        if (playerBoxes[i].transform.position.x == playerBoxes[j].transform.position.x &&
                            playerBoxes[j].transform.position.x == playerBoxes[k].transform.position.x)
                        {
                            Debug.Log(player + " win");
                            return false;
                        }
                        //Horizontal Match
                        if (playerBoxes[i].transform.position.y == playerBoxes[j].transform.position.y &&
                            playerBoxes[j].transform.position.y == playerBoxes[k].transform.position.y)
                        {
                            Debug.Log(player + " wins");
                            return false;
                        }
                        //Diagonal Match
                        float dif1 = playerBoxes[i].transform.position.x - playerBoxes[j].transform.position.x;
                        float dif2 = playerBoxes[j].transform.position.x - playerBoxes[k].transform.position.x;
                        float dif3 = playerBoxes[i].transform.position.y - playerBoxes[j].transform.position.y;
                        float dif4 = playerBoxes[j].transform.position.y - playerBoxes[k].transform.position.y;
                        Debug.Log(dif1+","+dif2 + "," + dif3 + "," + dif4);
                        if (dif1 == dif2 && dif3 == dif4)
                        { 
                            Debug.Log(player + " wins");
                            return false;
                        }
                    }
        }
        return true;
    }
}
