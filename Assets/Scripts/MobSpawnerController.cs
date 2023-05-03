using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject _mobPrefab;
    [SerializeField] MobDefinition[] _mobDefinitions;
    [SerializeField] GameObject[] _spawnPoints;
    [SerializeField] float _spawnRate; //in seconds

    private float _elapsed = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_elapsed >= _spawnRate)
        {
            SpawnRandomMob();
            _elapsed = 0f;
        }
        else
        {
            _elapsed += Time.deltaTime;
        }
    }


    private void SpawnRandomMob()
    {
        GameObject mob = Instantiate(_mobPrefab, _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform);
        mob.GetComponent<MobController>().MobDef = _mobDefinitions[Random.Range(0, _mobDefinitions.Length)];
    }
}
