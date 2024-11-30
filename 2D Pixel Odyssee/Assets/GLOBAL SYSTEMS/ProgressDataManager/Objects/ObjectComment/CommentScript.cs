using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  

[CreateAssetMenu(fileName = "Comment", menuName = "ScriptableObjects/Comment", order = 1)]
public class Comment : ScriptableObject
{

    [Header("Comment")]
    [TextArea]
    public string[] CommentText;

}