using Aparte.Models;
using System.Linq;

namespace Aparte.Repository.Test
{
    class TestProductDbSet : TestDbSet<Tenant>
    {
        public override Tenant Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.PK == (int)keyValues.Single());
        }
    }
}
