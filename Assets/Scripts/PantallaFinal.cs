using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PantallaFinal : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPuntuacion;

    void Start()
    {
        int puntuacion = PlayerPrefs.GetInt("Puntuacion", 0);

        if (textoPuntuacion != null)
        {
            string mensaje = "PUNTUACION FINAL\n\n";
            mensaje += $"<color=#FFD700><size=60>{puntuacion}</size></color>\n";
            mensaje += "puntos";
            textoPuntuacion.text = mensaje;
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("HomeMenu");
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("Game");
    }
}
