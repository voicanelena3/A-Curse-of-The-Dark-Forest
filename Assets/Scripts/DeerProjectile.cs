using UnityEngine;

public class DeerProjectile : MonoBehaviour
{
    private int damage = 10;
    private float lifetime = 5f;
    private float speed = 20f;

    private Vector3 directionNormal;

    public void Initialize(Vector3 targetPosition)
    {
        this.directionNormal = (targetPosition - this.transform.position).normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime); // se distruge automat după câteva secunde
    }

    private void Update()
    {
        this.transform.position += directionNormal * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject); // proiectilul dispare după impact
        }
    }
}