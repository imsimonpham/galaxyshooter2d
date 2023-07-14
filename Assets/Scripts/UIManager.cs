using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private TMP_Text _ammoCountText;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score: " + 0;
       _gameOverText.gameObject.SetActive(false);
       _restartText.gameObject.SetActive(false);
       _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
       if (_gameManager == null)
       {
           Debug.LogError("Game Manager is null");
       }

    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    { 
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int playerAmmo)
    {
        _ammoCountText.text = "Ammo: " + playerAmmo;
    }

    public void UpdateLives(int _currentLives)
    {
       _livesImg.sprite = _liveSprites[_currentLives];
       if (_currentLives == 0)
       {
           DisplayGameOverText();
           DisplayRestartText();
           _gameManager.GameOver();
       }
    }

    public void DisplayGameOverText()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine());
    }

    public void DisplayRestartText()
    {
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.4f);
            _gameOverText.text = "GAME OVER";
        }
    }
}