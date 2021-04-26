namespace SplineFramework
{
    public enum BezierControlPointMode
    {
        // Can freely adjust the point to where-ever it can be placed at. May create cusps or pointy corners
        // if 2 or more Bezier curves are joined to form a spline
        Free,
        // Ensures new tangent has the same length as the old tangent
        Aligned,
        Mirrored,
    }
}
