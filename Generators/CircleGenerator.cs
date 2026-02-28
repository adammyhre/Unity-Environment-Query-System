using System.Collections.Generic;
using UnityEngine;

namespace EQS.Generators {
    [CreateAssetMenu(menuName = "EQS/Generators/Circle (Fibonacci)", fileName = "EQS_Circle")]
    public class CircleGenerator : EQSGeneratorBase {
        #region Fields

        [SerializeField] [Tooltip("Radius of the circle around the querier.")]
        float radius = 10f;

        [SerializeField] [Range(4, 256)] [Tooltip("Number of points; Fibonacci disc gives even distribution.")]
        int pointCount = 32;

        #endregion

        public override IReadOnlyList<QueryPoint> Generate(EQSContext context) {
            var points = new List<QueryPoint>(pointCount);
            float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));

            for (int i = 0; i < pointCount; i++) {
                float r = radius * Mathf.Sqrt((i + 0.5f) / pointCount);
                float theta = i * goldenAngle;
                float x = r * Mathf.Cos(theta);
                float z = r * Mathf.Sin(theta);
                Vector3 local = new Vector3(x, 0f, z);
                Vector3 world = context.QuerierPosition + local;
                points.Add(new QueryPoint(world));
            }

            return points;
        }
    }
}