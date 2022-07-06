﻿using UnityEngine;
using System.Collections;
using Costume = Qupcakery.AssetCostumeUtilities;

/* Button manager (experiment mode) */
namespace Qupcakery
{
    public class ExperimentButtonController : ButtonController
    {
        public new static ButtonController Instance { get; protected set; }

        private new void Awake()
        {
            Instance = this;
            UpdateButtonState(ButtonState.CanNotBePressed);
        }

        // Subscriber to CakeOnBeltTracker
        public override void OnCakesReady()
        {
            UpdateButtonState(ButtonState.CanBePressed);
        }

        // Subscriber to CakeOnBeltTracker
        public override void OnCakesRemovedFromBelt()
        {
            UpdateButtonState(ButtonState.CanNotBePressed);
        }
    }
}
