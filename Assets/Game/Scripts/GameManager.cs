using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int FruitScore { get; private set; } = 0;

    [Header("Player Settings")]
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _spawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(ResoawnRoutine());
    }

    public void ChangeSpawnPoint(Transform newSpawnPoint)
    {
        if (newSpawnPoint != null)
        {
            _spawnPoint = newSpawnPoint;
        }
    }
    private IEnumerator ResoawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        if (_playerPrefab != null)
        {
            Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }

    public void GetFruit() => FruitScore++;

}
