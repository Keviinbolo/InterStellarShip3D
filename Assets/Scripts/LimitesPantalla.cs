using UnityEngine;

public class LimitesPantalla : MonoBehaviour
{
    private Camera camaraPrincipal;
    private float anchoMitad, altoMitad;

    void Start()
    {
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

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        float cameraCenterX = camaraPrincipal.transform.position.x;
        float cameraCenterY = camaraPrincipal.transform.position.y;

        pos.x = Mathf.Clamp(pos.x, cameraCenterX - anchoMitad, cameraCenterX + anchoMitad);
        pos.y = Mathf.Clamp(pos.y, cameraCenterY - altoMitad, cameraCenterY + altoMitad);
        transform.position = pos;
    }
}