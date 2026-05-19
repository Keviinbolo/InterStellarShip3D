using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 10f;      // Velocidad de desplazamiento

    [Header("Límites de pantalla")]
    public Camera camaraPrincipal;     // Asigna la cámara principal aquí

    private float anchoMitad;
    private float altoMitad;
    private float centroCamaraX;
    private float centroCamaraY;

    void Start()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = Camera.main;

        CalcularLimites();
    }

    void CalcularLimites()
    {
        // Calcula el ancho y alto del mundo visible por la cámara
        float altoCamara = camaraPrincipal.orthographicSize * 2f;
        float anchoCamara = altoCamara * camaraPrincipal.aspect;

        anchoMitad = anchoCamara / 2f;
        altoMitad = altoCamara / 2f;
    }

    void Update()
    {
        // Movimiento con las teclas (Horizontal: A/D, flechas ?/?; Vertical: W/S, flechas ?/?)
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoY = Input.GetAxis("Vertical");

        Vector3 desplazamiento = new Vector3(movimientoX, movimientoY, 0f) * velocidad * Time.deltaTime;
        transform.Translate(desplazamiento, Space.World);
    }

    void LateUpdate()
    {
        // Después de mover la nave, la limitamos a la pantalla
        Vector3 pos = transform.position;
        centroCamaraX = camaraPrincipal.transform.position.x;
        centroCamaraY = camaraPrincipal.transform.position.y;

        pos.x = Mathf.Clamp(pos.x, centroCamaraX - anchoMitad, centroCamaraX + anchoMitad);
        pos.y = Mathf.Clamp(pos.y, centroCamaraY - altoMitad, centroCamaraY + altoMitad);

        transform.position = pos;
    }
}