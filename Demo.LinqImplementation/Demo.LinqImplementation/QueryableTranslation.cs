using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.LinqImplementation
{
    internal sealed class QueryableTranslation
    {
        private readonly QueryableExecutionModel _model;
        private readonly IResultTransformer _resultTransformer;

        public QueryableTranslation(QueryableExecutionModel model, IResultTransformer resultTransformer)
        {
            _model = Ensure.IsNotNull(model, nameof(model));
            _resultTransformer = resultTransformer;
        }

        public QueryableExecutionModel Model
        {
            get { return _model; }
        }

        public IResultTransformer ResultTransformer
        {
            get { return _resultTransformer; }
        }
    }
}
