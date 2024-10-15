using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    [Range(0, 2)] public int UnlockMethod = 0;           //Pass Unlock Method from attached Unlock Script to Object Script
    public bool CanSequenceUnlock = false;                   //Enable Object Script to use SequenceUnlock Method, when SequenceUnlock is attached
}
