using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ihasprogress {

    public event EventHandler<onProgressCHANGedEventArgs> onProgressChanged;
    public class onProgressCHANGedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

}

