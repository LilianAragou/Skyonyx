using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour {

    [HideInInspector] public int degats = 1;
    [HideInInspector] public float lifeTime = 1.5f;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D truc) {
        if (truc.tag == "Player") {
            truc.SendMessage("takeDamage", degats);
            Destroy(gameObject);
        }
        else if (!truc.isTrigger && truc.tag != "Batard") {
            Destroy(gameObject);
        }
    }
}
