using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            
            StartCoroutine(nameof(Test));
        }
    }

    IEnumerator Test()
    {
        Debug.Log("Hello world");
        yield return new WaitForSeconds(10);
    }
}
