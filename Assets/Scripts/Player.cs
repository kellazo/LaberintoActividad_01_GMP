using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RollingBall
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float velocidad;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(gameObject.transform.position.z);
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                //transform.position = transform.position + new Vector3(0, 0, 1) * velocidad * Time.deltaTime;
                transform.position += new Vector3(0, 0, 1) * velocidad * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, 0, -1) * velocidad * Time.deltaTime;
            }
        }
    }
}