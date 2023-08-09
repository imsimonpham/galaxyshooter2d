using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver = false;
    [SerializeField] private bool _playerWin = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true || Input.GetKeyDown(KeyCode.R) && _playerWin == true)
        {
            SceneManager.LoadScene("Game");
        } 
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
    
    public void PlayerWin()
    {
        _playerWin = true;
    }
}
