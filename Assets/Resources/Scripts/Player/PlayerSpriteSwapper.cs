using Resources.Scripts.Lighting;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerSpriteSwapper : MonoBehaviour{
        
        private LightDetection _lightDetectionScript;
        private PlayerData _playerDataScript;

        private void Awake(){

            _lightDetectionScript = GetComponent<LightDetection>();
            _playerDataScript = GetComponent<PlayerData>();
        }

        private void FixedUpdate(){
            switch (_lightDetectionScript._inLight){
                case true:
                    _playerDataScript._hoodUpSprite.SetActive(true);
                    _playerDataScript._hoodDownSprite.SetActive(false);
                    break;
                case false:
                    _playerDataScript._hoodUpSprite.SetActive(false);
                    _playerDataScript._hoodDownSprite.SetActive(true);
                    break;
            }
        }
    }
}
