using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "nawLangue", menuName = "SOLangue") ]
public class SOLangue : ScriptableObject
{
   [TextArea]public string[] LangueSave = new string[300];
}
