using Resources.Scripts.Lighting;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerSpriteSwapper : MonoBehaviour{
        
        [SerializeField] private LightDetection _lightDetectionScript;
        [SerializeField] private GameObject _hoodUpSprite;
        [SerializeField] private GameObject _hoodDownSprite;

        private void FixedUpdate(){
            switch (_lightDetectionScript._inLight){
                case true:
                    _hoodUpSprite.SetActive(true);
                    _hoodDownSprite.SetActive(false);
                    break;
                case false:
                    _hoodUpSprite.SetActive(false);
                    _hoodDownSprite.SetActive(true);
                    break;
            }
        }
    }
}
