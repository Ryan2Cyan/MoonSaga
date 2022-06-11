using System;
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
            
            // Change animation based on enemy's current state:
            ProcessStateAnimation();
        }

        private void ResetAnimator(){
            
            _animator.SetBool(Damaged, false);
        }

        private void ProcessStateAnimation(){
            ResetAnimator();
            switch (_state){
                case enemyMoveState.Walking:
                    break;
                case enemyMoveState.Damaged:
                    _animator.SetBool(Damaged, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
