﻿using OpenSTSM.Models.MainWindow.SimulinkElements;
using System;
using System.Windows;

namespace OpenSTSM.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SimulinkElementPropertiesAttribute : Attribute
    {
        public string Name { get; private set; }
        public int NumberOfInputs { get; private set; }
        public int NumberOfOutputs { get; private set; }
        public bool IsFlippedHorizontally { get; private set; }

        public SimulinkElementPropertiesAttribute(string name, int numberOfInputs, int numberOfOutputs, bool isFlippedHorizontally)
        {
            Name = name;
            NumberOfInputs = numberOfInputs;
            NumberOfOutputs = numberOfOutputs;
            IsFlippedHorizontally = isFlippedHorizontally;
        }
    }
}
