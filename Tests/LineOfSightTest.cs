using UnityEngine;

namespace EQS.Tests {
    [CreateAssetMenu(menuName = "EQS/Tests/Line Of Sight", fileName = "EQS_LineOfSight")]
    public class LineOfSightTest : EQSTestBase {
        #region Fields

        [SerializeField] [Tooltip("Layers that block line of sight.")]
        LayerMask blockLayers;

        [SerializeField] [Tooltip("If false, points WITH line of sight get 0 (e.g. for cover).")]
        bool requireLos = true;

        [SerializeField] [Tooltip("If true, check from candidate point to target; else from querier to target.")]
        bool fromPointToTarget = false;

        #endregion

        public override float Evaluate(in QueryPoint point, in EQSContext context) {
            Vector3 origin = fromPointToTarget ? point.Position : context.QuerierPosition;
            Vector3 toTarget = context.TargetPosition - origin;
            float dist = toTarget.magnitude;
            if (dist < 0.001f) return 1f;
            bool hasLos = !Physics.Raycast(origin, toTarget.normalized, dist, blockLayers);
            bool pass = requireLos ? hasLos : !hasLos;
            return pass ? 1f : 0f;
        }
    }
}