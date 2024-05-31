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
    private bool isTextDisrupted = false;
    public string unscrambledText;

    void Start()
    {
        scrambleMaterial = new Material(Shader.Find("Custom/ScrambleShader"));
        npcPortrait.material = scrambleMaterial;
        UpdateScrambleAmount();
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
        SetScrambleAmount(0f);
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
        unscrambledText = text;
        isTextDisrupted = false;

        Regex regex = new Regex(@"<disruption>(.*?)<\/disruption>");
        MatchCollection matches = regex.Matches(text);

        foreach (Match match in matches)
        {
            isTextDisrupted = true;

            string disruptedText = match.Groups[1].Value;
            string scrambledText = ScrambleText(disruptedText);
            string aloneText = RemoveDisruption(disruptedText);
        
            // Replacing with <disruption> tags included
            unscrambledText = unscrambledText.Replace(match.Value, $"<disruption>{aloneText}</disruption>");
            text = text.Replace(match.Value, $"<disruption>{scrambledText}</disruption>");
        }

        return text;
    }

    private string ScrambleText(string text)
    {
        Regex regex = new Regex(@"(<.*?>|[\w']+|[^\w\s<>]+|\s)");
        MatchCollection matches = regex.Matches(text);

        List<string> parts = new List<string>();
        List<bool> positions = new List<bool>();

        foreach (Match match in matches)
        {
            string token = match.Value;
            if (token.StartsWith("<") && token.EndsWith(">"))
            {
                parts.Add(token);
                positions.Add(false);
            }
            else
            {
                parts.Add(token);
                positions.Add(true);
            }
        }

        List<string> scrambledParts = ScrambleWords(parts, positions);

        return string.Join("", scrambledParts);
    }

    private string RemoveDisruption(string text)
    {
        Regex regex = new Regex(@"(<.*?>|[\w']+|[^\w\s<>]+|\s)");
        MatchCollection matches = regex.Matches(text);

        List<string> parts = new List<string>();
        List<bool> positions = new List<bool>();

        foreach (Match match in matches)
        {
            string token = match.Value;
            if (token.StartsWith("<") && token.EndsWith(">"))
            {
                parts.Add(token);
                positions.Add(false);
            }
            else
            {
                parts.Add(token);
                positions.Add(true);
            }
        }

        List<string> scrambledParts = LeftAlone(parts, positions);

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
                scrambledParts.Add(parts[i]);
            }
        }

        return scrambledParts;
    }
    
    private static List<string> LeftAlone(List<string> parts, List<bool> positions)
    {
        List<string> scrambledParts = new List<string>();

        for (int i = 0; i < parts.Count; i++)
        {
            scrambledParts.Add(parts[i]);
        }
        
        return scrambledParts;
    }

    public bool IsTextDisrupted()
    {
        return isTextDisrupted;
    }
}
