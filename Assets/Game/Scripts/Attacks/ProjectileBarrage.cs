using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBarrage : MonoBehaviour
{
    [SerializeField] GameObject[] projectile;
    [SerializeField] Transform[] barrageSpawnPoints;

   

    [Header("Barrage Settings")]
     [SerializeField] BarrageMode barrageMode;
    [SerializeField]float barrageInterval = 1f;
    [SerializeField]int waveAmount = 3;

    int waveCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Barrage(barrageInterval, waveAmount));
    }
    public void FireWaveBarrage()
    {
        transform.Rotate(transform.eulerAngles+new Vector3(0,15,0));
        foreach(Transform spawnPoint in barrageSpawnPoints)
        {
            //Instantiate(projectile, transform.position, transform.rotation);
            FireSingle(spawnPoint);
        }
    }
    IEnumerator Barrage(float interval, int waveAmount)
    {
        FireBarrage();
        waveCounter++;
        while (waveCounter < waveAmount)
        {
            yield return new WaitForSeconds(interval);

            FireBarrage();
            waveCounter++;
        }
    }

    private void FireBarrage()
    {
        if (barrageMode == BarrageMode.wave)
            FireWaveBarrage();
        else if (barrageMode == BarrageMode.serial)
        {
            int posIndex = waveCounter;
            while(posIndex>=barrageSpawnPoints.Length)
            {
                posIndex -= barrageSpawnPoints.Length;
            }
            FireSingle(barrageSpawnPoints[posIndex]);

        }
            
        else if (barrageMode == BarrageMode.random)
            FireSingle(barrageSpawnPoints[UnityEngine.Random.Range(0, barrageSpawnPoints.Length)]);
    }

    private void FireSingle(Transform spawnPoint)
    {
        if(projectile.Length>1)
        Instantiate(projectile[UnityEngine.Random.Range(0,projectile.Length)], spawnPoint.position, spawnPoint.rotation);
        else
        Instantiate(projectile[0], spawnPoint.position, spawnPoint.rotation);
    }
}
public enum BarrageMode
{
    wave,
    serial,
    random
    
} 
