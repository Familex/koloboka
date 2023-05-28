using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    /// <summary>
    /// MonoBehaviour component for enemy spawning.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private uint maxEnemies = 3;
        [SerializeField] private uint initialSpawnRange = 15;
        [SerializeField] private float minSpawnTime = 5f;
        [SerializeField] private float maxSpawnTime = 10f;
        
        private uint _enemiesCount;
        
        /// <summary>
        /// Start infinite coroutine for enemy spawning.
        /// </summary>
        private void Start()
        {
            StartCoroutine(SpawnEnemy());
        }

        /// <summary>
        /// Infinite coroutine for enemy spawning.
        /// Spawns random enemy from <see cref="enemyPrefabs"/> at random position in <see cref="initialSpawnRange"/>
        /// radius.
        /// </summary>
        /// <returns>IEnumerator for coroutine</returns>
        private IEnumerator SpawnEnemy()
        {
            while (true)
            {
                if (_enemiesCount < maxEnemies)
                {
                    var randomPos =  Random.insideUnitCircle * initialSpawnRange;
                    NavMesh.SamplePosition(transform.position + new Vector3(randomPos.x, 0, randomPos.y), out var hit, Mathf.Infinity, NavMesh.AllAreas);
                    var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], hit.position + Vector3.up * 2,
                        Quaternion.Euler(Random.insideUnitCircle));
                    enemy.GetComponent<Enemy.Enemy>().OnDeathEvent += () => _enemiesCount--;
                    enemy.GetComponentInChildren<Enemy.MoveAndJumpingAround>().goal = transform;
                    _enemiesCount++;
                }
                
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            }
        } 
    }
}