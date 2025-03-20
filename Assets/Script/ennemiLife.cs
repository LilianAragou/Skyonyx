using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemiLife : MonoBehaviour {

    public int vie = 1;
    public GameObject bloodEffect;
    public GameObject gun;
    public GameObject me;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            takeDamage(1);
        }
    }

    public void takeDamage (int damage) {

        vie = vie - damage;
        if (vie <= 0) {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            gun.SetActive(false);
            me.SetActive(false);
            
            if (GetComponent<ennemiPatrol>() != null) {
                GetComponent<ennemiPatrol>().enabled = false;
            }
            if(bloodEffect != null) {
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
