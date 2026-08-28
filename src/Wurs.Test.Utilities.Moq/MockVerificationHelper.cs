using System.Reflection;
using Moq;
using Wurs.Test.Utilities.Helpers.Reflection;

namespace Wurs.Test.Utilities.Moq;

public static class MockVerificationHelper
{
    public static void Verify(this object testInstance)
    {
        ArgumentNullException.ThrowIfNull(testInstance);

        VerificationObjectGraphWalker.Walk(testInstance, ShouldRecurse, VerifyMoqMockIfApplicable);
    }

    private static void VerifyMoqMockIfApplicable(object value)
    {
        if (value is not Mock mock)
        {
            return;
        }

        mock.VerifyAll();
    }

    private static bool ShouldRecurse(object value, Assembly testAssembly)
    {
        if (!VerificationObjectGraphWalker.ShouldRecurseInTestObjectGraph(value, testAssembly))
        {
            return false;
        }

        return value is not Mock;
    }
}
