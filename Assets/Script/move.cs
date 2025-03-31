using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 8f;
    public float wallJumpForce = 6f;
    public Transform shadowPrefab;

    private Rigidbody2D rb;
    public CapsuleCollider2D monColl;
    private Collider2D[] colls;
    private bool grounded;
    private float rayonDetection;
    private bool wallSliding;
    private bool touchingWallLeft, touchingWallRight;
    private SpriteRenderer skin;
    private Animator anim;
    private Transform shadow;
    private float lastWallJumpTime;
    public LayerMask groundLayer;
    public Transform groundChecker;

    void Start()
    {
        
        monColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Création de l'ombre
        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f); // Ombre semi-transparente
    }

    void Update()
    {
        groundCheck();
        moveCheck();
        flipCheck();
        animCheck();
        shadowUpdate();

        // Échange de position avec l'ombre
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapWithShadow();
        }
    }

    void groundCheck() {
        grounded = false;
        rayonDetection = monColl.size.x * 0.45f;
        colls = Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.up * (monColl.offset.y + rayonDetection * 0.8f - (monColl.size.y / 2)), rayonDetection);
        foreach(Collider2D coll in colls) {
            if(coll != monColl && !coll.isTrigger) {
                grounded = true; 
                break;
            }
        }
    }

    void moveCheck() {
        rb.linearVelocityX = Input.GetAxis("Horizontal") * speed;

        if (Input.GetButtonDown("Jump") && grounded) {
            rb.linearVelocityY = jump;
        }
    }


    void flipCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            skin.flipX = Input.GetAxis("Horizontal") < 0;
        }
    }

    void animCheck()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("velocityY", rb.linearVelocity.y);
        anim.SetBool("grounded", grounded);
    }

    void shadowUpdate()
    {
        if (shadow != null)
        {
            Vector3 mirroredPosition = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            shadow.position = mirroredPosition;
            shadow.rotation = transform.rotation;
        }
    }

    void SwapWithShadow()
    {
        if (shadow != null)
        {
            Vector3 tempPosition = transform.position;
            transform.position = shadow.position;
            shadow.position = tempPosition;
        }
    }
    private void OnDrawGizmos() {
        if (monColl == null) {
            monColl = GetComponent<CapsuleCollider2D>();
        }
        rayonDetection = monColl.size.x * 0.45f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.up * (monColl.offset.y + rayonDetection * 0.8f - (monColl.size.y / 2)), rayonDetection);
    }
}