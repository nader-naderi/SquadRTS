
using UnityEngine;

namespace NDRSquad
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;  // Prefab of the projectile to fire
        [SerializeField] private Transform projectileSpawnPoint;  // Point where the projectile should spawn
        [SerializeField] private float projectileSpeed = 10f;  // Speed at which the projectile should move
        [SerializeField] private float fireRate = 0.5f;  // Delay between shots
        private float fireTimer = 0f;

        private void Fire()
        {
            // Spawn a new projectile at the specified spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            // Get the rigidbody component of the projectile and set its velocity to the desired speed in the forward direction
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = projectileSpawnPoint.forward * projectileSpeed;
        }
    }
}
