using System;

namespace Assets.Scripts
{
    public static class EventManager
    {
        public static event Action<Shape> OnShapePlaced;
        public static void RaiseShapePlaced(Shape shape) => OnShapePlaced?.Invoke(shape);
    }
}