using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents the cardinal directions to be used in game
/// Provides methods for transforming between local and world space
/// and other related utility
/// </summary>

public static class Directions
{
    public enum Direction { NORTH, WEST, SOUTH, EAST }

    private static Dictionary<Direction, Vector3> RotationForDirection = new Dictionary<Direction, Vector3> 
    { 
        {Directions.Direction.NORTH, new Vector3(90f, 0f, 0f) },
        {Directions.Direction.WEST, new Vector3(90f, 0, 90f) },
        {Directions.Direction.SOUTH, new Vector3(90f, 0, 180f) },
        {Directions.Direction.EAST, new Vector3(90f, 0f, 270f) }
    };

    public static bool GetRotationForDirection(Direction dir, out Vector3 rotation)
    {
        return RotationForDirection.TryGetValue(dir, out rotation);
    }

    public static Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.WEST:
                return Direction.EAST;
            default:
                throw new System.InvalidOperationException("Incorrect value for direction");
        }
    }

    // Rotations
    public static Direction GetNextDirectionClockwise(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return Direction.EAST;
            case Direction.EAST:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.NORTH;
            default:
                throw new System.InvalidOperationException("Incorrect value for direction");
        }
    }

    public static Direction GetNextDirectionCounterClockwise(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return Direction.WEST;
            case Direction.EAST:
                return Direction.NORTH;
            case Direction.SOUTH:
                return Direction.EAST;
            case Direction.WEST:
                return Direction.SOUTH;
            default:
                throw new System.InvalidOperationException("Incorrect value for direction");
        }
    }

    // Word to local space of supplied tile
    public static Direction BoardToLocalDirection(Direction boardDirection, CircuitTile forTile)
    {
        int inputRotation = DirectionToOffset(boardDirection);
        int tileRotation = DirectionToOffset(forTile.TileFacingDirection);

        int localRotation = inputRotation + tileRotation;
        while (localRotation >= 360)
        {
            localRotation -= 360;
        }

        return OffsetToDirection(localRotation);
    }

    public static Direction LocalToBoardDirection(Direction localDirection, CircuitTile forTile)
    {
        int inputRotation = DirectionToOffset(localDirection);
        int tileRotation = DirectionToOffset(forTile.TileFacingDirection);

        int localRotation = inputRotation + tileRotation;
        while (localRotation >= 360)
        {
            localRotation -= 360;
        }

        return OffsetToDirection(localRotation);
    }

    public static int DirectionToOffset(Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return 0;
            case Direction.EAST:
                return 90;
            case Direction.SOUTH:
                return 180;
            case Direction.WEST:
                return 270;
            default:
                throw new System.InvalidOperationException("Incorrect value for direction");
        }
    }

    public static Direction OffsetToDirection(int rotation)
    {
        switch (rotation)
        {
            case 0:
                return Direction.NORTH;
            case 90:
                return Direction.EAST;
            case 180:
                return Direction.SOUTH;
            case 270:
                return Direction.WEST;
            default:
                throw new System.InvalidOperationException("Incorrect value for direction");
        }
    }
}
