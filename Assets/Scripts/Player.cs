using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 10f;

    [Header("Limites de pantalla")]
    public Camera camaraPrincipal;

    [Header("Disparo")]
    public float cadenciaDisparo = 8f;   // Disparos por segundo

    private float anchoMitad;
    private float altoMitad;
    private float centroCamaraX;
    private float centroCamaraY;
    private float timerDisparo = 0f;

    void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        CalcularLimites();
    }

    void CalcularLimites()
    {
        float altoCamara = camaraPrincipal.orthographicSize * 2f;
        float anchoCamara = altoCamara * camaraPrincipal.aspect;

        anchoMitad = anchoCamara / 2f;
        altoMitad = altoCamara / 2f;
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.juegoActivo) return;

        // Movimiento
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoY = Input.GetAxis("Vertical");
        Vector3 desplazamiento = new Vector3(movimientoX, movimientoY, 0f) * velocidad * Time.deltaTime;
        transform.Translate(desplazamiento, Space.World);

        // Disparo (Space o boton izquierdo del raton, mantenido)
        timerDisparo -= Time.deltaTime;
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && timerDisparo <= 0f)
        {
            DispararBala();
            timerDisparo = 1f / cadenciaDisparo;
        }
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        centroCamaraX = camaraPrincipal.transform.position.x;
        centroCamaraY = camaraPrincipal.transform.position.y;

        pos.x = Mathf.Clamp(pos.x, centroCamaraX - anchoMitad, centroCamaraX + anchoMitad);
        pos.y = Mathf.Clamp(pos.y, centroCamaraY - altoMitad, centroCamaraY + altoMitad);

        transform.position = pos;
    }

    void DispararBala()
    {
        // Crear el GameObject de la bala
        GameObject bala = new GameObject("Bala");
        bala.transform.position = transform.position + camaraPrincipal.transform.forward * 1.5f;

        // Collider (trigger) para detectar enemigos
        SphereCollider col = bala.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 0.25f;

        // Rigidbody cinematico (necesario para los triggers en Unity)
        Rigidbody rb = bala.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        // Visual: esfera pequeña cyan
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(visual.GetComponent<SphereCollider>());
        visual.transform.SetParent(bala.transform, false);
        visual.transform.localScale = Vector3.one * 0.25f;

        Renderer rend = visual.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0f, 0.9f, 1f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", new Color(0f, 2f, 3f));
            rend.material = mat;
        }

        // Script de movimiento de la bala
        Bullet bulletScript = bala.AddComponent<Bullet>();
        bulletScript.Init(camaraPrincipal, camaraPrincipal.transform.forward);
    }
}