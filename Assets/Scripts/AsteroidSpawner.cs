using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//Ova skripta kontrolira kako se stvaraju objekti Asteroid
public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float spawnRate = 2.0f;
    public int spawnAmount = 1;
    public float spawnDistance = 15.0f;
    public float trajectoryVariance = 15.0f;

    //U funkciji Start govorimo da se funkcija Spawn zapocinje i ponavlja svakih spawnRate sekundi
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    //Mrzim ovu skriptu, mrzim ovu skriptu, mrzim ovu skriptu
    //U funkciji Spawn govorimo kako se stvaraju objekti Asteroid
    private void Spawn()
    {
        for(int i = 0; i < this.spawnAmount; i++)
        {
            //spawnDirection odreduje od kuda ce se Asteroid stvoriti, a spawn point u kojem smjeru
            Vector3 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            
            //necu ni pokusavati ovo objasniti, bitno da radi
            float variance = UnityEngine.Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            
            //Instancira Asteroid i odreduje mu velicinu i rotaciju
            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = UnityEngine.Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
