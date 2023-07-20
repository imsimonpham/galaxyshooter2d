using UnityEngine;

public class MegaBlastGun : MonoBehaviour
{
    [SerializeField] private GameObject _energyBallPrefab;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //direction = (transform.localRotation * Vector3.up).normalized;
    }

    public void MegaBlast()
    {
        Instantiate(_energyBallPrefab, transform.position, Quaternion.identity);
    }
}
