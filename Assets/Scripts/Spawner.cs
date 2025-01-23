using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField]
    private EnemyAI enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        

        StartCoroutine(Spawnear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodo ejecutado por intervalos. (Rutina)
    private IEnumerator Spawnear()
    {
        while (true)
        {

            Instantiate(enemyPrefab, transform.position, Quaternion.identity); //Quaternion.Euler(0,0,0) = Quaternion.identity, matriz identidad (diagonal de unos), nulo
           // UnityEngine.Debug.Log("Verde");
            yield return new WaitForSeconds(2f);
            //Espera 2 segundos
            //UnityEngine.Debug.Log("Amarillo");
            //Espera 2 segundos
            //yield return new WaitForSeconds(2f);
            //UnityEngine.Debug.Log("Rojo");
        }
    }
}
