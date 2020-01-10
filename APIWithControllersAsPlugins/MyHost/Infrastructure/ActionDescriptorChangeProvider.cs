using System.Threading;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;

namespace MyHost.Infrastructure
{
    public class ActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        public IChangeToken GetChangeToken()
        {
            return new CancellationChangeToken(new CancellationTokenSource().Token);
        }
    }
}
