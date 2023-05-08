using System.Linq;
using UnityEngine;

public class MobSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject _mobPrefab;
    [SerializeField] MobDefinition[] _mobDefinitions;
    [SerializeField] GameObject[] _spawnPoints;
    [SerializeField] float _spawnRate; //in seconds
    [SerializeField] float _spawnRateDelta; //after how many seconds does the spawner speed up
    [SerializeField] int _difficultyIncreaseDelta; // after how many mob kills do we get more difficult enemies



    Lookup<int, MobDefinition> _crMobDefMap; // a key-value mapping of CR difficulty and the respective Mob definitions
    int _spawnCr;
    float _currentSpawnRate;
    int _lastIncrementScore;

    private float _elapsed = 0f;
    private float _elapsedForDifficulty = 0f;

    void Start()
    {
        _currentSpawnRate = _spawnRate;
        _crMobDefMap = (Lookup<int, MobDefinition>)_mobDefinitions.ToLookup(mdef => mdef.cr);
        _spawnCr = 0;
        _lastIncrementScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_elapsed >= _currentSpawnRate)
        {
            SpawnRandomMob();
            _elapsed = 0f;
        }
        else
        {
            _elapsed += Time.deltaTime;
        }

        if (_elapsedForDifficulty >= _spawnRateDelta)
        {
            _currentSpawnRate -= 0.2f;
            _elapsedForDifficulty = 0f;
        }
        else
        {
            _elapsedForDifficulty += Time.deltaTime;
        }
    }


    private void SpawnRandomMob()
    {
        //increase mob difficulty
        if (GameController.Instance.Score % _difficultyIncreaseDelta == 0 &&
            _lastIncrementScore != GameController.Instance.Score &&
            _spawnCr < _crMobDefMap.Max(m => m.Key))
        {
            _lastIncrementScore = GameController.Instance.Score;
            _spawnCr++;
        }

        GameObject mob = Instantiate(_mobPrefab, ChooseSpawnPoint());
        MobDefinition[] randomCr = _crMobDefMap[Random.Range(0, _spawnCr + 1)].ToArray();
        mob.GetComponent<MobController>().MobDef = randomCr[Random.Range(0, randomCr.Length)];
    }

    private Transform ChooseSpawnPoint()
    {
        var orderByDistance = _spawnPoints.OrderBy(p => Vector3.Distance(p.transform.position, PlayerController.Instance.transform.position)).Reverse().ToArray();
        return orderByDistance[Random.Range(0, _spawnPoints.Length - 2)].transform; //the furthest 4 spawn points 
    }
}
