using System.Collections.Generic;
using UnityEngine;

namespace EQS.Generators {
    [CreateAssetMenu(menuName = "EQS/Generators/Grid", fileName = "EQS_Grid")]
    public class GridGenerator : EQSGeneratorBase {
        #region Fields

        [SerializeField] [Tooltip("Half-extent of the grid on X and Z (centered on querier).")]
        float size = 10f;

        [SerializeField] [Tooltip("Number of divisions per axis (total points = divisions * divisions).")] [Range(2, 64)]
        int divisions = 10;

        [SerializeField] [Range(0f, 1f)] [Tooltip("Random offset per point to break regularity.")]
        float noise = 0f;

        #endregion

        public override IReadOnlyList<QueryPoint> Generate(EQSContext context) {
            int count = divisions * divisions;
            var points = new List<QueryPoint>(count);
            float step = 2f * size / divisions;
            float half = size;

            for (int i = 0; i < count; i++) {
                int x = i % divisions;
                int z = i / divisions;
                float px = -half + x * step + step * 0.5f;
                float pz = -half + z * step + step * 0.5f;

                if (noise > 0f) {
                    px += (Random.value - 0.5f) * step * noise;
                    pz += (Random.value - 0.5f) * step * noise;
                }

                Vector3 local = new Vector3(px, 0f, pz);
                points.Add(new QueryPoint(context.QuerierPosition + local));
            }

            return points;
        }
    }
}