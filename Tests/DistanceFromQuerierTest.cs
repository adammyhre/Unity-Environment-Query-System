using UnityEngine;

namespace EQS.Tests {
    [CreateAssetMenu(menuName = "EQS/Tests/Distance From Querier", fileName = "EQS_DistanceFromQuerier")]
    public class DistanceFromQuerierTest : EQSTestBase {
        #region Fields

        [SerializeField] [Tooltip("Ideal distance from querier; score falls off with difference.")]
        float idealDistance = 5f;

        [SerializeField] [Tooltip("Tolerance; within this of ideal = 1.")]
        float tolerance = 2f;

        #endregion

        public override float Evaluate(in QueryPoint point, in EQSContext context) {
            float dist = Vector3.Distance(context.QuerierPosition, point.Position);
            float diff = Mathf.Abs(dist - idealDistance);
            if (diff <= tolerance) return 1f;
            return Mathf.Clamp01(1f - (diff - tolerance) / (tolerance * 2f));
        }
    }
}