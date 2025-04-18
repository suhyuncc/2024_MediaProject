using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class R_H_Manager : MonoBehaviour
{
    public static R_H_Manager instance;

    [SerializeField]
    private Text text;
    [SerializeField]
    private Block[] blocks;
    [SerializeField]
    private GameObject _finishPanel;
    [SerializeField]
    private GameObject[] _btns;
    private string[] _next_Events; // 다음 대사 list

    private static Stack<ICommand> _undoStack = new Stack<ICommand>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        text.text = _undoStack.Count.ToString();

        if(_undoStack.Count >= 40)
        {
            On_Finish(false);
        }
    }

    public void Reset_Block()
    {
        while (_undoStack.Count > 0)
        {
            ICommand command = _undoStack.Pop();
            command.Undo();
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].Reset_point();
        }
    }

    public void RunBlockCommand(GameObject block, Vector2 point)
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

        if(_undoStack.Count == 0)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Reset_point();
            }
        }
    }

    public void SetEventName(string[] _eventName)
    {
        _next_Events = _eventName;
    }

    public void On_Finish(bool is_win)
    {
        _finishPanel.SetActive(true);

        if(is_win)
        {
            _btns[0].SetActive(true);
        }
        else
        {
            _btns[1].SetActive(true);
        }
    }

    public void Finish_RushHour()
    {
        if(_undoStack.Count < 20)
        {
            Dialogue_Manage.Instance.GetEventName(_next_Events[0].ToString());
        }
        else if(_undoStack.Count >= 20 && _undoStack.Count < 40)
        {
            Dialogue_Manage.Instance.GetEventName(_next_Events[1].ToString());
        }
        else
        {
            Dialogue_Manage.Instance.GetEventName(_next_Events[2].ToString());
        }
    }
}
