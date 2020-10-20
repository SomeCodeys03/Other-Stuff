using Actors;
using Audio;
using CameraEffects;
using UnityEngine;

namespace Weapons
{
    public class Barrel : MonoBehaviour
    {
        //i did this in a really DUMB way, seriously. On youtube there are so many better ways using only like 10 lines of code

        public GameObject explosionFx;
        private GameObject particles;

        [SerializeField]
        private float damage = 35f;

        private SpriteRenderer sr;
        private CircleCollider2D explosionCol;
        private BoxCollider2D box;
        private Rigidbody2D rb;

        [HideInInspector]
        public static bool isExploding = false;

        public static Barrel instance;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            explosionCol = GetComponent<CircleCollider2D>();
            box = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Explode()
        {
            isExploding = true;
            AudioManager.Play("BarrelExplode");
            CameraShaker.ShakeOnce(5f, 1.8f);
            explosionCol.radius = 3f;
            particles = Instantiate(explosionFx, transform.position, transform.rotation);
            Invoke("AfterExplosion", 0.2f);
            Destroy(particles, 1f);
            Invoke("Destroy", 1.8f);
        }

        private void AfterExplosion()
        {
            sr.sprite = null;
            Destroy(rb);
            Destroy(explosionCol);
            Destroy(box);
        }

        private void Destroy()
        {
            isExploding = false;
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Actor"))
            {
                Actor hitActor = other.GetComponent(typeof(Actor)) as Actor;
                Vector2 dir = other.transform.position - transform.position;

                hitActor.TakeDamage(damage);
                hitActor.GetRb().AddForce(dir * 5f, ForceMode2D.Impulse);
            }
        }
    }
}
