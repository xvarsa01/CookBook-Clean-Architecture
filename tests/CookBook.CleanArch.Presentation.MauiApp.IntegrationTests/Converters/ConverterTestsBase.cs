using System.Runtime.CompilerServices;

namespace CookBook.CleanArch.Presentation.MauiApp.IntegrationTests.Converters;


public abstract class ConverterTestsBase
{
    protected static T CreateConverter<T>() where T : class
        => (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
}
