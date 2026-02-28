using UnityEngine;

namespace EQS {
    /// <summary>
    /// A single candidate item produced by a generator and scored by tests.
    /// </summary>
    public struct QueryPoint {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public float Score { get; set; }
        public bool Valid { get; set; }

        public QueryPoint(Vector3 position, Vector3 normal = default) {
            Position = position;
            Normal = normal == default ? Vector3.up : normal;
            Score = 1f;
            Valid = true;
        }
    }
}