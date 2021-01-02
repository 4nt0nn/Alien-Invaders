using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;   
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int i = startingWave; i < waveConfigs.Count; i++)
        {
            var currentWaveConfig = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWaveConfig));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWaveConfig)
    {
        for(int i = 0; i < currentWaveConfig.GetNumberOfEnemies(); i++)
        {
            var newEnemy = Instantiate(currentWaveConfig.GetEnemyPrefab(), currentWaveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(currentWaveConfig);

            yield return new WaitForSeconds(currentWaveConfig.GetTimeBetweenSpawns());
        }
    }
}
