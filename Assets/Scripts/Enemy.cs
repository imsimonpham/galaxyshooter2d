using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    private Player _player;
    private Animator _enemyExplosionAnim;
    private BoxCollider2D _enemyCollider;
   
    [SerializeField] private AudioSource _audioSource;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        _enemyExplosionAnim =  GetComponent<Animator>();
        if (_enemyExplosionAnim == null)
        {
            Debug.LogError("Enemy explosion animator is null");
        }
        _enemyCollider = GetComponent<BoxCollider2D>();
        if (_enemyCollider == null)
        {
            Debug.LogError("Enemy Box Collider is null");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio source on the enemy is null");
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-9.3f, 9.3f);
            transform.position = new Vector3(randomX, 6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _enemyExplosionAnim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 2.3f);
        } 
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.CalculateScore(10);
            _enemyExplosionAnim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(_enemyCollider);
            Destroy(this.gameObject, 2.3f);
        }
    }
}
