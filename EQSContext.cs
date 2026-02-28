using UnityEngine;

namespace EQS {
    /// <summary>
    /// Runtime context for an EQS query. Provides named references (querier, target) used by generators and tests.
    /// </summary>
    public struct EQSContext {
        public Vector3 QuerierPosition { get; set; }
        public Transform Querier { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Transform Target { get; set; }

        public static EQSContext From(Transform querier, Transform target = null) {
            EQSContext ctx = new EQSContext {
                Querier = querier,
                QuerierPosition = querier != null ? querier.position : Vector3.zero,
                Target = target,
                TargetPosition = target != null ? target.position : Vector3.zero
            };

            return ctx;
        }

        public void Refresh() {
            if (Querier != null) QuerierPosition = Querier.position;
            if (Target != null) TargetPosition = Target.position;
        }
    }
}