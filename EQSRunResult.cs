using System.Collections.Generic;
using UnityEngine;

namespace EQS {
    /// <summary>
    /// Result of running an environment query. Best candidate and optional full set for debugging.
    /// </summary>
    public struct EQSRunResult {
        public bool Success { get; set; }
        public Vector3 BestPosition { get; set; }
        public float BestScore { get; set; }
        public int BestIndex { get; set; }
        public IReadOnlyList<QueryPoint> AllCandidates { get; set; }

        public static EQSRunResult Failure() {
            return new EQSRunResult { Success = false, BestPosition = Vector3.zero, BestScore = 0f, BestIndex = -1 };
        }
    }
}