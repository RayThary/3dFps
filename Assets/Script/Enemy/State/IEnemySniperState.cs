using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySniperState : IEnemyState
{
    public bool SniperShot { get; set; }
}
