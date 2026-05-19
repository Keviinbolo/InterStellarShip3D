using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject prefabEnemigo;
    public Transform player;
    public Camera camaraPrincipal;

    [Header("Dificultad")]
    [Tooltip("Intervalo de spawn al inicio (segundos)")]
    public float intervaloInicial = 2f;
    [Tooltip("Intervalo minimo de spawn (segundos)")]
    public float intervaloMinimo = 0.4f;
    [Tooltip("Distancia desde la camara a la que aparecen los enemigos")]
    public float distanciaSpawn = 30f;

    private float timerSpawn;
    private float intervaloActual;

    void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        intervaloActual = intervaloInicial;
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.juegoActivo) return;

        intervaloActual = CalcularIntervalo();

        timerSpawn += Time.deltaTime;
        if (timerSpawn >= intervaloActual)
        {
            timerSpawn = 0f;
            SpawnEnemigo();
        }
    }

    float CalcularIntervalo()
    {
        // El intervalo disminuye progresivamente con el tiempo de partida
        float t = Time.timeSinceLevelLoad;

        if (t < 30f)
            return intervaloInicial;
        if (t < 60f)
            return Mathf.Lerp(intervaloInicial, intervaloInicial * 0.6f, (t - 30f) / 30f);
        if (t < 90f)
            return Mathf.Lerp(intervaloInicial * 0.6f, intervaloMinimo * 1.5f, (t - 60f) / 30f);

        return intervaloMinimo;
    }

    void SpawnEnemigo()
    {
        if (player == null) return;

        // Posicion aleatoria dentro del viewport
        float viewX = Random.Range(0.05f, 0.95f);
        float viewY = Random.Range(0.05f, 0.95f);

        Vector3 posSpawn = camaraPrincipal.ViewportToWorldPoint(
            new Vector3(viewX, viewY, distanciaSpawn)
        );

        GameObject enemigo = Instantiate(prefabEnemigo, posSpawn, Quaternion.identity);

        Vector3 direccion = (player.position - posSpawn).normalized;
        if (direccion != Vector3.zero)
            enemigo.transform.rotation = Quaternion.LookRotation(direccion);

        Enemy scriptEnemigo = enemigo.GetComponent<Enemy>();
        if (scriptEnemigo != null)
        {
            scriptEnemigo.player = player;
            scriptEnemigo.camara = camaraPrincipal;
        }
    }
}
