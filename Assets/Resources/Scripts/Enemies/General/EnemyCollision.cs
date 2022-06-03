using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyCollision : MonoBehaviour{

        // Trigger collider:
        [SerializeField] internal Collider2D _boxCollider;

        // Values:

        private void Awake(){
            // Fetch values:
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // Ignore collisions with player:
            Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
        }
    }
}
