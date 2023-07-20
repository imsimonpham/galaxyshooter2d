using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject[] _powerUps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(2.5f);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.3f,9.3f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(10f);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.3f, 9.3f), 6f, 0);
            int randomIndex = Random.Range(0, 6);
            Instantiate(_powerUps[randomIndex], posToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
