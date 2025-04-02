using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class R_H_Manager : MonoBehaviour
{
    public static R_H_Manager instance;

    [SerializeField]
    private Text text;

    private static Stack<ICommand> _undoStack = new Stack<ICommand>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        text.text = _undoStack.Count.ToString();
    }

    public void RunBlockCommand(GameObject block, Transform point)
    {
        ICommand command = new Command(block, point);
        _undoStack.Push(command);
    }

    public void UndoCommand()
    {
        if(_undoStack.Count> 0)
        {
            ICommand command = _undoStack.Pop();
            command.Undo();
        }
    }
}
