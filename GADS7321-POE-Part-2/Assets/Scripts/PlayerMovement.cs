using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horixontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horixontalInput*speed, body.velocity.y);
        
        //Flip player when moving left and right
        if (horixontalInput > 0.01f) 
            transform.localScale = Vector3.one;
        else if (horixontalInput < -0.01f) 
            transform.localScale = new Vector3(-1,1,1);
        
        if (Input.GetKey(KeyCode.Space)) 
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        
        
        anim.SetBool("run",horixontalInput != 0);
    }

    private void Jumping()
    {
        
    }
}
