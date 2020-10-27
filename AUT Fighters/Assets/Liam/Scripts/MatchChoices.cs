﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    SAN,
    CHARLIE,
    LIAM,
    MICHAEL
}

public class MatchChoices
{
    public static Characters p1Character = Characters.CHARLIE;
    public static Characters p2Character = (Characters)Random.Range(0, 3);//Changes to a random character for TrainingMode

    public static string chosenStage = Stages.CROSSING;
}
