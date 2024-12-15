using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistanceManager : MonoBehaviour
{
    #region Singleton

    private static PersistanceManager _instance;
    public static PersistanceManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveTimePassed(float time)
    {
        PlayerPrefs.SetFloat("timePassed", time);
    }
    public float GetTimePassed()
    {
        return PlayerPrefs.GetFloat("timePassed");
    }
}
