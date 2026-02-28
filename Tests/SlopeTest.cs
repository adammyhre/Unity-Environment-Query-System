using UnityEngine;

namespace EQS.Tests {
    [CreateAssetMenu(menuName = "EQS/Tests/Slope", fileName = "EQS_Slope")]
    public class SlopeTest : EQSTestBase {
        #region Fields

        [SerializeField] [Range(0f, 90f)] [Tooltip("Max allowed slope (degrees from up); steeper = 0 score.")]
        float maxSlopeDegrees = 45f;

        #endregion

        public override float Evaluate(in QueryPoint point, in EQSContext context) {
            float angle = Vector3.Angle(Vector3.up, point.Normal);
            if (angle > maxSlopeDegrees) return 0f;
            return 1f - angle / maxSlopeDegrees * 0.5f;
        }
    }
}