using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyPFXSpawner : MonoBehaviour{

        [SerializeField] private Transform _playerTransform;

        private void Awake(){
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        internal void SpawnDamagedPfx(){
            
            // Calc norm between player and enemy:
            float normX = _playerTransform.position.x - transform.position.x;
            
            // Spawn pfx based on position relative to player:
            if (normX > 0f){
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/PFX/Enemy/Enemy-Damaged-Right"),
                    new Vector3(
                        transform.position.x,
                        transform.position.y + 1f,
                        transform.position.z),
                    Quaternion.identity);
            }
            else{
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/PFX/Enemy/Enemy-Damaged-Left"),
                    new Vector3(
                        transform.position.x,
                        transform.position.y + 1f,
                        transform.position.z),
                    Quaternion.identity);
            }
        }
    }
}
