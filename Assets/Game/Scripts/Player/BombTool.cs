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
    [SerializeField] float placingBombRange =2f;
    SphereCollider bombcollider;

    bool canBomb = true;

    private void Awake() {
        bombcollider = bombPrefab.GetComponent<SphereCollider>();
    }
    private void OnEnable() {
        GetComponent<PlayerController>().onUseTool+=PutBomb;
    }
    private void OnDisable() {
        GetComponent<PlayerController>().onUseTool-=PutBomb;
    }
    public void PutBomb(Transform userTransform)
    {
        if(!gotBombTool) return;
        if(!canBomb) return;
        StartCoroutine(PlacingBombRoutine(userTransform));
    }
    IEnumerator PlacingBombRoutine(Transform userTransform)
    {
        canBomb =false;
        if(Physics.Raycast(userTransform.position+new Vector3(0,1.4f,0),userTransform.forward,out RaycastHit hitInfo,placingBombRange))
        {
            Vector3 pointDirection = hitInfo.point - userTransform.position;
            pointDirection = new Vector3(pointDirection.x,0, pointDirection.z);
            Instantiate(bombPrefab,hitInfo.point+(pointDirection- userTransform.forward*(4*bombcollider.radius)),Quaternion.identity);

        }
        else
            Instantiate(bombPrefab,userTransform.position+(placingBombRange*userTransform.forward)+new Vector3(0,1.4f,0)- userTransform.forward*(3*bombcollider.radius),Quaternion.identity);

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
