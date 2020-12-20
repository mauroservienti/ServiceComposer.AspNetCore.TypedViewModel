using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using Xunit;

namespace ServiceComposer.AspNetCore.TypedViewModel.Tests.API
{
    public class APIApprovals
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [MethodImpl(MethodImplOptions.NoInlining)]
        #if NETCOREAPP3_1
        [UseApprovalSubdirectory("NETCOREAPP3_1")]
        #endif
        #if NET5_0
        [UseApprovalSubdirectory("NET5_0")]
        #endif
        public void Approve_API()
        {
            var publicApi = typeof(CastleDynamicProxyViewModelFactory).Assembly.GeneratePublicApi();

            Approvals.Verify(publicApi);
        }
    }
}
