using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using Wurs.Test.Utilities.Helpers.Reflection;

namespace Wurs.Test.Utilities.NSubstitute;

public static class SubstituteVerificationHelper
{
    public static void Verify(this object testInstance)
    {
        ArgumentNullException.ThrowIfNull(testInstance);

        var verifiedSubstitutes = new HashSet<object>(ReferenceEqualityComparer.Instance);

        VerificationObjectGraphWalker.Walk(testInstance, ShouldRecurse, value => VerifySubstituteWasUsedIfApplicable(value, verifiedSubstitutes));
    }

    private static void VerifySubstituteWasUsedIfApplicable(object value, HashSet<object> verifiedSubstitutes)
    {
        if (verifiedSubstitutes.Contains(value))
        {
            return;
        }

        IEnumerable<ICall> receivedCalls;

        try
        {
            receivedCalls = SubstituteExtensions.ReceivedCalls(value);
        }
        catch (NotASubstituteException)
        {
            return;
        }

        if (!receivedCalls.Any())
        {
            throw new InvalidOperationException($"Substitute of type '{value.GetType().FullName}' did not receive any calls.");
        }

        verifiedSubstitutes.Add(value);
    }

    private static bool ShouldRecurse(object value, Assembly testAssembly)
    {
        return VerificationObjectGraphWalker.ShouldRecurseInTestObjectGraph(value, testAssembly);
    }
}
