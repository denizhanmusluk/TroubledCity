using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHelper
{
    int helpNo { get; set; }
    void helper(Line currentLine, GameObject troubleArea);
}
