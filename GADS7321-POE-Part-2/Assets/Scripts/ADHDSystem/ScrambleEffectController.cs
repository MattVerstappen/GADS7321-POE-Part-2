using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrambleEffectController : MonoBehaviour
{
    [SerializeField] private Image npcPortrait;
    private Material scrambleMaterial;
    private Coroutine scrambleCoroutine;
    [SerializeField] private float currentScrambleAmount = 1f; 

    void Start()
    {
        scrambleMaterial = new Material(Shader.Find("Custom/ScrambleShader"));
        npcPortrait.material = scrambleMaterial;
        UpdateScrambleAmount(); // Apply initial scramble amount
    }

    // Method to start scrambling with a specified amount and duration
    public void StartScrambling(float amount, float duration)
    {
        if (scrambleCoroutine != null)
        {
            StopCoroutine(scrambleCoroutine);
        }
        scrambleCoroutine = StartCoroutine(ScrambleRoutine(amount, duration));
    }

    // Method to stop scrambling
    public void StopScrambling()
    {
        if (scrambleCoroutine != null)
        {
            StopCoroutine(scrambleCoroutine);
        }
        SetScrambleAmount(0f); // Stop scrambling by setting scramble amount to 0
    }

    // Method to set the scramble amount
    public void SetScrambleAmount(float amount)
    {
        currentScrambleAmount = amount;
        UpdateScrambleAmount();
    }

    // Method to update the scramble amount in the material
    private void UpdateScrambleAmount()
    {
        scrambleMaterial.SetFloat("_ScrambleAmount", currentScrambleAmount);
    }

    // Continuous monitoring of scramble amount and update
    void Update()
    {
        UpdateScrambleAmount();
    }

    private IEnumerator ScrambleRoutine(float amount, float duration)
    {
        float elapsedTime = 0f;
        float startAmount = currentScrambleAmount;
        float targetAmount = amount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            currentScrambleAmount = Mathf.Lerp(startAmount, targetAmount, t);
            yield return null;
        }

        // Ensure the target amount is set correctly at the end of the duration
        SetScrambleAmount(targetAmount);
        scrambleCoroutine = null;
    }
}
