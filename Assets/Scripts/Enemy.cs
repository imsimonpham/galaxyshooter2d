using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    private Player _player;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
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
            Destroy(this.gameObject);
        } 
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.CalculateScore(10);
            Destroy(this.gameObject);
        }
    }
}
