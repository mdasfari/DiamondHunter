using System;
using UnityEngine;

public static class GlobalFunctions
{
    public static bool IsVectorNearBy(Vector3 original, Vector3 moving)
    {
        return (original.y - moving.y < 0.1f && original.x - moving.x < 0.1f && original.z - moving.z < 0.1f);
    }

    internal static Vector3 GetVectorDirection(MoveDirections direction)
    {
        Vector3 result;

        switch (direction)
        {
            case MoveDirections.Up:
                result = Vector3.up;
                break;
            case MoveDirections.Down:
                result = Vector3.down;
                break;
            case MoveDirections.Forward:
                result = Vector3.forward;
                break;
            case MoveDirections.Backward:
                result = Vector3.back;
                break;
            case MoveDirections.Left:
                result = Vector3.left;
                break;
            case MoveDirections.Right:
                result = Vector3.right;
                break;
            default:
            case MoveDirections.Idle:
                result = Vector3.zero;
                break;
        }
        return result;
    }

}
