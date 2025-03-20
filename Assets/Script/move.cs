using System;
using UnityEngine;

public class move : MonoBehaviour {

    public float speed = 5f;
    public float jump = 8f;
    private Rigidbody2D rb;

    private CapsuleCollider2D monColl;
    private Collider2D[] colls;
    private bool grounded;
    private float rayonDetection;
    private SpriteRenderer skin;
    private Animator anim;


    void Start() {
        monColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        groundCheck();
        moveCheck();
        flipCheck();
        animCheck();
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

    private void flipCheck() {
        if(Input.GetAxis("Horizontal") > 0) {
            skin.flipX = false;
        }

        if (Input.GetAxis("Horizontal") < 0) {
            skin.flipX = true;
        }
    }

    void animCheck() {
        anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocityX));
        anim.SetFloat("velocityY", rb.linearVelocityY);
        anim.SetBool("grounded", grounded);
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
