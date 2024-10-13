using System.Collections;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;  // Array für verschiedene Fußschrittgeräusche
    public AudioClip jumpClip;  // Sound beim Springen
    public AudioClip landClip;  // Sound beim Landen
    public Basic3DMovement playerMovement;

    public float footstepDelay = 0.5f;  // Zeit zwischen den Fußschritten beim Laufen
    private float nextFootstepTime;

    private bool wasGrounded;  // Um den Zustand des Spielers zu verfolgen

    void Start()
    {
        // Verbindung zum Player-Movement-Script herstellen
        playerMovement = GetComponent<Basic3DMovement>();
        nextFootstepTime = 0f;
        wasGrounded = playerMovement.isGrounded; // Initialen Zustand festlegen
    }

    void Update()
    {
        // Nur Fußschritte abspielen, wenn der Spieler sich bewegt und am Boden ist
        if (playerMovement.IsMoving() && playerMovement.isGrounded && Time.time >= nextFootstepTime)
        {
            PlayFootstepSound();
        }

        // Überprüfen, ob der Spieler gesprungen ist
        if (!wasGrounded && playerMovement.isGrounded)
        {
            PlayLandSound();  // Spielen des Landegeräuschs
        }

        // Überprüfen, ob der Spieler springt
        if (Input.GetKeyDown(playerMovement.jumpKey) && playerMovement.isGrounded)
        {
            PlayJumpSound();  // Spielen des Sprunggeräuschs
        }

        // Den vorherigen Zustand aktualisieren
        wasGrounded = playerMovement.isGrounded;
    }

    void PlayFootstepSound()
    {
        // Wähle zufällig ein Fußschrittgeräusch aus
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        // Passen den Pitch basierend auf der Bewegungsgeschwindigkeit an
        if (playerMovement.IsSprinting())
        {
            audioSource.pitch = 1.3f;  // Höherer Pitch beim Sprinten
            nextFootstepTime = Time.time + footstepDelay / 1.5f;  // Schnellere Schritte beim Sprinten
        }
        else if (playerMovement.IsCrouching())
        {
            audioSource.pitch = 0.8f;  // Tieferer Pitch beim Schleichen
            nextFootstepTime = Time.time + footstepDelay * 1.5f;  // Langsamere Schritte beim Crouchen
        }
        else
        {
            audioSource.pitch = 1.0f;  // Normaler Pitch beim normalen Laufen
            nextFootstepTime = Time.time + footstepDelay;  // Normale Zeit zwischen Schritten
        }

        // Spiele das ausgewählte Fußschrittgeräusch ab
        audioSource.PlayOneShot(clip);
    }

    void PlayJumpSound()
    {
        // Spiele den Sprung-Sound ab
        audioSource.PlayOneShot(jumpClip);
    }

    void PlayLandSound()
    {
        // Spiele den Landegeräusch ab
        audioSource.PlayOneShot(landClip);
    }
}
