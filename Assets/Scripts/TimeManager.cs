using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton

    private static TimeManager _instance;
    public static TimeManager Instance
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

    public float timePassed;
    public float timeScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
        timePassed = PersistanceManager.Instance.GetTimePassed();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime; // devuelve el mismo tiempo independientemente del framrate del pc que se ejecute
        PersistanceManager.Instance.SaveTimePassed(timePassed);
    }

    public void SetTimeScale()
    {
        Time.timeScale = timeScale;
    }
}
