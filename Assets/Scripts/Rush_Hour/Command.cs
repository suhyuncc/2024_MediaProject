using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Command : ICommand
{
    private GameObject _block;
    private Transform _point;

    public Command(GameObject block, Transform point)
    {
        this._block = block;
        this._point = point;
    }

    public void Undo()
    {
        _block.GetComponent<Block>().Move_to_point(_point);
    }
}

public interface ICommand
{
    public void Undo();
}