using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Samples.BusinessLogic.xUnit.Test
{
    public class SkipIfAttribute : FactAttribute 
        //: TestSkipped
    {
//        public SkipIfAttribute(ITest test, string reason) : base(test, reason)
//        {
//            
//        }

        public SkipIfAttribute()
        {
            
        }
    }
}