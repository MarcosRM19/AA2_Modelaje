using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{
    public GameObject asteroidPrefab;   // Prefab of the asteroid
    public Transform spawner;           // Reference to the airship
    public float spawnRadius;     // Radius around the airship to spawn asteroids
    public float noSpawnConeAngle; // Angle of the no-spawn cone (in degrees)
    public float spawnRate;        // Time between spawns

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            SpawnAsteroid();
            spawnTimer = 0f;
        }
    }

    void SpawnAsteroid()
    {
        // Generate a random direction within a sphere
        Vector3 randomDirection = Random.onUnitSphere;

        // Exclude the "no-spawn" cone above the airship
        float angleFromUp = Vector3.Angle(Vector3.down, randomDirection); // Angle from downward direction
        if (angleFromUp < noSpawnConeAngle / 2f)
        {
            return; // Skip this spawn and try again in the next frame
        }

        // Calculate the spawn position
        Vector3 spawnPosition = spawner.position + randomDirection * spawnRadius;

        // Instantiate the asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
    }
}
