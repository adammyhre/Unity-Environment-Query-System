using UnityEngine;

namespace EQS.Tests {
    [CreateAssetMenu(menuName = "EQS/Tests/Distance To Target", fileName = "EQS_DistanceToTarget")]
    public class DistanceToTargetTest : EQSTestBase {
        #region Fields

        [SerializeField]
        [Tooltip("Score decays with distance; this controls how fast (higher = slower decay).")]
        [Range(0.5f, 50f)]
        float decayParam = 5f;

        [SerializeField] [Tooltip("If true, prefer points farther from target (e.g. for retreat).")]
        bool preferFar = false;

        #endregion

        public override float Evaluate(in QueryPoint point, in EQSContext context) {
            float distance = Vector3.Distance(context.TargetPosition, point.Position);
            float score = Mathf.Exp(-distance / decayParam);
            return preferFar ? 1f - score : score;
        }
    }
}