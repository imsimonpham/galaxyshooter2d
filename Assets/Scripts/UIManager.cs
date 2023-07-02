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

    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score: " + 0;
       _gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    { 
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int _currentLives)
    {
       _livesImg.sprite = _liveSprites[_currentLives];
    }

    public void DisplayGameOverText()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine());
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