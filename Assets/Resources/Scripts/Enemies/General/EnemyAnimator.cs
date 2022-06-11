using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyAnimator : MonoBehaviour{
        
        // Scripts:
        private EnemyMovement _enemyMovementScript;
        
        // Values:
        private Animator _animator;
        private enemyMoveState _state;
        
        // Property Indexes:
        private static readonly int Damaged = Animator.StringToHash("Damaged");


        private void Awake(){

            _enemyMovementScript = transform.parent.GetComponent<EnemyMovement>();
            _animator = GetComponent<Animator>();
        }

        private void Update(){
            
            _state = _enemyMovementScript._state;
        }
    }
}
