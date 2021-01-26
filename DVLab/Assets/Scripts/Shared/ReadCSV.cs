using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ReadCSV : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; // Define delimiters, regular expression craziness
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r"; // Define line delimiters, regular experession craziness
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file) //Declare method
    {
        Debug.Log("Reading  " + file); // Print filename, make sure parsed correctly

        var list = new List<Dictionary<string, object>>(); //declare dictionary list

        TextAsset data = Resources.Load(file) as TextAsset; //Loads the TextAsset named in the file argument of the function

        var lines = Regex.Split(data.text, LINE_SPLIT_RE); // Split data.text into lines using LINE_SPLIT_RE characters



        if (lines.Length <= 1) return list; //Check that there is more than one line

        var header = Regex.Split(lines[0], SPLIT_RE); //Split header (element 0)

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE); //Split lines according to SPLIT_RE, store in var (usually string array)
            if (values.Length == 0 || values[0] == "") continue; // Skip to end of loop (continue) if value is 0 length OR first value is empty

            var entry = new Dictionary<string, object>(); // Creates dictionary object

            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j]; // Set local variable value
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); // Trim characters

                object finalvalue;
                finalvalue = GetFinalValue(value);
                if(finalvalue is string)
                    finalvalue = GetFinalValue(value.Replace(".", ","));

                entry[header[j]] = finalvalue;
            }
            list.Add(entry); // Add Dictionary ("entry" variable) to list
        }

        return list; //Return list
    }

    private static object GetFinalValue(string value)
    {
        object finalValue = value;
        int n; 
        float f;

        if (int.TryParse(value, out n))
        {
            finalValue = n;
        }
        else if (float.TryParse(value, out f))
        {
            finalValue = f;
        }

        return finalValue;
    }
}
