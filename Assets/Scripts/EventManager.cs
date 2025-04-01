using System;

namespace Assets.Scripts
{
    public static class EventManager
    {
        public static event Action<Shape> OnShapePlaced;
        public static event Action<int, int> OnLineCleared;
        
        public static void RaiseShapePlaced(Shape shape) => OnShapePlaced?.Invoke(shape);
        public static void RaiseLineCleared(int tiles, int lines) => OnLineCleared?.Invoke(tiles, lines);
    }
}