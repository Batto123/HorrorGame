using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private bool _enable = true;

    [Header("Screen Shake Settings")]
    [SerializeField, Range(0, 0.1f)] private float _walkAmplitude = 0.002f; // Amplitude für das Gehen
    [SerializeField, Range(0, 30)] private float _walkFrequency = 13.5f; // Frequenz für das Gehen

    [SerializeField, Range(0, 0.1f)] private float _sprintAmplitude = 0.004f; // Amplitude für das Sprinten
    [SerializeField, Range(0, 30)] private float _sprintFrequency = 15.0f; // Frequenz für das Sprinten

    [SerializeField, Range(0, 0.1f)] private float _crouchAmplitude = 0.001f; // Amplitude für das Schleichen
    [SerializeField, Range(0, 30)] private float _crouchFrequency = 10.0f; // Frequenz für das Schleichen

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHodler = null;

    private float _toggleSpeed = 2.5f;
    private Vector3 _startPos;
    private Rigidbody _rigidbody;

    // Referenz auf den Basic3DMovement-Skript
    private Basic3DMovement playerMovement;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<Basic3DMovement>();
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if (!_enable) return;

        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion()
    {
        // Bestimme die Amplitude und Frequenz basierend auf dem Bewegungszustand
        float currentAmplitude = _walkAmplitude;
        float currentFrequency = _walkFrequency;

        if (playerMovement.IsSprinting())
        {
            currentAmplitude = _sprintAmplitude; // Amplitude beim Sprinten
            currentFrequency = _sprintFrequency; // Frequenz beim Sprinten
        }
        else if (playerMovement.IsCrouching())
        {
            currentAmplitude = _crouchAmplitude; // Amplitude beim Schleichen
            currentFrequency = _crouchFrequency; // Frequenz beim Schleichen
        }

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * currentFrequency) * currentAmplitude;
        pos.x += Mathf.Cos(Time.time * currentFrequency / 2) * currentAmplitude * 2; 
        return pos;
    }

    private void CheckMotion()
    {
        if (_rigidbody == null) return;

        // Überprüfen, ob der Spieler am Boden ist
        if (!playerMovement.isGrounded) return;

        float speed = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;

        if (speed < _toggleSpeed) return;

        Debug.Log("Player is moving: " + speed); // Zeige die aktuelle Geschwindigkeit im Debug-Log an
        PlayMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHodler.localPosition.y, transform.position.z);
        pos += _cameraHodler.forward * 15.0f;
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion; 
    }
}
