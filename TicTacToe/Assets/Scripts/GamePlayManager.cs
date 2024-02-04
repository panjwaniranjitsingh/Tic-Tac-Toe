using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int noOfButtons;
    [SerializeField] private List<GameObject> allButtons;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Text message;
    [SerializeField] private GameObject RestartButton;
    private int playerChances = 0;
    private GridLayoutGroup gridLayoutgroup;

    private void Start()
    {
        gridLayoutgroup = GetComponent<GridLayoutGroup>();
        RectTransform rect = GetComponent<RectTransform>();
        gridLayoutgroup.cellSize = new Vector2(rect.rect.width / 3, rect.rect.height / 3);
        if(noOfButtons>0)
            ButtonSetUp();
        RestartButton.GetComponent<Button>().onClick.AddListener(() => RestartGame());
        RestartButton.SetActive(false);
    }

    private void ButtonSetUp()
    {
        allButtons = new List<GameObject>();
        for (int i = 0; i < noOfButtons; i++)
        {
            GameObject button = Instantiate(buttonPrefab,gameObject.transform);
            button.name = "empty";
            button.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(button));
            allButtons.Add(button);
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
        {
            message.text = "Game Draw";
            RestartButton.SetActive(true);
        }
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

    private IEnumerator BotChance()
    {
        message.text = "O chance";
        yield return new WaitForSeconds(0.5f);
        int buttonToChoose;
        do
        {
            buttonToChoose = UnityEngine.Random.Range(0, allButtons.Count);
        } while (allButtons[buttonToChoose].name != "empty");

        ChangeButtonTo(allButtons[buttonToChoose],"O",images[1]);
        bool playerShouldPlay = true;
        if (playerChances >= 3)
            playerShouldPlay = CheckPlayerWin("O");
        if(playerShouldPlay)
            message.text = "Your chance";
    }

    private bool CheckPlayerWin(string player)
    {
        GameObject[] playerBoxes = new GameObject[playerChances];
        int noOfplayer = 0;
        for (int l = 0; l < allButtons.Count; l++)
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
                        Debug.Log(dif1+","+dif2 + "," + dif3 + "," + dif4);
                        if (RoundOff(dif1) == RoundOff(dif2) && RoundOff(dif3) == RoundOff(dif4))
                        {
                            GameOver(player);
                            return false;
                        }
                    }
        }
        return true;
    }

    private void GameOver(string player)
    {
        Debug.Log(player + " wins");
        message.text = player + " wins";
        foreach(GameObject button in allButtons)
            button.GetComponent<Button>().interactable = false;
        RestartButton.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private float RoundOff(float value)
    {
        return Mathf.Round(value * Mathf.Pow(10, 3)) / Mathf.Pow(10, 3);
    }
}
