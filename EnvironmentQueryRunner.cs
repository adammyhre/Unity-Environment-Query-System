using UnityEngine;

namespace EQS {
    /// <summary>
    /// MonoBehaviour that runs an EnvironmentQueryAsset each frame or on demand and exposes the best result.
    /// Assign a query asset, set querier (this transform) and optional target; read BestPosition / LastResult.
    /// </summary>
    public class EnvironmentQueryRunner : MonoBehaviour {
        #region Fields

        [SerializeField] EnvironmentQueryAsset query;

        [SerializeField] [Tooltip("Optional. If set, context target is updated from this.")]
        Transform target;

        public bool RunEveryFrame { get; set; } = true;

        [SerializeField] [Tooltip("Draw best (green) and candidates (blue/red) when selected and playing.")]
        bool drawGizmos = true;

        EQSContext context;
        EQSRunResult lastResult;

        #endregion

        public EnvironmentQueryAsset Query => query;
        public EQSRunResult LastResult => lastResult;
        public bool HasValidResult => lastResult.Success;
        public Vector3 BestPosition => lastResult.Success ? lastResult.BestPosition : transform.position;

        protected void Start() {
            if (query != null) {
                RunEveryFrame = query.Strategy == AgentStrategy.Continuous && !query.RunOnce;
                if (!RunEveryFrame) RunQuery();
            }

        }

        protected void Update() {
            if (RunEveryFrame && query != null) RunQuery();
        }

        /// <summary>
        /// Run the assigned query once and store the result in LastResult.
        /// </summary>
        public void RunQuery() {
            if (query == null) return;
            context = EQSContext.From(transform, target);
            lastResult = query.Run(context);
        }

        /// <summary>
        /// Run a specific query with optional target override.
        /// </summary>
        public EQSRunResult RunQuery(EnvironmentQueryAsset asset, Transform targetOverride = null) {
            EQSContext ctx = EQSContext.From(transform, targetOverride != null ? targetOverride : target);
            return asset != null ? asset.Run(ctx) : EQSRunResult.Failure();
        }

#if UNITY_EDITOR
        const float NormalGizmoLength = 3f;

        void OnDrawGizmosSelected() {
            if (!drawGizmos || !Application.isPlaying || query == null) return;
            if (lastResult.AllCandidates == null) return;

            for (int i = 0; i < lastResult.AllCandidates.Count; i++) {
                QueryPoint p = lastResult.AllCandidates[i];

                if (lastResult.Success && i == lastResult.BestIndex) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(p.Position, 0.5f);
                } else {
                    Gizmos.color = p.Score < 0.05f ? Color.red : Color.blue;
                    Gizmos.DrawWireSphere(p.Position, 0.2f);
                }

                Gizmos.DrawLine(p.Position, p.Position + p.Normal * NormalGizmoLength);
            }
        }
#endif
    }
}