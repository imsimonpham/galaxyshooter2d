using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _fireRate = 0.15f;
    private float _maxFuelAmount = 100f;
    private float _fuelAmountBurntPerSec = 15f;
    private float _fuelAmountRefilledPerSec = 20f;
    private float _canFire = -1f;
    
    private float _inputHorizontal, _inputVertical;
    
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _megaBlastPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _thruster;

    [SerializeField] private int _lives = 0;
    [SerializeField] private int _shieldLives = 0;
    [SerializeField] private int _score;
    [SerializeField] private int _ammoCount = 100;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private BoxCollider2D _playerCollider;
    private ParticleSystem _shieldParticleSystem;
    private CameraShakeManager _cameraShakeManager;
    private CinemachineImpulseSource _impulseSource;

    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private bool _isOutOfAmmo = false;
    [SerializeField] private bool _isMegaBlastActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false; 

    [SerializeField] private GameObject _leftEngine, _rightEngine;

    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _outOfAmmoSound;
    [SerializeField] private AudioClip _playerExplosionSound;
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private Image _fuelBar;
    

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

        _shieldParticleSystem = _shield.GetComponent<ParticleSystem>();
        if (_shieldParticleSystem == null)
        {
            Debug.LogError("Shield Particle System is null");   
        }

        _cameraShakeManager = GameObject.Find("CameraShakeManager").GetComponent<CameraShakeManager>();
        if (_cameraShakeManager == null)
        {
            Debug.LogError("Camera Shake Manager is null");
        }
        
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        if (_impulseSource == null)
        {
            Debug.LogError("Impulse Source is null");
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
        AdjustFuelBarColor();
        ActivateSpeedBoostOnFuel();
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
        if (_ammoCount > 0)
        {
            if (_isMegaBlastActive)
            {
                Instantiate( _megaBlastPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
           else if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
            else 
            {
                Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
            }
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            _audioSource.PlayOneShot(_laserSound);
        }
        else
        {
            _isOutOfAmmo = true;
            _audioSource.PlayOneShot(_outOfAmmoSound);
        }
    }
    public void Damage()
    {
        if (_isShieldActive)
        {
            ShieldDamage();
            return;
        }
        
        _lives --;
        _uiManager.UpdateLives(_lives);
        _cameraShakeManager.CameraShake(_impulseSource);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        } else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        } else if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            _playerExplosion.SetTrigger("OnPlayerDeath");
            _audioSource.PlayOneShot(_playerExplosionSound);
            Destroy(_playerCollider);
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this.gameObject, 2.3f);
        }
    }
    public void ShieldDamage()
    {
        if (_shieldLives == 3)
        {
            _shieldLives--;
            ChangeShieldVisual(0.50f);
            return;
        }
        else if (_shieldLives == 2)
        {
            _shieldLives--;
            ChangeShieldVisual(0.20f);
            return;
        }
        else if (_shieldLives == 1)
        {
            _shieldLives--;
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
    }

    public void RefillAmmo()
    {
        _ammoCount += 50;
        _uiManager.UpdateAmmo(_ammoCount);
    }
    
    public void ActivateMegaBlast()
    {
        _isTripleShotActive = false;
        _isMegaBlastActive = true;
        StartCoroutine(MegaBlastDownRoutine());
    }

    public void AddExtraLife()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            if (_lives == 3)
            {
                _leftEngine.SetActive(false);
            }
            else if (_lives == 2)
            {
                _rightEngine.SetActive(false);
            } 
        }
    }

    void ActivateSpeedBoostOnFuel()
    {
        if (_isSpeedBoostActive == false && Input.GetKey(KeyCode.LeftShift))
        {
            if (_fuelBar.fillAmount > 0)
            {
                _speed = 8.0f;
                _fuelBar.fillAmount -= _fuelAmountBurntPerSec/_maxFuelAmount * Time.deltaTime;
                _thruster.transform.localScale = new Vector3 (1f, 1f, 1f);
            }
            else
            {
                if (_isSpeedBoostActive == false)
                {
                    _speed = 5.0f;
                    _thruster.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
                }
            }
        }
        else
        {
            if (_isSpeedBoostActive == false)
            {
                _speed = 5.0f;
                _thruster.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
            }
            _fuelBar.fillAmount += _fuelAmountRefilledPerSec/_maxFuelAmount * Time.deltaTime;
        }
    }

    void AdjustFuelBarColor()
    {
        if (_fuelBar.fillAmount > 0.5 && _fuelBar.fillAmount <= 1)
        {
            _fuelBar.color = new Color32(219, 145, 55, 255);
        } else if (_fuelBar.fillAmount > 0.2 && _fuelBar.fillAmount <= 0.5)
        {
            _fuelBar.color = new Color32(215, 95, 30, 255);
        }
        else
        {
            _fuelBar.color = new Color32(215, 31, 38, 255);
        }
    } 

    public void ActivateTripleShot()
    {
        _isMegaBlastActive = false; 
        _isTripleShotActive = true;
       StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    public void ActivateSpeedBoost()
    {
        _speed = 8.0f;
        _isSpeedBoostActive = true;
        _thruster.transform.localScale = new Vector3 (1f, 1f, 1f);
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
        ChangeShieldVisual(1f);
        _shieldLives = 3;
        _isShieldActive = true;
    }

    void ChangeShieldVisual(float opacity)
    {
        var main = _shieldParticleSystem.main;
        main.startColor = new Color(1f,1f,1f,opacity);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _thruster.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        _speed = 5.0f;
    }

    IEnumerator MegaBlastDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isMegaBlastActive = false;
    }
    
}
