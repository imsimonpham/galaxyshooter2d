using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    
    private float _inputHorizontal, _inputVertical;
    
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shield;
    
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private BoxCollider2D _playerCollider;
    
        
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;
    
    [SerializeField] private GameObject _leftEngine, _rightEngine;

    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioSource _audioSource;

    private Animator _playerExplosion;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manger is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is null");
        }
        

        _playerExplosion = GetComponent<Animator>();
        if (_playerExplosion == null)
        {
            Debug.LogError("Player Explosion animator is null");
        }

        _playerCollider = GetComponent<BoxCollider2D>();
        if (_playerCollider == null)
        {
            Debug.LogError("Player Collider == null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Fire();
        }
    }

    void CalculateMovement()
    {
        _inputHorizontal = Input.GetAxis("Horizontal");
        _inputVertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_inputHorizontal, _inputVertical, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        
        //limit movement when exceeding the top and bottom of screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-5f, 5f) ,0);
        
        //player appears on the other side when exceeding the left and right of the screen
        if (transform.position.x > 9.3f)
        {
            transform.position = new Vector3(-9.3f, transform.position.y, 0);
        } else if (transform.position.x < -9.3f)
        {
            transform.position = new Vector3(9.3f, transform.position.y, 0); 
        }
    }

    public void Fire()
    {
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
        }

        _audioSource.PlayOneShot(_laserSound);
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
       
        _lives --;
        
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        } else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        } else if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
       StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    public void ActivateSpeedBoost()
    {
        _speed = 8.0f;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    public void CalculateScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ActivateShield()
    {
        _shield.SetActive(true);
        _isShieldActive = true;
    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 4.0f;
    }
    
}
