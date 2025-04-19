using System;
using Assets.Scripts;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelView : MonoBehaviour
{
    private VisualElement _root;
    private Label _scoreLbl;
    internal int Score
    {
        set => _scoreLbl.text = value.ToString();
    }

    public void Init()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _scoreLbl = _root.Q<Label>("score-lbl");
      
        /*
                var binding = new DataBinding
                {
                    dataSource = ,
                    dataSourcePath = PropertyPath.FromName(nameof(Car.Coins)),
                    bindingMode = BindingMode.ToTarget
                };

                // _coinsLbl.dataSource = _car;
                //_coinsLbl.dataSourcePath = PropertyPath.FromName(nameof(Car.Coins));



                _coinsLbl.SetBinding(nameof(Label.text), binding);
                */

    }

    public void BindScore(GameInfo scoreSource)
    {
        _scoreLbl.Bind(scoreSource, PropertyPath.FromName(nameof(GameInfo.CurrentScore)));
    }

    internal void UpdateScore(int currentScore)
    {
        throw new NotImplementedException();
    }
}