using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] float idleBobbingAmount = 0.05f; // Intensität des Kopf-Bobbings beim Stehen
    [SerializeField] float walkBobbingAmount = 0.1f; // Intensität des Kopf-Bobbings beim Gehen
    [SerializeField] float sprintBobbingAmount = 0.15f; // Intensität des Kopf-Bobbings beim Sprinten
    [SerializeField] float crouchBobbingAmount = 0.05f; // Intensität des Kopf-Bobbings beim Geduckt Gehen
    [SerializeField] float bobbingSpeed = 1f; // Geschwindigkeit des Kopf-Bobbings

    Vector3 originalPos;
    float seed;

    Basic3DMovment playerMovement; // Referenz auf das Basic3DMovment-Skript

    void Start()
    {
        originalPos = transform.localPosition;
        seed = Random.value * 100f; // Zufälliger Seed für die Perlin-Noise-Bewegung

        playerMovement = GetComponentInParent<Basic3DMovment>(); // Basic3DMovment-Skript des Eltern-Objekts holen
    }

    void Update()
    {
        // Bestimme die aktuelle Bobbing-Intensität basierend auf dem Spielerzustand
        float currentBobbingAmount = CalculateBobbingAmount();

        // Simuliere Head Bobbing mit Perlin Noise
        float bobbingOffset = Mathf.PerlinNoise(seed, Time.time * bobbingSpeed) * 2f - 1f;
        bobbingOffset *= currentBobbingAmount;
        Vector3 bobbingMovement = new Vector3(0, bobbingOffset, 0);

        // Aktualisiere die Position basierend auf dem Bobbing
        transform.localPosition = originalPos + bobbingMovement;
    }

    float CalculateBobbingAmount()
    {
        if (playerMovement == null)
        {
            return idleBobbingAmount;
        }

        if (playerMovement.IsSprinting())
        {
            return sprintBobbingAmount;
        }
        else if (playerMovement.IsCrouching())
        {
            return crouchBobbingAmount;
        }
        else if (playerMovement.IsWalking())
        {
            return walkBobbingAmount;
        }
        else
        {
            return idleBobbingAmount;
        }
    }
}
