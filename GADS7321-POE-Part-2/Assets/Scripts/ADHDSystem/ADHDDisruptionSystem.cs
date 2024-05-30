using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ADHDDisruptionSystem : MonoBehaviour
{
    private Animator animator;
    
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
