using System.Collections.Generic;
using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyData : MonoBehaviour{

        // Scripts:
        private MonoBehaviourUtility _monoBehaviourUtilityScript;
        
        [Range(0f, 5000f)][SerializeField] internal float _maxHp;
        [SerializeField] internal float _hp;

        [Range(0f, 1f)][SerializeField] private float firstDropThreshold;
        [Range(0f, 1f)][SerializeField] private float secondDropThreshold;
        [SerializeField] private List<int> firstDrop;
        [SerializeField] private List<int> secondDrop;
        [SerializeField] private List<int> deathDrop;
        private bool _spawnedFirstLoot;
        private bool _spawnedSecondLoot;
        private bool _spawnedDeathLoot;
        private void Awake(){
            
            // Fetch components:
            _monoBehaviourUtilityScript = GameObject.Find("Utility").GetComponent<MonoBehaviourUtility>();
            
            // Set values:
            _hp = _maxHp;
        }

        private void Update(){
            
            ClampHp();
            SpawnLoot();
        }

        public void DecrementHp(float value){
            _hp -= value;
        }
        private void ClampHp(){
            if (_hp < 0f)
                _hp = 0f;
            if (_hp > _maxHp)
                _hp = _maxHp;
        }

        private void SpawnLoot(){
            // First threshold:
            if (_hp < _maxHp * firstDropThreshold && !_spawnedFirstLoot){
                _monoBehaviourUtilityScript.StartSleep(0.05f);
                SpawnSapphires(0, firstDrop[0]);
                SpawnSapphires(1, firstDrop[1]);
                // SpawnSapphires(2, firstDrop[2]);
                _spawnedFirstLoot = true;
            }
            // Second threshold:
            if (_hp < _maxHp * secondDropThreshold && !_spawnedSecondLoot){
                _monoBehaviourUtilityScript.StartSleep(0.05f);
                SpawnSapphires(0, secondDrop[0]);
                SpawnSapphires(1, secondDrop[1]);
                // SpawnSapphires(2, secondDrop[2]);
                _spawnedSecondLoot = true;
            }
            // Death threshold:
            if (_hp <= 0f && !_spawnedDeathLoot){
                _monoBehaviourUtilityScript.StartSleep(0.1f);
                SpawnSapphires(0, deathDrop[0]);
                SpawnSapphires(1, deathDrop[1]);
                // SpawnSapphires(2, deathDrop[2]);
                _spawnedDeathLoot = true;
            }
        }

        private void SpawnSapphires(int type, int amount){
            if (amount > 0){
                for (int i = 0; i < amount; i++){
                    Instantiate(UnityEngine.Resources.Load<GameObject>
                            ("Prefabs/General/ShadowSapphires/Shadow-Sapphire-" + type), 
                        new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z),
                        Quaternion.identity);
                }
            }
        }
    }
}
