using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Extensions;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementFindElementsWithJsonPathCheckExtensions
{
    /// <summary>
    /// Checks that a single element exists in the JsonElement using the specified JSON path query.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="query">the JSON path query</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">No or more than one element found with the specified query.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasSingleElementAt(this ICheck<JsonElement> check, string query)
    {
        ExtensibilityHelper.BeginCheck(check)
            .Analyze((element, c) =>
            {
                try
                {
                    _ = element.GetRequiredElementAt(query);
                }
                catch (Exception e)
                {
                    c.Fail(e.Message);
                }
            })
            .OnNegate($"A single element is found at '{query}' whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that at least one element exist in the JsonElement using the specified JSON path query.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="query">the JSON path query</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">No element is found with the specified query.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasAtLeastOneElementAt(this ICheck<JsonElement> check, string query)
    {
        ExtensibilityHelper.BeginCheck(check)
            .Analyze((element, c) =>
            {
                try
                {
                    if (element.CountElementsAt(query) == 0)
                    {
                        c.Fail($"No element found at '{query}'.");
                    }
                }
                catch (Exception e)
                {
                    c.Fail(e.Message);
                }
            })
            .OnNegate($"At least one element found at '{query}' whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that multiple elements exist in the JsonElement using the specified JSON path query.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="query">the JSON path query</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">No or single element is found with the specified query.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasMultipleElementsAt(this ICheck<JsonElement> check, string query)
    {
        ExtensibilityHelper.BeginCheck(check)
            .Analyze((element, c) =>
            {
                try
                {
                    if (element.CountElementsAt(query) == 0)
                    {
                        c.Fail($"No element found at '{query}'.");
                    }
                    if (element.CountElementsAt(query) == 1)
                    {
                        c.Fail($"Only one element found at '{query}'.");
                    }
                }
                catch (Exception e)
                {
                    c.Fail(e.Message);
                }
            })
            .OnNegate($"Multiple elements found at '{query}' whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }
}