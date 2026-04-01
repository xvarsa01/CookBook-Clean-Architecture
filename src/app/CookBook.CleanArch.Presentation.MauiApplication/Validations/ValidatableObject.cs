using CommunityToolkit.Mvvm.ComponentModel;

namespace CookBook.CleanArch.Presentation.MauiApplication.Validations;

public class ValidatableObject<T> : ObservableObject, IValidity
{
    private IEnumerable<string> _errors;
    private bool _isValid;

    public ValidatableObject()
    {
        _isValid = true;
        _errors = Enumerable.Empty<string>();
    }

    public List<IValidationRule<T>> Validations { get; } = new();

    public IEnumerable<string> Errors
    {
        get => _errors;
        private set => SetProperty(ref _errors, value);
    }

    public T Value
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool IsValid
    {
        get => _isValid;
        private set => SetProperty(ref _isValid, value);
    }

    public bool Validate()
    {
        Errors = Validations
                     ?.Where(v => !v.Check(Value))
                     ?.Select(v => v.ValidationMessage)
                     ?.ToArray()
                 ?? Enumerable.Empty<string>();

        IsValid = !Errors.Any();

        return IsValid;
    }
}
