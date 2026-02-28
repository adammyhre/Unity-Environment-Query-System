using System.Collections.Generic;

namespace EQS {
    /// <summary>
    /// Produces candidate points for an environment query. Implement as ScriptableObject for asset-based queries.
    /// </summary>
    public interface IEQSGenerator {
        /// <summary>
        /// Generate candidate points in world space using the given context (e.g. around querier).
        /// </summary>
        IReadOnlyList<QueryPoint> Generate(EQSContext context);
    }
}