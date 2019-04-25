using Demo.LinqImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Test.Demo.LinqImplementation
{
    public class LinqQueryTest
    {
        [Fact]
        public void Where()
        {
            IDemoCollection<DemoModel> collection = new DemoCollection<DemoModel>();
            var query = collection.AsQueryable();
            query = query.Where(t => t.Age < 999888);
            var list = query.ToList();
        }
    }
}
