using System.Collections.Generic;
using UnityEngine;

namespace EQS.Generators {
    [CreateAssetMenu(menuName = "EQS/Generators/Donut", fileName = "EQS_Donut")]
    public class DonutGenerator : EQSGeneratorBase {
        #region Fields

        [SerializeField] [Tooltip("Inner radius of the donut.")]
        float innerRadius = 2f;

        [SerializeField] [Tooltip("Outer radius (donut thickness = outer - inner).")]
        float outerRadius = 8f;

        [SerializeField] [Range(1, 16)] [Tooltip("Number of concentric rings.")]
        int rings = 3;

        [SerializeField] [Range(4, 64)] [Tooltip("Points per ring.")]
        int pointsPerRing = 12;

        [SerializeField] [Tooltip("Rotate each ring by half step for better coverage.")]
        bool offsetRings = true;

        #endregion

        public override IReadOnlyList<QueryPoint> Generate(EQSContext context) {
            int total = rings * pointsPerRing;
            var points = new List<QueryPoint>(total);
            float ringStep = rings > 1 ? (outerRadius - innerRadius) / (rings - 1) : 0f;
            float angleStep = 2f * Mathf.PI / pointsPerRing;

            for (int r = 0; r < rings; r++) {
                float rad = innerRadius + r * ringStep;
                float offset = offsetRings ? angleStep * 0.5f : 0f;

                for (int p = 0; p < pointsPerRing; p++) {
                    float angle = offset + p * angleStep;
                    float x = rad * Mathf.Cos(angle);
                    float z = rad * Mathf.Sin(angle);
                    Vector3 local = new Vector3(x, 0f, z);
                    Vector3 world = context.QuerierPosition + local;
                    points.Add(new QueryPoint(world));
                }

            }

            return points;
        }
    }
}