using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Demo.LinqImplementation
{
    public class DemoQueryableImp<T> : IDemoQueryable<T>
    {
        private IQueryProvider _provider;
        private readonly Expression _expression;
        public DemoQueryableImp(IDemoQueryProvider queryProvider)
        {
            _provider = queryProvider;
            _expression = Expression.Constant(this, typeof(IDemoQueryable<T>));
        }

        public Type ElementType => typeof(T);

        public Expression Expression => _expression;

        public IQueryProvider Provider => _provider;

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
