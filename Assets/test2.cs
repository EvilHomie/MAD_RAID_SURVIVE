using Unity.VisualScripting;
using UnityEngine;

public class test2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnBecameVisible()
    {
        Debug.Log(1);
    }
    private void OnBecameInvisible() 
    {
        Debug.Log(0);
    }
}
