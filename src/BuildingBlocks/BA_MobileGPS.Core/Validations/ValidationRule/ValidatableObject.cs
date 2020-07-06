using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core
{
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity
    {
        private List<string> _errors;
        private string _errorFirst = string.Empty;
        private T _value;
        private bool _isValid;
        private bool _isNotValid;

        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        public event EventHandler<T> OnChanged;

        public List<string> Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public string ErrorFirst
        {
            get
            {
                return _errorFirst;
            }
            set
            {
                _errorFirst = value;
                RaisePropertyChanged(() => ErrorFirst);
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnChanged?.Invoke(this, _value);
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsNotValid
        {
            get
            {
                return _isNotValid;
            }
            set
            {
                _isNotValid = value;
                RaisePropertyChanged(() => IsNotValid);
            }
        }

        public string ValidationDescriptions => string.Join(Environment.NewLine, Validations.Select(v => v.ValidationMessage));

        public ValidatableObject()
        {
            _isValid = true;
            _isNotValid = false;
            _errors = new List<string>();
        }

        public bool Validate()
        {
            Errors.Clear();

            Errors = Validations.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage).ToList();

            if (Errors.Count > 0)
            {
                ErrorFirst = Errors[0];
            }

            IsValid = !Errors.Any();
            IsNotValid = !IsValid;

            return IsValid;
        }
    }
}