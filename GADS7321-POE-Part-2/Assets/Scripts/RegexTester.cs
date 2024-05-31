using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class RegexTester : MonoBehaviour
{
    void Start()
    {
        Debug.Log("RegexTester script started.");

        string text = @"Hello there! #speaker:Dr. Green #portrait:dr_green_neutral #layout:left #audio:animal_crossing_low
                        -> main

                        === main ===
                        How are you feeling today?
                        + [Happy]
                            <disruption>That makes me feel <color=\#F8FF30>happy</color> as well!</disruption> #portrait:dr_green_happy#audio:animal_crossing_low
                        + [Sad]
                            Oh, well <disruption>that makes me</disruption> <color=\#5B81FF>sad</color> too. #portrait:dr_green_sad#audio:animal_crossing_low";

        string pattern = @"(<disruption>.*?<\/disruption>|<.*?>|[\w']+|[^\w\s<>]+|\s)";
        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(text);

        foreach (Match match in matches)
        {
            Debug.Log("Match found: " + match.Value);
        }
    }
}