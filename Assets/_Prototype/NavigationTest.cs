using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
