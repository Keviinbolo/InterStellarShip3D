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
            textoPuntuacion.text = $"Puntuacion Final\n\n{puntuacion} puntos";
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
