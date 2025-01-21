using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    private Image healthBar;

    //Encapsular para que des de otro script se pueda acceder
    public Image HealthBar { get => healthBar; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
