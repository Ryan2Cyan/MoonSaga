using Resources.Scripts.General;
using Resources.Scripts.Lighting;
using UnityEngine;

// Code within this class is responsible for handling particle
// effects in relation to the player model:
namespace Resources.Scripts.Player{
    public class PlayerPFXSpawner : MonoBehaviour{
        
        // Scripts:
        private PlayerData _playerDataScript;
        private LightDetection _lightDetectionScript;
        [SerializeField] private RadiusChecker _groundCheckScript;
        
        // PFX Parent:
        private Transform _pfxParent;
        
        // Values:
        [SerializeField] private float _dashOffsetX = 2f;
        [SerializeField] private float _dashOffsetY = 2f;
        [SerializeField] private float _doubleJumpOffsetX = 1f;
        [SerializeField] private float _doubleJumpOffsetY = 2f;


        private void Awake(){
            
            // Fetch components:
            _playerDataScript = GetComponent<PlayerData>();
            _lightDetectionScript = GetComponent<LightDetection>();
            _pfxParent = GameObject.FindGameObjectWithTag("PFXParent").transform;
        }

        internal void SpawnLandPfx(){
            
            // Spawn light leaves:
            if (_lightDetectionScript._inLight){
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves-Light"),
                    _groundCheckScript._transform.position,
                    Quaternion.identity);
            }
            // Spawn shadow leaves:
            else{
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves"),
                    _groundCheckScript._transform.position,
                    Quaternion.identity);
            }
        }
        internal void SpawnDashPfx(){
            
            // Player facing right, spawn pfx to go left:
            if (_playerDataScript._isFacingRight){
                Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Dash-Burst-Right"),
                    new Vector3(transform.position.x - _dashOffsetX, transform.position.y - _dashOffsetY,
                        transform.position.z), Quaternion.identity, _pfxParent);
            }
            else{
                // Player facing right, spawn pfx to go right:
                Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Dash-Burst-Left"),
                    new Vector3(transform.position.x + _dashOffsetX, transform.position.y - _dashOffsetY,
                        transform.position.z), Quaternion.identity, _pfxParent);
            }
        }
        internal void SpawnDashDownPfx(){
            
            Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Dash-Burst-Down"),
                new Vector3(transform.position.x, transform.position.y - _dashOffsetY,
                    transform.position.z), Quaternion.identity, _pfxParent);
        }
        internal void SpawnDoubleJumpPfx(){
           
            // Spawn first wing pfx:
            Instantiate(UnityEngine.Resources.Load<GameObject>
                    ("Prefabs/PFX/Player/Double-Jump-0"),
                new Vector3(transform.position.x - _doubleJumpOffsetX, transform.position.y - _doubleJumpOffsetY,
                    transform.position.z), Quaternion.identity, _pfxParent);
            // Spawn other wing pfx:
            Instantiate(UnityEngine.Resources.Load<GameObject>
                    ("Prefabs/PFX/Player/Double-Jump-1"),
                new Vector3(transform.position.x + _doubleJumpOffsetX, transform.position.y - _doubleJumpOffsetY,
                    transform.position.z), Quaternion.identity, _pfxParent);
        }
        internal void SpawnDamagedPfx(){
          
        Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/VFX/Player/Player-Damaged-VFX"), new 
            Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), Quaternion.identity, 
            _pfxParent);
        Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Damaged"), new 
                Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), Quaternion.identity, 
            _pfxParent);
        }
        internal void SpawnArmourSparkPfx(){
          
            Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Enemy/Enemy-Sparks"), new 
                    Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, 
                _pfxParent);
        }
    }
}
