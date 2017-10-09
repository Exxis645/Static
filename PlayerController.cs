using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //player HP
    public float PlayerHealth = 3;
    public float MaxHP = 3;
    public GameObject HP1;
    public GameObject HP2;
    public GameObject HP3;
    public float damageTimer;
    private float moveTimer;
    public bool canMove;
    public bool canJump;
    public bool ledgeGrabbed;
    private Vector3 currentPos;
    private Rigidbody2D rb;
    public bool canGrab = true;
   

    public AudioSource soundPlayer;
    public AudioClip jumpSound;
    public AudioClip grabSound;
   

  

    
    //canvasObject.SetActive(false);


    //how fast the player moves
    public float topSpeed = 1f;
    //facing right?
    bool facingRight = true;

    //get reference to animator
    Animator anim;

    //ground check
    private bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.0002f;

    public float jumpForce = 250f;
    public LayerMask whatIsGround;

   




    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        PlayerHealth = 3;
        rb = GetComponent<Rigidbody2D>();
        soundPlayer.clip = jumpSound;




    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //health UI
        HealthManagment();

        // true or false, did the ground transform hit the ground layermask with the ground radius
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        

        //tell the animator we are grounded
        anim.SetBool("Grounded", grounded);

        //vert speed
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (Time.time > moveTimer) {
            canMove = true;
            anim.SetBool("damage", false);
        }


        //get move direction
        if (canMove)
        {
            float move = Input.GetAxis("Horizontal");

            //add velocity to the rigidbody in the move direction * the speed

            GetComponent<Rigidbody2D>().velocity = new Vector2(move * topSpeed, GetComponent<Rigidbody2D>().velocity.y);

            //reference speed from animator to move variable
            anim.SetFloat("Speed", Mathf.Abs(move));

            //if we are facing the left

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }
	}

     void Update()
    {


        transform.localEulerAngles = new Vector3(0, 0, 0);

        if ((grounded && Input.GetKeyDown(KeyCode.Space) && canMove && !ledgeGrabbed) || (Input.GetKeyDown(KeyCode.Space) && canJump && !ledgeGrabbed)) {
            
            //not grounded
            anim.SetBool("Grounded", false);

            //add vertical force to y axis, jump
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            soundPlayer.clip = jumpSound;
            soundPlayer.Play();
            canJump = false;
            canGrab = false;




        }

        if (grounded) {
            canJump = false;
            canGrab = true;
        }

        if (ledgeGrabbed && Input.GetKeyDown(KeyCode.Space)) {
            ledgeGrabbed = false;
            canGrab = false;
            canJump = false;
            
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            soundPlayer.clip = jumpSound;
            soundPlayer.Play();
            

        }

        
    }

    void Flip() {
        //saying we are facing the opposite directions

        facingRight = !facingRight;

        //get the local scale
        Vector3 theScale = transform.localScale;

        //flip the x axis
        theScale.x *= -1;

        //apply that to the local scale
        transform.localScale = theScale;
        


           

    }

    //collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            PlayerHealth -= 1;
            TakeDamage();
        }

        if (collision.gameObject.tag == "ledge" && canGrab) {
            soundPlayer.clip = grabSound;
            soundPlayer.Play();
            ledgeGrabbed = true;
            canJump = true;
            anim.SetBool("ledgeGrabbed", true);
            
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            jumpForce = 250f;



        }

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            PlayerHealth -= 1;
            TakeDamage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ledge") {
            
            anim.SetBool("ledgeGrabbed", false);
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            jumpForce = 250f;


        }
    }

    void HealthManagment() {
        if (PlayerHealth > MaxHP)
        {
            PlayerHealth = MaxHP;
        }
        else if (PlayerHealth == 3)
        {
            HP1.SetActive(true);
            HP2.SetActive(true);
            HP3.SetActive(true);
        }
        else if (PlayerHealth == 2)
        {
            HP3.SetActive(false);
            HP2.SetActive(true);
            HP1.SetActive(true);
        }
        else if (PlayerHealth == 1)
        {
            HP3.SetActive(false);
            HP2.SetActive(false);
            HP1.SetActive(true);
        }
        else if (PlayerHealth == 0)
        {
            HP3.SetActive(false);
            HP2.SetActive(false);
            HP1.SetActive(false);
            Destroy(gameObject);

        }
        
    }
    public void TakeDamage() {
        canMove = false;
        anim.SetBool("damage", true);
        moveTimer= Time.time + damageTimer;
    }

    
}
