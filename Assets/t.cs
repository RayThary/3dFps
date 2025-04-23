using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class t : MonoBehaviour
{
    bool solution(string[] phone_book)
    {
        
        string[] tempPhone = phone_book;

        Array.Sort(tempPhone);
        for (int i = 0; i < tempPhone.Length; i++)
        {
            for (int j = 1; j < tempPhone.Length; j++)
            {
                if (tempPhone[j].StartsWith(tempPhone[i]))
                {
                    return false;
                }

            }
        }

        return true;
    }
}
