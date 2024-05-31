using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ADHDDisruptionSystem : MonoBehaviour
{
    private Animator animator;
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
    
    public static string ApplyDisruption(string text)
    {
        
        // Regex to match words, tags, and spaces separately
        var regex = new Regex(@"(<.*?>|[\w']+|[^\w\s<>]+|\s)");
        var matches = regex.Matches(text);

        var scrambledText = string.Empty;
        var parts = new List<string>();
        var positions = new List<bool>();

        // Separate words, tags, and spaces
        foreach (Match match in matches)
        {
            string token = match.Value;
            if (token.StartsWith("<") && token.EndsWith(">"))
            {
                parts.Add(token);
                positions.Add(false); // False indicates this part is a tag or punctuation
            }
            else
            {
                parts.Add(token);
                positions.Add(true); // True indicates this part is a word
            }
        }

        // Scramble the words
        var scrambledParts = ScrambleWords(parts, positions);

        // Reconstruct the text with tags and spaces in place
        for (int i = 0; i < parts.Count; i++)
        {
            scrambledText += scrambledParts[i];
        }

        return scrambledText;
    }
    private static List<string> ScrambleWords(List<string> parts, List<bool> positions)
    {
        var scrambledParts = new List<string>();
        var rand = new System.Random();

        for (int i = 0; i < parts.Count; i++)
        {
            if (positions[i]) // If it's a word, scramble it
            {
                var word = parts[i];
                char[] characters = word.ToCharArray();
                int n = characters.Length;

                for (int j = 0; j < n; j++)
                {
                    if (char.IsLetter(characters[j]))
                    {
                        int k = rand.Next(j, n);
                        // Swap only letters
                        if (char.IsLetter(characters[k]))
                        {
                            var temp = characters[j];
                            characters[j] = characters[k];
                            characters[k] = temp;
                        }
                    }
                }
                scrambledParts.Add(new string(characters));
            }
            else // If it's a tag, punctuation, or space, leave it as is
            {
                scrambledParts.Add(parts[i]);
            }
        }

        return scrambledParts;
    }

    // Utility function to shuffle an array
    private static void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = UnityEngine.Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
