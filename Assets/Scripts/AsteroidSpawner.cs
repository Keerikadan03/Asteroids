using UnityEngine;
using UnityEngine.UI;

public class AsteroidSpawner : MonoBehaviour
{   
    public static AsteroidSpawner instance;
    public Text waveText;
    public Asteroid asteroidPrefab;

    public float trajectoryVariance = 15.0f; //changing angle of trajectory

    public float spawnRate = 2.0f; //set spawnRate to 2 sec
    public int spawnAmount = 1; //no. of asteroids spawning at a time
    public float spawnDistance = 15.0f;

    int wave = 1;
        
    void Awake(){
        instance = this;
    }

    private void Start(){
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
        waveText.text = "WAVE: " + wave.ToString();
    }

    public void AddWave(){
        wave++;
        waveText.text = "WAVE: " + wave.ToString();
    }

    private void Spawn(){
        for(int i = 0; i < this.spawnAmount*wave; i++){
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;// setting the spawn direction to the border of a circle with centre at origin
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-this.trajectoryVariance,this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);

            // asteroid.gameObject.layer = LayerMask.NameToLayer("IgnoreAsteroidCollisions");
            // Invoke(nameof(TurnOnCollisions), 3.0f);
        }
    }

    // private void TurnOnCollisions(){
    //     this.asteroidPrefab.gameObject.layer = LayerMask.NameToLayer("Asteroid");
    // }
}
