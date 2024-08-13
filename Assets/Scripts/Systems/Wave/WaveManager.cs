using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //Singleton
    public static WaveManager instance { get; private set; }
    
    //Wave variables
    /// <summary>
    /// 전체 웨이브 리스트
    /// </summary>
    public List<Wave> waves = new List<Wave>();
    /// <summary>
    /// 이번 웨이브의 적 리스트
    /// </summary>
    public static List<Transform> cur_wave;
    /// <summary>
    /// 이번 웨이브의 각 적과 플레이어 간의 거리 리스트
    /// </summary>
    public static List<float> eachEnemyDistanceFromPlayer;
    /// <summary>
    /// 이번 웨이브의 적 수
    /// </summary>
    private int enemyCount = 0;
    /// <summary>
    /// 웨이브 번호
    /// </summary>
    public int waveCount = 0;
    

    // Unity Functions
    void Awake()
    {
        // Singleton
        instance = this;
    }
    void Update()
    {
        // Wave Spawn
        if (enemyCount <= 0 && waveCount < waves.Count)
            Spawn();
        
        // Calculate distance of Enemy to Player
        UpdateDistanceFromPlayer();
    }
    
    // System Functions
    void Spawn()
    {
        //List Clear
        if (cur_wave != null)                       cur_wave.Clear();
        if (eachEnemyDistanceFromPlayer != null)    eachEnemyDistanceFromPlayer.Clear();

        //New List
        cur_wave = waves[waveCount++].enemys;
        eachEnemyDistanceFromPlayer = new List<float>();

        //Spawn enemies included in current wave
        foreach (var obj in cur_wave)
        {
            obj.gameObject.SetActive(true);
            obj.GetComponent<Enemy>().SpawnSetting();
            eachEnemyDistanceFromPlayer.Add(obj.GetComponent<Enemy>().DistanceToPlayer());
        }
        enemyCount = cur_wave.Count;
    }
    void UpdateDistanceFromPlayer()
    {
        for (int i = 0; i < cur_wave.Count; i++)
            eachEnemyDistanceFromPlayer[i] = cur_wave[i].GetComponent<Enemy>().DistanceToPlayer();
    }

    // Enemy Counter
    public void EnemyCountDecrease(Transform deadEnemy)
    {
        // Remove the data of deadEnemy
        for (int i = 0; i <= cur_wave.Count; i++)
        {
            // Not found deadEnemy in List(cur_wave)
            if (i == cur_wave.Count)
                return;

            // Remove the dead Enemy from enemy list
            if (cur_wave[i] == deadEnemy)
            {
                cur_wave.RemoveAt(i);
                eachEnemyDistanceFromPlayer.RemoveAt(i);
                enemyCount--;
                break;
            }
        }
        Destroy(deadEnemy.gameObject);

        // GameOver (StageClear)
        GameManager.GameState result = GameManager.instance.gameState;
        if (result != GameManager.GameState.Win && result != GameManager.GameState.Lose)
            if (enemyCount <= 0 && waveCount >= waves.Count)
                GameManager.instance.SetGameState(GameManager.GameState.RewardSelect);
    }

    // Class Function

    /// <summary>
    /// Player로부터 가장 가까운 Enemy 반환
    /// </summary>
    /// <returns>Player로부터 가장 가까운 Enemy의 Transfrom</returns>
    public static Transform ClosestEnemyToPlayer()
    {
        if (cur_wave.Count <= 0)
            return null;

        (float, int) min = (eachEnemyDistanceFromPlayer[0], 0);
        for(int i = 1; i < eachEnemyDistanceFromPlayer.Count; i++)
        {
            if (eachEnemyDistanceFromPlayer[i] < min.Item1)
                min = (eachEnemyDistanceFromPlayer[i], i);
        }
        
        return cur_wave[min.Item2];
    }
    /// <summary>
    /// 이번 Stage의 Enemy가 모두 죽었는지 검사
    /// </summary>
    /// <returns>true : Wave가 종료됨, false : Wave가 종료되지 않음</returns>
    public static bool WaveEnd()
    {
        return eachEnemyDistanceFromPlayer == null || eachEnemyDistanceFromPlayer.Count <= 0;
    }

}