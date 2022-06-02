using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{
        
        [SerializeField] internal Collider2D _boxCollider;

        [SerializeField] private bool _dashRecovery;
    }
}
