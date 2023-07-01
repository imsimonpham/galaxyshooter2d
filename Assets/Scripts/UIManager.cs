using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _liveSprites;

    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score: " + 0;
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
}