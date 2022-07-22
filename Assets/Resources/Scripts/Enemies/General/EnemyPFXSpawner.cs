using UnityEngine;

// Code within this class contains functions relating to spawning
// particle effects for all classes of enemies:
namespace Resources.Scripts.Enemies.General{
    public class EnemyPFXSpawner : MonoBehaviour{

         private Transform _playerTransform;
        [SerializeField] internal Vector2 _offset;
        
        // PFX Parent:
        internal Transform _pfxParent;
        
        private void Awake(){
            
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _pfxParent = GameObject.FindGameObjectWithTag("PFXParent").transform;
        }

        internal void SpawnDamagedPfx(){
            
            // Calc norm between player and enemy:
            float normX = _playerTransform.position.x - transform.position.x;
            
            // Spawn pfx based on position relative to player:
            if (normX > 0f){
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/PFX/Enemy/Enemy-Damaged-Right"),
                    new Vector3(
                        transform.position.x + _offset.x,
                        transform.position.y + _offset.y,
                        transform.position.z),
                    Quaternion.identity);
            }
            else{
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/PFX/Enemy/Enemy-Damaged-Left"),
                    new Vector3(
                        transform.position.x + _offset.x,
                        transform.position.y + _offset.y,
                        transform.position.z),
                    Quaternion.identity,
                    _pfxParent);
            }
        }
    }
}
