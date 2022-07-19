using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Resources.Scripts.Test{
    public class Launch : MonoBehaviour{
        
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _target;
        [SerializeField] private float _initVel;
        private bool _shoot;

        private void Awake(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update(){
            if (!_shoot)
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            else if(_shoot)
                _rigidbody2D.constraints = RigidbodyConstraints2D.None;
        }

        public void Shoot(){
            _shoot = true;
            // Calc angle:
            float theta = CalcShootAngleDiffY();

            // Calc direction:
            Vector2 norm = _target.position - transform.position;
            Vector2 shootVec = Vector2.zero;;
            if(norm.x >= 0f)
                shootVec.x = Mathf.Cos(theta);
            else{
                shootVec.x = -Mathf.Cos(theta);
            }
            shootVec.y = Mathf.Sin(theta);

            // Multiply by velocity:
            _rigidbody2D.velocity = shootVec * _initVel;
        }
        
        private float CalcShootAngleSameY(){
            float x = Mathf.Abs(_target.position.x - transform.position.x);
            float pt1 = -Physics.gravity.y * x;
            float pt2 = _initVel * _initVel;
            float theta = 0.5f * Mathf.Asin(pt1 / pt2);
            return theta;
        }
        
        private float CalcShootAngleDiffY(){
            float x = Mathf.Abs(_target.position.x - transform.position.x);
            float y = Mathf.Abs(_target.position.y - transform.position.y);

            float pt1 = -Physics.gravity.y * Mathf.Pow(x, 2) / Mathf.Pow(_initVel, 2) - y;
            float pt2 = pt1 / Mathf.Sqrt(Mathf.Pow(y, 2) + Mathf.Pow(x, 2));
            float pt3 = Mathf.Acos(pt2);
            float face = Mathf.Atan(x / y);
            float pt4 = pt3 + face;
            float theta = pt4 / 2f;

            return theta;
        }

        private void OnCollisionEnter2D(Collision2D other){
            _shoot = false;
            _target.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0.16f);
            transform.position = Vector3.zero;
        }
    }
}
