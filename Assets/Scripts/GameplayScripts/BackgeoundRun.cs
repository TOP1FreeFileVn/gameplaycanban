using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public float backgroundWidth = 11.52f;
    public float scrollSpeed = 5f;
    private float resetPosition;
    
    void Start()
    {
        resetPosition = backgroundWidth; 
    }
    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime, 0) ;
        if(transform.position.x < -resetPosition)
        {
            float offset = backgroundWidth * 2f;
            transform.position = new Vector3(transform.position.x + offset,transform.position.y ,transform.position.z);
        }

    }
}
