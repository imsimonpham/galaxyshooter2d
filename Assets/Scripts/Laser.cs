
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;

    private Player _player;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "EnemyLaser")
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
               
        }
    }
    
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.tag == "EnemyLaser" && other.tag == "Player")
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage();
            }
        }
    }
}
