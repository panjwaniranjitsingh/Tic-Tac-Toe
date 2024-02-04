using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform menuScreen;
    [SerializeField] private Transform gamePlayScreen;
    [SerializeField] private Transform contactListScreen;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button inviteButton;
    [SerializeField] private Button closeButton;
    
    private void Start()
    {
        ShowMenuScreen();

        startGameButton.onClick.AddListener(() => {
            menuScreen.gameObject.SetActive(false);
            gamePlayScreen.gameObject.SetActive(true);
            contactListScreen.gameObject.SetActive(false);
        });

        inviteButton.onClick.AddListener(() => {
            menuScreen.gameObject.SetActive(false);
            gamePlayScreen.gameObject.SetActive(false);
            contactListScreen.gameObject.SetActive(true);
        });

        closeButton.onClick.AddListener(() =>
        {
            ShowMenuScreen();
        });
    }

    private void ShowMenuScreen()
    {
        menuScreen.gameObject.SetActive(true);
        gamePlayScreen.gameObject.SetActive(false);
        contactListScreen.gameObject.SetActive(false);
    }
}
