using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {

    public float HP;


    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;
    
    
    public Transform groundChecker;
    public float groundRadius = 0.0002f;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform FlipChecker;
    public float flipRadius = 0.0002f;
    public LayerMask whatIsFlipground;
    public bool flip;
    public float forward;




    public float jumptimer;
    private float nextJump;
    public float jumpPower;
    
    Animator anim;
   

    // Use this for initialization
    void Start () {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag == "Projectile") {
            Destroy(col.gameObject);
            HP = HP-1;
        }
            

    }

    // Update is called once per frame
    void Update () {

       

        if (HP <= 0) {
            Destroy(gameObject);
        }
        flip = Physics2D.OverlapCircle(FlipChecker.position, flipRadius, whatIsFlipground);
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, whatIsGround);
        


        if (!flip){ 

            Flip();


        }

        

        //always move forward
        if (grounded &&(Time.time > nextJump))
        {
            Vector2 myVel = myBody.velocity;
            myVel = new Vector2(forward,jumpPower);
            myBody.velocity = myVel;
            nextJump = Time.time + jumptimer;
        }

    }

   

    void Flip() {

        Vector3 theScale = transform.localScale;

        //flip the x axis
        theScale.x *= -1;

        //apply that to the local scale
        transform.localScale = theScale;
        forward *= -1;

        

    }


}
