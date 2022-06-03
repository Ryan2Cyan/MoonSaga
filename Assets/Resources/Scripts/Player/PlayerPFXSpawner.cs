using UnityEngine;

// Code within this class is responsible for handling particle
// effects in relation to the player model:
namespace Resources.Scripts.Player{
    public class PlayerPFXSpawner : MonoBehaviour{
        // Scripts:
        [SerializeField] private PlayerMovement _playerMovementScript;
        // PFX Parent:
        [SerializeField] private GameObject _pfxParent;
        
        // Values:
        [SerializeField] private float _dashOffsetX = 2f;
        [SerializeField] private float _dashOffsetY = 2f;
        [SerializeField] private float _doubleJumpOffsetX = 1f;
        [SerializeField] private float _doubleJumpOffsetY = 2f;
        internal void SpawnDashPfx(){
            
            if (_playerMovementScript._isFacingRight){
                // Player facing right, spawn pfx to go left:
                Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Dash-Burst-Right"),
                    new Vector3(transform.position.x - _dashOffsetX, transform.position.y - _dashOffsetY, 
                        transform.position.z), Quaternion.identity);
            }
            else{
                // Player facing right, spawn pfx to go right:
                Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Player/Dash-Burst-Left"),
                    new Vector3(transform.position.x + _dashOffsetX, transform.position.y - _dashOffsetY, 
                        transform.position.z), Quaternion.identity);
            }
        }

        internal void SpawnDoubleJumpPfx(){
            Instantiate(UnityEngine.Resources.Load<GameObject>
                    ("Prefabs/PFX/Player/Double-Jump-0"), 
                new Vector3(transform.position.x - _doubleJumpOffsetX, transform.position.y - _doubleJumpOffsetY,
                    transform.position.z),
                Quaternion.identity);
            Instantiate(UnityEngine.Resources.Load<GameObject>
                    ("Prefabs/PFX/Player/Double-Jump-1"), 
                new Vector3(transform.position.x + _doubleJumpOffsetX, transform.position.y - _doubleJumpOffsetY,
                    transform.position.z),
                Quaternion.identity);
        }
    }
}
