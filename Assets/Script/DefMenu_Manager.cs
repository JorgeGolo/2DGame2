using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefMenu_Manager : MonoBehaviour
{
    
    public static DefMenu_Manager instance;

    void Awake()
    {
         if(DefMenu_Manager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
