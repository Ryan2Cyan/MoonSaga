using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.VFX{
    public class ShadowSapphire : MonoBehaviour{
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;
        private GameObject[] _sceneEnemies;
        private void Awake(){
            
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            // Apply force on spawn:
            _rigidbody2D.AddForce(new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY)));
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                }
            }
        }
    }
}
