namespace NDatabase.Odb.Core.Query.Soda
{
    /// <summary>
    /// constraint to limit the objects returned upon
    /// query execution.
    /// 
    /// 
    /// Constraints are constructed by calling 
    /// Query.Constrain().
    /// 
    /// 
    /// Constraints can be joined with the methods and() and or().
    /// 
    /// 
    /// The methods to modify the constraint evaluation algorithm may
    /// be merged, to construct combined evaluation rules.
    /// Examples:
    /// <ul>
    ///   <li> <code>Constraint#Smaller().Equal()</code> for "smaller or equal" </li>
    ///   <li> <code>Constraint#Not().Like()</code> for "not like" </li>
    ///   <li> <code>Constraint#Not().Greater().Or().Equal()</code> for "not greater or equal" </li>
    /// </ul>
    ///
    /// </summary>
    public interface IConstraint
    {
        /// <summary>
        /// Links two IConstrains for AND evaluation. 
        /// </summary>
        /// <param name="with">The other IConstraint</param>
        /// <returns>A new IConstraint, that can be used for further calls to and() and or()</returns>
        IConstraint And(IConstraint with);


        /// <summary>
        /// Links two IConstrains for OR evaluation.
        /// </summary>
        /// <param name="with">The other IConstraint</param>
        /// <returns>A new IConstraint, that can be used for further calls to and() and or()</returns>
        IConstraint Or(IConstraint with);


        /// <summary>
        /// Sets the evaluation mode to <code>==</code>.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Equal();


        /// <summary>
        /// Sets the evaluation mode to <code>&gt;</code>.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Greater();


        /// <summary>
        /// Sets the evaluation mode to <code>&lt;</code>.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Smaller();

        /// <summary>
        /// Sets the evaluation mode to identity comparison.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Identity();


        /// <summary>
        /// Sets the evaluation mode to "like" comparison.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Like();


        /// <summary>
        /// Sets the evaluation mode to containment comparison.
        /// 
        /// Evaluation is dependant on the constrained query node:
        /// <dl>
        ///  <dt><code>String</code></dt>
        ///   <dd>the persistent object is tested to contain a substring.</dd>
        ///  <dt>arrays, collections</dt>
        ///   <dd>the persistent object is tested to contain all elements of
        ///      the constraining object.</dd>
        /// </dl>
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Contains();


        /// <summary>
        /// turns on not() comparison.
        /// </summary>
        /// <returns>this IConstraint to allow the chaining of method calls.</returns>
        IConstraint Not();


        /// <summary>
        /// returns the Object the query graph was constrained with to
        /// create this IConstraint.
        /// </summary>
        /// <returns>The constraining object.</returns>
        object GetObject();
    }
}