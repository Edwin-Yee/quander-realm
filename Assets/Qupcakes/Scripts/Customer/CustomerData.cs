﻿using System;
using UnityEngine;

[CreateAssetMenu]
public class CustomerData : ScriptableObject
{
    public float Speed;
    public int BasePay;
    public int MaxTip;
    public int MaxPatience;
    public int PatienceFreezeTime; // Wait time before patience starts to decrease
}
