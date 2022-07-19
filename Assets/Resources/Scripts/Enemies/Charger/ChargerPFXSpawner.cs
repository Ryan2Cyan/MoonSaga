using Resources.Scripts.Enemies.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.Charger{
    public class ChargerPFXSpawner : EnemyPFXSpawner
    {
        internal void SpawnArmourSparkPfx(){
            
            Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/PFX/Enemy/Enemy-Sparks"), new 
                    Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }
}
