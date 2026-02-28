using System.Collections.Generic;

namespace EQS {
    /// <summary>
    /// Runs an EnvironmentQueryAsset: generates candidates, applies tests, returns best result.
    /// </summary>
    public static class EQSExecutor {
        /// <summary>
        /// Execute the query and return the best-scoring point (if any).
        /// </summary>
        public static EQSRunResult Run(EnvironmentQueryAsset query, EQSContext context) {
            if (query == null || query.Generator == null) return EQSRunResult.Failure();
            context.Refresh();

            var candidates = query.Generator.Generate(context);
            if (candidates == null || candidates.Count == 0) return EQSRunResult.Failure();

            var tests = query.Tests;
            int bestIndex = -1;
            float bestScore = 0f;
            var scored = new List<QueryPoint>(candidates.Count);

            for (int i = 0; i < candidates.Count; i++) {
                QueryPoint p = candidates[i];
                if (!p.Valid) continue;
                float score = 1f;

                if (tests != null) {
                    for (int t = 0; t < tests.Count; t++) {
                        score *= tests[t].Evaluate(in p, in context);
                        if (score <= 0f) break;
                    }
                }

                p.Score = score;
                scored.Add(p);

                if (score > bestScore) {
                    bestScore = score;
                    bestIndex = scored.Count - 1;
                }

            }

            QueryPoint best = bestIndex >= 0 ? scored[bestIndex] : default;
            return new EQSRunResult {
                Success = bestIndex >= 0,
                BestPosition = bestIndex >= 0 ? best.Position : default,
                BestScore = bestIndex >= 0 ? best.Score : 0f,
                BestIndex = bestIndex,
                AllCandidates = scored
            };
        }
    }
}