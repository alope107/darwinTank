// GdUnit generated TestSuite
using Godot;
using GdUnit3;

namespace GdUnitDefaultTestNamespace
{
    using static Assertions;
    using static Utils;

    [TestSuite]
    public class ConvexPolygonTest
    {
        private const float tolerance = .01f;

        [TestCase]
        public void signedArea()
        {
            ConvexPolygon polygon = new ConvexPolygon(new Vector2[]{
                new Vector2(1f, 2f),
                new Vector2(3f, 4f),
                new Vector2(7f, 10f),
                new Vector2(1f, 2f),
            });

            float actual = polygon.Area;
            float expected = 2f;
            float diff = Mathf.Abs(actual - expected);

            AssertFloat(diff).IsLess(tolerance);
        }

        [TestCase]
        public void toString()
        {
            ConvexPolygon polygon = new ConvexPolygon(new Vector2[]{
                new Vector2(1f, 2f),
                new Vector2(3f, 4f),
                new Vector2(7f, 10f),
                new Vector2(1f, 2f),
            });

            string actual = polygon.ToString();
            string expected = "((-2.7, -3.3), (-0.7, -1.3), (3.3, 4.7), (-2.7, -3.3))";

            AssertString(actual).IsEqual(expected);
        }
    }


}