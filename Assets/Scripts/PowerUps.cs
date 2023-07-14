using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpId;
    [SerializeField] private AudioClip _powerUpSound;
    
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
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            if (player != null)
            {
                switch (_powerUpId)
                {
                    case 0:
                        player.ActivateTripleShot(); 
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.AddExtraLife();
                        break;
                    default: 
                        Debug.Log("Default");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
