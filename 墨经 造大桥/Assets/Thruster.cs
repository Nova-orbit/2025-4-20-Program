using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float thrust = 20f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Global.StartButtonOn == true)
        {
            rb.AddForce(Vector2.right *  thrust, ForceMode2D.Force);
        }
        
    }
}
