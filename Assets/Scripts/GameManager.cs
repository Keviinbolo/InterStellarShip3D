using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI textoPuntuacion;

    [Header("Configuracion")]
    public string nombreEscenaFinal = "Final";

    public bool juegoActivo { get; private set; } = true;

    private float puntuacionFloat = 0f;
    private int puntuacion = 0;
    private float tiempoPartida = 0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!juegoActivo) return;

        tiempoPartida += Time.deltaTime;

        // Acumular 100 puntos por segundo con el multiplicador actual
        puntuacionFloat += 100f * ObtenerMultiplicador() * Time.deltaTime;
        puntuacion = Mathf.FloorToInt(puntuacionFloat);

        ActualizarUI();
    }

    float ObtenerMultiplicador()
    {
        if (tiempoPartida < 30f)  return 1f;
        if (tiempoPartida < 60f)  return 2f;
        if (tiempoPartida < 90f)  return 3f;
        return 5f;
    }

    void ActualizarUI()
    {
        if (textoPuntuacion == null) return;

        int mult = Mathf.RoundToInt(ObtenerMultiplicador());
        string multiplicadorTxt = mult > 1 ? $"  <color=#FFD700>x{mult}</color>" : "";
        textoPuntuacion.text = $"Puntos: {puntuacion}{multiplicadorTxt}";
    }

    public void GameOver()
    {
        if (!juegoActivo) return;
        juegoActivo = false;

        PlayerPrefs.SetInt("Puntuacion", puntuacion);
        PlayerPrefs.Save();

        Invoke(nameof(CargarEscenaFinal), 1.5f);
    }

    void CargarEscenaFinal()
    {
        SceneManager.LoadScene(nombreEscenaFinal);
    }
}
