using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    [SerializeField] LayerMask groundLayer;

    //  TODO improve it
    [SerializeField] GameObject player;

    private void Awake() {
        if(Instance && Instance!=this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    // Update is called once per frame
   public void ReloadLevel()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    public LayerMask GetGroundLayer()
    {
        return groundLayer;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
