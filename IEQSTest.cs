namespace EQS {
    /// <summary>
    /// Evaluates a query point. Return 0 to reject, or a value in (0,1] to score (multiplied with other tests).
    /// </summary>
    public interface IEQSTest {
        /// <summary>
        /// Evaluate the point. 0 = filter out; (0,1] = score multiplier.
        /// </summary>
        float Evaluate(in QueryPoint point, in EQSContext context);
    }
}