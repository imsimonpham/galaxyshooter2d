using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private float _inputHorizontal;
    private float _inputVertical;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private GameObject _shield;
    [SerializeField] private int _score;
    private UIManager _uiManager;
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

    void Fire()
    {
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
        }
        
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
        
        if (_lives < 1)
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
