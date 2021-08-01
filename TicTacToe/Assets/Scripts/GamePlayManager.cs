using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] int noOfButtons;
    [SerializeField] GameObject[] allButtons;
    [SerializeField] Sprite[] images;
    [SerializeField] Text message;
    [SerializeField] GameObject RestartButton;
    int playerChances = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(noOfButtons>0)
            ButtonSetUp();
        RestartButton.GetComponent<Button>().onClick.AddListener(() => RestartGame());
        RestartButton.SetActive(false);
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
            button.name = "empty";
            button.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(button));
        }
        message.text = "X chance";
    }

    

    private void ButtonClicked(GameObject button)
    {
        Debug.Log("Button Clicked", gameObject);
        ChangeButtonTo(button,"X",images[0]);
        playerChances++;
        bool botShouldPlay = true;
        if (playerChances >= 3)
            botShouldPlay = CheckPlayerWin("X");
        if (botShouldPlay && playerChances < 5)
            StartCoroutine(BotChance());
        if (playerChances == 5)
            message.text = "Game Draw";
    }

    private void ChangeButtonTo(GameObject button,string player,Sprite sprite)
    {
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.sprite = sprite;
        buttonImage.color = Color.white;
        button.name = player;
        button.GetComponent<Button>().onClick.RemoveListener(() => ButtonClicked(button));
        button.GetComponent<Button>().interactable = false;
    }

    IEnumerator BotChance()
    {
        message.text = "O chance";
        yield return new WaitForSeconds(0.5f);
        int buttonToChoose;
        do
        {
            buttonToChoose = UnityEngine.Random.Range(0, allButtons.Length);
        } while (allButtons[buttonToChoose].name != "empty");

        ChangeButtonTo(allButtons[buttonToChoose],"O",images[1]);
        bool playerShouldPlay = true;
        if (playerChances >= 3)
            playerShouldPlay = CheckPlayerWin("O");
        if(playerShouldPlay)
            message.text = "X chance";
    }

    private bool CheckPlayerWin(string player)
    {
        GameObject[] playerBoxes = new GameObject[playerChances];
        int noOfplayer = 0;
        for (int l = 0; l < allButtons.Length; l++)
        {
            if (allButtons[l].name == player)
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
                            GameOver(player);
                            return false;
                        }
                        //Horizontal Match
                        if (playerBoxes[i].transform.position.y == playerBoxes[j].transform.position.y &&
                            playerBoxes[j].transform.position.y == playerBoxes[k].transform.position.y)
                        {
                            GameOver(player);
                            return false;
                        }
                        //Diagonal Match
                        float dif1 = playerBoxes[i].transform.position.x - playerBoxes[j].transform.position.x;
                        float dif2 = playerBoxes[j].transform.position.x - playerBoxes[k].transform.position.x;
                        float dif3 = playerBoxes[i].transform.position.y - playerBoxes[j].transform.position.y;
                        float dif4 = playerBoxes[j].transform.position.y - playerBoxes[k].transform.position.y;
                        //Debug.Log(dif1+","+dif2 + "," + dif3 + "," + dif4);
                        if (dif1 == dif2 && dif3 == dif4)
                        {
                            GameOver(player);
                            return false;
                        }
                    }
        }
        return true;
    }

    void GameOver(string player)
    {
        Debug.Log(player + " wins");
        message.text = player + " wins";
        foreach(GameObject button in allButtons)
            button.GetComponent<Button>().interactable = false;
        RestartButton.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
