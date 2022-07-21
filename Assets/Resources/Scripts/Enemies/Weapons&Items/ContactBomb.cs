using UnityEngine;

namespace Resources.Scripts.Enemies{
    public class ContactBomb : MonoBehaviour{
        
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _xMod;
        [SerializeField] private float _explosionTime;
        private float _explosionTimer;
        [SerializeField] private float _initVel;
        private Transform _playerTransform;
        
        private void Awake(){
            
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            // Set timer:
            _explosionTimer = _explosionTime;
            
            
            // Calc angle and direction to shoot the bomb:
            float theta = CalcShootAngleDiffY();
            Vector2 norm = _playerTransform.position - transform.position;
            Vector2 shootVec = Vector2.zero;
            shootVec.x = norm.x >= 0f ? shootVec.x = Mathf.Cos(theta) + Random.Range(-0f, _xMod) : 
                shootVec.x = -Mathf.Cos(theta) - Random.Range(-0f, _xMod);
            shootVec.y = Mathf.Sin(theta);
            // Multiply by velocity (shoot):
            _rigidbody2D.velocity = shootVec * _initVel;
        }

        private void Update(){
            
            // Timer runs out [Explode]:
            _explosionTimer -= Time.deltaTime;
            if (_explosionTimer <= 0f){
                // Spawn Explosion:
                Destroy(gameObject);
            }
        }

        private float CalcShootAngleDiffY(){
            float x = Mathf.Abs(_playerTransform.position.x - transform.position.x);
            float y = Mathf.Abs(_playerTransform.position.y - transform.position.y);

            float pt1 = -Physics.gravity.y * Mathf.Pow(x, 2) / Mathf.Pow(_initVel, 2) - y;
            float pt2 = pt1 / Mathf.Sqrt(Mathf.Pow(y, 2) + Mathf.Pow(x, 2));
            float pt3 = Mathf.Acos(pt2);
            if (!float.IsNaN(pt3)){
                float face = Mathf.Atan(x / y);
                float pt4 = pt3 + face;
                float theta = pt4 / 2f;
                return theta;
            }
            return 1;
        }
        
        private void OnTriggerEnter2D(Collider2D other){
            
            // Collision with player [Explode]:
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Platform")
            || other.gameObject.CompareTag("PlatformEdge")){
                // Spawn Explosion:
                Destroy(gameObject);
            }
        }
    }
}
