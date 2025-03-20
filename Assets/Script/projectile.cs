using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    // Script a mettre sur votre projectile que vous allez tirer depuis le script attackDIST
    // IL doit y avoir un TRIGGER sur l'objet et un rigidbody

    [HideInInspector] public int degats = 1;          // Les dégats du projectile
    [HideInInspector] public float lifeTime = 3.0f;   // Le temps maximal que vivra le projectile (pour être sur qu'il se détruise au bout d'un moment)
    public GameObject ExplosionEffect;
    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    // La fonction OnTriggerEnter s'enclenche quand votre Trigger touche un autre collider/trigger
    void OnTriggerEnter2D(Collider2D truc) {
        if (truc.tag == "Batard") {                 // Si le truc qu'on touche a le tag "Ennemi"
            truc.SendMessage("takeDamage", degats); // On cherche sur lui une fonction qui s'appel "takeDamage", et on la lance en lui donnant le nombre de dégat qu'on fait
            explode();
        }
        else if (truc.tag == "balle") {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), truc);
        }
        else if (!truc.isTrigger && truc.tag != "Player") {     // Sinon si on touche un mur (un collider qui n'est PAS un trigger) et que ce n'est pas le joueur
            explode();
        }
    }
    void explode() {
        GameObject ExplosionEffectIns = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(ExplosionEffectIns, 10);
        Destroy(gameObject);
    }
}
