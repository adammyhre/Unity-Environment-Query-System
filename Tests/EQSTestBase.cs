using UnityEngine;

namespace EQS.Tests {
    /// <summary>
    /// Base for asset-based EQS tests. Return 0 to reject a point, or (0,1] as score multiplier.
    /// </summary>
    public abstract class EQSTestBase : ScriptableObject, IEQSTest {
        public abstract float Evaluate(in QueryPoint point, in EQSContext context);
    }
}