
using UnityEngine;
using System.Collections;

public class RockJumpscare : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public GameObject jumpscareImage;
    public float duration = 3f;
    public string playerTag = "Player";

    [Header("Options")]
    public bool triggerOnlyOnce = true;

    private bool isPlaying = false;
    private bool hasTriggered = false;

    private void Start()
    {
        if (jumpscareImage != null)
        {
            jumpscareImage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (isPlaying)
            return;

        if (triggerOnlyOnce && hasTriggered)
            return;

        hasTriggered = true;
        StartCoroutine(ShowJumpscare());
    }

    private IEnumerator ShowJumpscare()
    {
        isPlaying = true;

        if (jumpscareImage != null)
        {
            jumpscareImage.SetActive(true);
            yield return new WaitForSeconds(duration);
            jumpscareImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Assign a Jumpscare Image in the Inspector!", this);
        }

        isPlaying = false;
    }
}
