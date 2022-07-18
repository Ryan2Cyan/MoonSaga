using System;
using UnityEngine;

namespace Resources.Scripts.Test{
    public class Launch : MonoBehaviour{
        
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _target;
        [SerializeField] private float _initVel;
        [SerializeField] private float _tempVel = 0f;

        private void Awake(){
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
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
            
            Debug.Log("Vector2: " + shootVec);
            
            // Multiply by velocity:
            _rigidbody2D.velocity = shootVec * _initVel;

        }

        private void FixedUpdate(){
            Debug.Log(CalcShootAngleDiffY());
        }

        private float CalcShootAngleSameY(){
            float x = Mathf.Abs(_target.position.x - transform.position.x);
            float pt1 = -Physics.gravity.y * x;
            Debug.Log("Pt1: " + pt1);
            float pt2 = _initVel * _initVel;
            Debug.Log("Pt2: " + pt2);
            float theta = 0.5f * Mathf.Asin(pt1 / pt2);
            Debug.Log("Theta: " + theta);
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
        
    }
}
