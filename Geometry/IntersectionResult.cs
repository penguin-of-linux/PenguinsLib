namespace Geometry
{
    /// <summary>
    /// Тип пересечения фигур. Point - Vector2,  Points - Vector2[], None - null, остальные соответствуют классам
    /// </summary>
    public enum IntersectionResult
    {
        Point,
        Points,
        Line,
        Segment,
        Ray,
        None
    }
}