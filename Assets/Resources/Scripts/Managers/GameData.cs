using UnityEngine;

// Code within this class is a responsible for all data that
// needs to be saved between scenes and re-loads:
namespace Resources.Scripts.Managers{
    public class GameData : MonoBehaviour{
    
        // Data manager game object:
        [SerializeField] private GameObject _dataManager;
    
        [SerializeField] internal int hitPoints = 5;
        [SerializeField] internal int maxPoints = 5;
        [SerializeField] internal int shadowSapphires;

        private void Awake(){
            DontDestroyOnLoad(_dataManager);
        }
    }
}

