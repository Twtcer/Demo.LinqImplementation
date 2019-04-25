using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Demo.LinqImplementation
{
    public class DemoQueryProviderImp<T> : IDemoQueryProvider
    {
        public DemoQueryProviderImp(IDemoCollection<T> collection)
        {

        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = expression.Type.GetSequenceElementType();

            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(DemoQueryableImp<>).MakeGenericType(typeof(T), elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new DemoQueryableImp<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            var executionPlan = ExecutionPlanBuilder.BuildPlan(Expression.Constant(this), Translate(expression));

            var lambda = Expression.Lambda(executionPlan);
            try
            {
                return lambda.Compile().DynamicInvoke(null);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
            //return DemoModel.GetDemoModelsByCondition(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = Execute(expression);
            return (TResult)result;
        }

        private QueryableTranslation Translate(Expression expression)
        {
            var pipelineExpression = Prepare(expression);
            return QueryableTranslator.Translate(pipelineExpression, _collection.Settings.SerializerRegistry, _options.TranslationOptions);
        }
    }
}
