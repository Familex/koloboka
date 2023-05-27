using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private uint maxEnemies = 3;
        [SerializeField] private uint initialSpawnRange = 15;
        [SerializeField] private float minSpawnTime = 5f;
        [SerializeField] private float maxSpawnTime = 10f;
        
        private uint _enemiesCount;
        private Player _player;
        
        private void Start()
        {
            _player = GetComponentInParent<Player>();
            StartCoroutine(SpawnEnemy());
        }

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
                    var sphereCollider = enemy.AddComponent<Enemy.SphereCollider>();
                    sphereCollider.target = transform;
                    sphereCollider.radius = .6f;
                    sphereCollider.OnCollide +=
                        () => _player.OnDeath();
                    _enemiesCount++;
                }
                
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            }
        } 
    }
}