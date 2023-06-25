using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private float inputHorizontal;
    private float inputVertical;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is NULL");
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
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(inputHorizontal, inputVertical, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        
        //limit movement when exceeding the top and bottom of screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,-5f, 5f) ,0);
        
        //player appears on the other side when exceeding the left and right of the scren
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
        Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 0.8f, 0), Quaternion.identity);
    }

    public void Damage()
    {
        _lives --;
        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }
}
