using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombTool : MonoBehaviour
{
    public UnityAction onPlaceBomb;
    public UnityAction onBombCooldownEnds; 
    public UnityAction onLearnBombTool;
    [Header("Bombs")]

    [SerializeField] bool gotBombTool= false;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombIntervals = 3f;
    bool canBomb = true;
    public void PutBomb()
    {
        if(!gotBombTool) return;
        if(!canBomb) return;
        StartCoroutine(PlacingBombRoutine());
    }
    IEnumerator PlacingBombRoutine()
    {
        canBomb =false;
        Instantiate(bombPrefab,transform.position+2*(transform.forward),Quaternion.identity);
        onPlaceBomb?.Invoke();
        yield return new WaitForSeconds(bombIntervals);
        onBombCooldownEnds?.Invoke();
        canBomb = true;
    }

    public void LearnBombSkill()
    {
        gotBombTool = true;
        onLearnBombTool?.Invoke();
    }
}
