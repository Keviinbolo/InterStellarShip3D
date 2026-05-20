using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad = 30f;

    private Camera camara;
    private Vector3 direccion;
    private bool _destruida = false;

    public void Init(Camera cam, Vector3 dir)
    {
        camara = cam;
        direccion = dir.normalized;
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;

        if (camara == null) return;

        Vector3 viewPos = camara.WorldToViewportPoint(transform.position);
        if (viewPos.z > 60f || viewPos.x < -0.3f || viewPos.x > 1.3f || viewPos.y < -0.3f || viewPos.y > 1.3f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_destruida) return;
        if (other.CompareTag("Player")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            _destruida = true;
            enemy.Morir();
            Destroy(gameObject);
        }
    }
}
