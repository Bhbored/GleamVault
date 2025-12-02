using PropertyChanged;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using GleamVault.Services.Interfaces;
using Shared.Models;
using Shared.Contracts;

namespace GleamVault.MVVM.ViewModels
{
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [AddINotifyPropertyChangedInterface]
    public class SignUpVM : INotifyPropertyChanged
    {

        #region fields

        private readonly IAdvanceHttpService _httpService;
        private readonly ISessionService _sessionService;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _isPasswordHidden = true;
        private bool _isConfirmPasswordHidden = true;
        private bool _isBusy = false;
        private string _nameError = string.Empty;
        private string _emailError = string.Empty;
        private string _passwordError = string.Empty;
        private string _confirmPasswordError = string.Empty;
        private bool _nameErrorVisible = false;
        private bool _emailErrorVisible = false;
        private bool _passwordErrorVisible = false;
        private bool _confirmPasswordErrorVisible = false;
        #endregion


        public SignUpVM(IAdvanceHttpService httpService, ISessionService sessionService)
        {
            _httpService = httpService;
            _sessionService = sessionService;
        }


        #region  Properties

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                ClearNameError();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
                ClearEmailError();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
                ClearPasswordError();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword == value) return;
                _confirmPassword = value;
                OnPropertyChanged();
                ClearConfirmPasswordError();
            }
        }

        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                if (_isPasswordHidden == value) return;
                _isPasswordHidden = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordEyeIcon));
            }
        }

        public bool IsConfirmPasswordHidden
        {
            get => _isConfirmPasswordHidden;
            set
            {
                if (_isConfirmPasswordHidden == value) return;
                _isConfirmPasswordHidden = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConfirmPasswordEyeIcon));
            }
        }

        public string PasswordEyeIcon => IsPasswordHidden ? "eye_closed.png" : "eye_open.png";
        public string ConfirmPasswordEyeIcon => IsConfirmPasswordHidden ? "eye_closed.png" : "eye_open.png";

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignUpCommand));
            }
        }

        public string NameError
        {
            get => _nameError;
            set
            {
                if (_nameError == value) return;
                _nameError = value;
                OnPropertyChanged();
            }
        }

        public string EmailError
        {
            get => _emailError;
            set
            {
                if (_emailError == value) return;
                _emailError = value;
                OnPropertyChanged();
            }
        }

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                if (_passwordError == value) return;
                _passwordError = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set
            {
                if (_confirmPasswordError == value) return;
                _confirmPasswordError = value;
                OnPropertyChanged();
            }
        }

        public bool NameErrorVisible
        {
            get => _nameErrorVisible;
            set
            {
                if (_nameErrorVisible == value) return;
                _nameErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public bool EmailErrorVisible
        {
            get => _emailErrorVisible;
            set
            {
                if (_emailErrorVisible == value) return;
                _emailErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public bool PasswordErrorVisible
        {
            get => _passwordErrorVisible;
            set
            {
                if (_passwordErrorVisible == value) return;
                _passwordErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public bool ConfirmPasswordErrorVisible
        {
            get => _confirmPasswordErrorVisible;
            set
            {
                if (_confirmPasswordErrorVisible == value) return;
                _confirmPasswordErrorVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region commands

        private ICommand? _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= new Command(async () => await SignUpAsync(), () => !IsBusy);

        public ICommand TogglePasswordVisibilityCommand => new Command(() => IsPasswordHidden = !IsPasswordHidden);
        public ICommand ToggleConfirmPasswordVisibilityCommand => new Command(() => IsConfirmPasswordHidden = !IsConfirmPasswordHidden);
        public ICommand GoToLoginCommand => new Command(async () => await GoToLoginAsync());
        #endregion

        #region methods
        private void ClearNameError()
        {
            if (NameErrorVisible)
            {
                NameError = string.Empty;
                NameErrorVisible = false;
            }
        }

        private void ClearEmailError()
        {
            if (EmailErrorVisible)
            {
                EmailError = string.Empty;
                EmailErrorVisible = false;
            }
        }

        private void ClearPasswordError()
        {
            if (PasswordErrorVisible)
            {
                PasswordError = string.Empty;
                PasswordErrorVisible = false;
            }
        }

        private void ClearConfirmPasswordError()
        {
            if (ConfirmPasswordErrorVisible)
            {
                ConfirmPasswordError = string.Empty;
                ConfirmPasswordErrorVisible = false;
            }
        }

        private bool ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameError = "Name is required";
                NameErrorVisible = true;
                return false;
            }

            if (Name.Length < 2)
            {
                NameError = "Name must be at least 2 characters";
                NameErrorVisible = true;
                return false;
            }

            NameError = string.Empty;
            NameErrorVisible = false;
            return true;
        }

        private bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
                EmailErrorVisible = true;
                return false;
            }

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailPattern))
            {
                EmailError = "Please enter a valid email address";
                EmailErrorVisible = true;
                return false;
            }

            EmailError = string.Empty;
            EmailErrorVisible = false;
            return true;
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                PasswordErrorVisible = true;
                return false;
            }

            if (Password.Length < 6)
            {
                PasswordError = "Password must be at least 6 characters";
                PasswordErrorVisible = true;
                return false;
            }

            PasswordError = string.Empty;
            PasswordErrorVisible = false;
            return true;
        }

        private bool ValidateConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError = "Please confirm your password";
                ConfirmPasswordErrorVisible = true;
                return false;
            }

            if (Password != ConfirmPassword)
            {
                ConfirmPasswordError = "Passwords do not match";
                ConfirmPasswordErrorVisible = true;
                return false;
            }

            ConfirmPasswordError = string.Empty;
            ConfirmPasswordErrorVisible = false;
            return true;
        }
        #endregion

        #region Tasks

        private async Task SignUpAsync()
        {
            if (IsBusy) return;

            var nameValid = ValidateName();
            var emailValid = ValidateEmail();
            var passwordValid = ValidatePassword();
            var confirmPasswordValid = ValidateConfirmPassword();

            if (!nameValid || !emailValid || !passwordValid || !confirmPasswordValid)
            {
                return;
            }

            IsBusy = true;
            OnPropertyChanged(nameof(SignUpCommand));

            try
            {
                var registerRequest = new RegisterRequest
                {
                    Username = Email.Split('@')[0], 
                    FullName = Name,
                    Email = Email,
                    Password = Password
                };

                var registerUrl = Constants.WEB_API_URL + "api/account/register";
                var result = await _httpService.Post<RegisterRequest, LoginResponse>(registerUrl, registerRequest);

                if (result.IsSuccess && result.Result != null)
                {
                    await _sessionService.SaveSessionAsync(result.Result);
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    var errorMessage = result.ErrorMessage ?? "Registration failed. Please try again.";
                    EmailError = errorMessage;
                    EmailErrorVisible = true;
                }
            }
            catch (Exception ex)
            {
                EmailError = "An error occurred. Please try again.";
                EmailErrorVisible = true;
                Debug.WriteLine($"SignUp error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(SignUpCommand));
            }
        }

        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        #endregion


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

