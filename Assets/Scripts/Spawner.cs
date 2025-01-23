using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private EnemyAI enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity); //Quaternion.Euler(0,0,0) = Quaternion.identity, matriz identidad (diagonal de unos), nulo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
