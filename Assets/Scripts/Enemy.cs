using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 8f;

    [HideInInspector] public Transform player;
    [HideInInspector] public Camera camara;

    private Vector3 direccion;

    void Start()
    {
        if (player != null)
            direccion = (player.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;
        ComprobarSiPasoLaCamara();
    }

    void ComprobarSiPasoLaCamara()
    {
        if (camara == null) return;

        Vector3 hastaEnemigo = transform.position - camara.transform.position;
        if (Vector3.Dot(hastaEnemigo, camara.transform.forward) < 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name);
        ManejarColision(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        ManejarColision(collision.gameObject);
    }

    void ManejarColision(GameObject objetivo)
    {
        Debug.Log("Colision con: " + objetivo.name);
        if (!objetivo.CompareTag("Player")) return;

        Destroy(objetivo);
        Destroy(gameObject);

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }
}
