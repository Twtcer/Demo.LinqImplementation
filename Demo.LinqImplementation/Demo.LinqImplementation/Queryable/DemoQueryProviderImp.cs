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
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
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

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
