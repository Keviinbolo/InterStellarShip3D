using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 8f;

    [Header("Puntuacion")]
    public int puntosPorKill = 500;

    [HideInInspector] public Transform player;
    [HideInInspector] public Camera camara;

    private Vector3 direccion;
    private bool _destruido = false;

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

    // Llamado por la bala al impactar
    public void Morir()
    {
        if (_destruido) return;
        _destruido = true;

        GameObject explosion = new GameObject("Explosion");
        explosion.transform.position = transform.position;
        explosion.AddComponent<Explosion>();

        if (GameManager.instance != null)
            GameManager.instance.AnadirPuntos(puntosPorKill);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        ManejarColision(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        ManejarColision(collision.gameObject);
    }

    void ManejarColision(GameObject objetivo)
    {
        if (_destruido) return;
        if (!objetivo.CompareTag("Player")) return;
        _destruido = true;

        GameObject explosion = new GameObject("Explosion");
        explosion.transform.position = transform.position;
        explosion.AddComponent<Explosion>();

        Destroy(objetivo);
        Destroy(gameObject);

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }
}
