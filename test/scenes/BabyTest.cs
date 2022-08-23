// GdUnit generated TestSuite
using Godot;
using GdUnit3;

namespace GdUnitDefaultTestNamespace
{
    using static Assertions;
    using static Utils;

    [TestSuite]
    public class BabyTest
    {
        private const float tolerance = .01f;

        [TestCase]
        public void signedArea()
        {
            float actual = Baby.signedArea(new Vector2[]{
                new Vector2(1f, 2f),
                new Vector2(3f, 4f),
                new Vector2(7f, 10f),
                new Vector2(1f, 2f),
            });

            float expected = 2f;

            float diff = Mathf.Abs(actual - expected);

            AssertFloat(diff).IsLess(tolerance);
        }
    }
}