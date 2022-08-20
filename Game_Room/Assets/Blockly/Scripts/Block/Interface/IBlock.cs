using System;
using System.Linq;
using UnityEngine;

public interface IBlock
{
    public bool isDragged { get; set; } //To check if the block is being dragged
    public bool isInMain { get; set; }  // To check in which canvas section the block is, Main or SideBar

    //block core
    public void Execute();

    public void ErrorMessage(ErrorCode errorCode);

    public static bool IsFloat(string stringValue)
    {
        try
        {
            float.Parse(stringValue);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool IsBool(string stringValue)
    {
        return stringValue is "True" or "true" or "False" or "false";
    }

    public static bool IsTrue(string stringValue)
    {
        return stringValue is "True" or "true";
    }

    public static ErrorCode IsValidName(string variableName)
    {
        // Check if string is empty
        if (string.IsNullOrEmpty(variableName))
            return ErrorCode.EmptyString;
        

        // Check if first char is a number
        if (variableName.First() >= '0' && variableName.First() <= '9')
            return ErrorCode.FirstElementIsDigit;
        

        // Check if contains spaces
        if (variableName.Contains(" "))
            return ErrorCode.NameHasSpaces;
        
        // Check if contains special character
        if (variableName.Any(c => c is not (>= 'A' and <= 'Z' or >= 'a' and <= 'z' or '_')))
            return ErrorCode.NoValidName;

        if (IBlock.IsBool(variableName))
            return ErrorCode.NoValidName;

        return ErrorCode.NoError;
    }

    //fisrt call from resizable block, returns the sum of the height of all contained blocks
    public float RecoursiveGetSize(GameObject toResizeBlock, GameObject endStatement);
}
