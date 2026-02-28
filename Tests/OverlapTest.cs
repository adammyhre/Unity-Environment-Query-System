using UnityEngine;

namespace EQS.Tests {
    [CreateAssetMenu(menuName = "EQS/Tests/Overlap", fileName = "EQS_Overlap")]
    public class OverlapTest : EQSTestBase {
        #region Fields

        [SerializeField] [Tooltip("Radius to check for overlaps.")]
        float radius = 0.5f;

        [SerializeField] [Tooltip("Layers to check; if any overlap, score is 0.")]
        LayerMask rejectLayers;

        #endregion

        public override float Evaluate(in QueryPoint point, in EQSContext context) {
            if (Physics.OverlapSphere(point.Position, radius, rejectLayers).Length > 0) {
                return 0f;
            }

            return 1f;
        }
    }
}