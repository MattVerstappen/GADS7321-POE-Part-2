using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    public void StartScrambling(float amount, float duration)
    {
        if (scrambleCoroutine != null)
        {
            StopCoroutine(scrambleCoroutine);
        }
        scrambleCoroutine = StartCoroutine(ScrambleRoutine(amount, duration));
    }

    public void StopScrambling()
    {
        if (scrambleCoroutine != null)
        {
            StopCoroutine(scrambleCoroutine);
        }
        SetScrambleAmount(0f); // Stop scrambling by setting scramble amount to 0
    }

    public void SetScrambleAmount(float amount)
    {
        currentScrambleAmount = amount;
        UpdateScrambleAmount();
    }

    private void UpdateScrambleAmount()
    {
        scrambleMaterial.SetFloat("_ScrambleAmount", currentScrambleAmount);
    }

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

        SetScrambleAmount(targetAmount);
        scrambleCoroutine = null;
    }

    public string ApplyDisruption(string text)
    {
        // Regex to find <disruption> tags and capture the text between them
        Regex regex = new Regex(@"<disruption>(.*?)<\/disruption>");
        MatchCollection matches = regex.Matches(text);
        
        // Process each match found
        foreach (Match match in matches)
        {
            // Extract the text between <disruption> tags
            string disruptedText = match.Groups[1].Value;
            // Scramble the extracted text
            string scrambledText = ScrambleText(disruptedText);
            // Replace the original text with scrambled text within the <disruption> tags
            text = text.Replace(match.Value, scrambledText);
        }

        return text;
    }

    private string ScrambleText(string text)
    {
        // Regex to split the text into tokens while preserving tags and non-word characters
        Regex regex = new Regex(@"(<.*?>|[\w']+|[^\w\s<>]+|\s)");
        MatchCollection matches = regex.Matches(text);

        List<string> parts = new List<string>();
        List<bool> positions = new List<bool>();

        foreach (Match match in matches)
        {
            string token = match.Value;
            if (token.StartsWith("<") && token.EndsWith(">"))
            {
                // If the token is a tag, add it to parts and mark as non-scramble position
                parts.Add(token);
                positions.Add(false);
            }
            else
            {
                // If the token is a word or character, add it to parts and mark as scramble position
                parts.Add(token);
                positions.Add(true);
            }
        }

        // Scramble only the words/characters, not the tags
        List<string> scrambledParts = ScrambleWords(parts, positions);

        return string.Join("", scrambledParts);
    }

    private static List<string> ScrambleWords(List<string> parts, List<bool> positions)
    {
        List<string> scrambledParts = new List<string>();
        var rand = new System.Random();

        for (int i = 0; i < parts.Count; i++)
        {
            if (positions[i])
            {
                // Scramble the word/character if marked for scrambling
                var word = parts[i];
                char[] characters = word.ToCharArray();
                int n = characters.Length;

                for (int j = 0; j < n; j++)
                {
                    if (char.IsLetter(characters[j]))
                    {
                        int k = rand.Next(j, n);
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
            else
            {
                // Add the tag as-is if not marked for scrambling
                scrambledParts.Add(parts[i]);
            }
        }

        return scrambledParts;
    }
}
