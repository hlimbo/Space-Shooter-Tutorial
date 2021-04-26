
namespace Experiments
{
    public enum BezierControlPointMode
    {
        // can adjust the point to however you want (groups of 3 points)
        Free,
        // Ensures the new tangent has the same length as the old tangent line
        Aligned,
        Mirrored,
    };
}
