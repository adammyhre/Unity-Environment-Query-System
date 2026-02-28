using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EQS.Generators {
    [CreateAssetMenu(menuName = "EQS/Generators/NavMesh Sample", fileName = "EQS_NavMesh")]
    public class NavMeshSampleGenerator : EQSGeneratorBase {
        #region Fields

        [SerializeField] [Tooltip("Radius around querier to sample.")]
        float radius = 10f;

        [SerializeField] [Range(8, 256)] [Tooltip("Number of candidate points to try; only valid NavMesh hits are kept.")]
        int sampleCount = 48;

        [SerializeField] [Tooltip("Max distance for NavMesh.SamplePosition.")]
        float maxSampleDistance = 5f;

        [SerializeField] [Tooltip("NavMesh area mask; -1 = all areas.")]
        int areaMask = -1;

        #endregion

        public override IReadOnlyList<QueryPoint> Generate(EQSContext context) {
            var points = new List<QueryPoint>(sampleCount);
            int mask = areaMask < 0 ? NavMesh.AllAreas : areaMask;

            for (int i = 0; i < sampleCount; i++) {
                Vector3 randomOffset = Random.insideUnitSphere * radius;
                randomOffset.y = 0f;
                Vector3 samplePos = context.QuerierPosition + randomOffset;

                if (NavMesh.SamplePosition(samplePos, out NavMeshHit hit, maxSampleDistance, mask)) {
                    points.Add(new QueryPoint(hit.position, hit.normal));
                }
            }

            return points;
        }
    }
}