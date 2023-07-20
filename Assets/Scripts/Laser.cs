using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    private Player _player;
    
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
        if (transform.name == "EnergyBall(Clone)")
        {
            transform.Translate(transform.up * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        
        if (transform.position.y > 6f || transform.position.x < -9f || transform.position.x > 9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 2f);
            }
            else
            {
                Destroy(this.gameObject, 2f);
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
            }  else
            {
                Destroy(this.gameObject);
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
                Destroy(this.gameObject);
            }
        }

        if (transform.tag == "Laser" && other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
