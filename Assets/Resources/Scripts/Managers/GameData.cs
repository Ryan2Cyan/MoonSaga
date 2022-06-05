using UnityEngine;

namespace Resources.Scripts.Managers{
    public class GameData : MonoBehaviour{
        
        // Data manager game object:
        [SerializeField] private GameObject _dataManager;
        
        [SerializeField] internal int hitPoints = 5;
        [SerializeField] internal int maxPoints = 5;

        private void Awake(){
            DontDestroyOnLoad(_dataManager);
        }
    }
}
