using System;
using System.Linq.Expressions;
using System.Threading;

namespace Demo.LinqImplementation
{
    internal static class ExecutionPlanBuilder
    {
        public static Expression BuildPlan(Expression provider, QueryableTranslation translation)
        {
            Expression executor = Expression.Call(
                provider,
                "ExecuteModel",
                null,
                Expression.Constant(translation.Model, typeof(QueryableExecutionModel)));

            executor = Expression.Convert(
                executor,
                typeof(IAsyncCursor<>).MakeGenericType(translation.Model.OutputType));

            // we have an IAsyncCursor at this point... need to change it into an IEnumerable
            executor = Expression.Call(
                typeof(IAsyncCursorExtensions),
                nameof(IAsyncCursorExtensions.ToEnumerable),
                new Type[] { translation.Model.OutputType },
                executor,
                Expression.Constant(CancellationToken.None));

            if (translation.ResultTransformer != null)
            {
                var lambda = translation.ResultTransformer.CreateAggregator(translation.Model.OutputType);
                executor = Expression.Invoke(
                    lambda,
                    executor);
            }

            return executor;
        }

        public static Expression BuildAsyncPlan(Expression provider, QueryableTranslation translation, Expression cancellationToken)
        {
            Expression executor = Expression.Call(
                    provider,
                    "ExecuteModelAsync",
                    null,
                    Expression.Constant(translation.Model, typeof(QueryableExecutionModel)),
                    cancellationToken);

            if (translation.ResultTransformer != null)
            {
                var lambda = translation.ResultTransformer.CreateAsyncAggregator(translation.Model.OutputType);
                executor = Expression.Invoke(
                    lambda,
                    Expression.Convert(executor, lambda.Parameters[0].Type),
                    cancellationToken);
            }

            return executor;
        }
    }
}
