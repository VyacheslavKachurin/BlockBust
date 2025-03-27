using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlueprintRow : MonoBehaviour
{
    [SerializeField] private Shape _shapeDummy;
    [SerializeField] private float _YStep;
    [SerializeField] private Grid _grid;
    [SerializeField] private List<Blueprint> _blueprints = new List<Blueprint>();

    void Start()
    {
        foreach (var blueprint in _blueprints)
        {
            blueprint.OnClicked += HandleClick;
        }
    }

    private void HandleClick(Blueprint blueprint)
    {
        blueprint.Show(false);
        var position = blueprint.transform.position;
        var shape = Instantiate(_shapeDummy, position, quaternion.identity);
        _grid.SetShape(shape);

    }

    internal void RestoreBlueprints()
    {
        foreach (var blueprint in _blueprints)
        {
            blueprint.Show(true);
        }
    }
}