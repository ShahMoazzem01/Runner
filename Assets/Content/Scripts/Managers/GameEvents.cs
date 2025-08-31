using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action OnMiniHit;
    public static event Action OnDeadlyHit;

    public static void MiniHit() => OnMiniHit?.Invoke();
    public static void Hit() => OnDeadlyHit?.Invoke();
}
