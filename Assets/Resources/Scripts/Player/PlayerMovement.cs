using System;
using UnityEngine;
using UnityEngine.Events;

// Code within this class is responsible (only) for the movement of the 
// player character.
namespace Resources.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour{
        private Rigidbody2D _rigidbody2D;

        public UnityEvent OnLandEvent; 

        private void Awake(){
            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();
        }
    }
}
