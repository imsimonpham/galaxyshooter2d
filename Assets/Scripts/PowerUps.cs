using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpId;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (_powerUpId == 0)
                {
                    player.ActivateTripleShot();
                } else if (_powerUpId == 1)
                {
                    player.ActivateSpeedBoost();
                }
            }
            Destroy(this.gameObject);
        }
    }
}
