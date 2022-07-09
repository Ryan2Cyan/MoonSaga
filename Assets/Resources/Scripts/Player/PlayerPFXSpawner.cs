using Resources.Scripts.General;
using Resources.Scripts.Lighting;
using UnityEngine;

// Code within this class is responsible for handling particle
// effects in relation to the player model:
namespace Resources.Scripts.Player{
    public class PlayerPFXSpawner : MonoBehaviour{
        // Scripts:
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private LightDetection _lightDetectionScript;
        [SerializeField] private GroundCheck _groundCheckScript;
        
        // PFX Parent:
        [SerializeField] private Transform _pfxParent;
        
        // Values:
        [SerializeField] private float _dashOffsetX = 2f;
        [SerializeField] private float _dashOffsetY = 2f;
        [SerializeField] private float _doubleJumpOffsetX = 1f;
        [SerializeField] private float _doubleJumpOffsetY = 2f;

        internal void SpawnLandPfx(){
            // If player in light, spawn light leaves:
            if (_lightDetectionScript._inLight){
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves-Light"),
                    _groundCheckScript._groundCheck.position,
                    Quaternion.identity);
            }
            // If player in light, spawn shadow leaves:
            else{
                Instantiate(UnityEngine.Resources.Load<GameObject>
                        ("Prefabs/Environment/CelestialGrove/PFX/Land-Leaves"),
                    _groundCheckScript._groundCheck.position,
                    Quaternion.identity);
            }
        }
        internal void SpawnDashPfx(){
            
            if (_playerMovementScript._isFacingRight){
                // Player facing right, spawn pfx to go left:
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
            
            // Player facing right, spawn pfx to go left:
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
    }
}
