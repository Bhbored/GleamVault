using PropertyChanged;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GleamVault.Services.Interfaces;
using Shared.Models;
using Shared.Contracts;
using Shared.Models.Enums;
using System.Diagnostics;
using GleamVault.Services;
using GleamVault.MVVM.Views;
using GleamVaultApp;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginVM : INotifyPropertyChanged
    {
        #region fiels
        private readonly IAdvanceHttpService _httpService;
        private readonly ISessionService _sessionService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isPasswordHidden = true;
        private bool _isBusy = false;
        private string _usernameError = string.Empty;
        private string _passwordError = string.Empty;
        private bool _usernameErrorVisible = false;
        private bool _passwordErrorVisible = false;
        #endregion

        public IShopDataStore ShopDataStore { get; }


        public LoginVM(IAdvanceHttpService httpService, ISessionService sessionService, IShopDataStore shopDataStore)
        {
            _httpService = httpService;
            _sessionService = sessionService;
            ShopDataStore = shopDataStore;
        }

        #region Properties
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged();
                ClearUsernameError();
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

        public string PasswordEyeIcon => IsPasswordHidden ? "eye_closed.png" : "eye_open.png";

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LoginCommand));
            }
        }

        public string UsernameError
        {
            get => _usernameError;
            set
            {
                if (_usernameError == value) return;
                _usernameError = value;
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

        public bool UsernameErrorVisible
        {
            get => _usernameErrorVisible;
            set
            {
                if (_usernameErrorVisible == value) return;
                _usernameErrorVisible = value;
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

        #endregion

        #region commands

        private ICommand? _loginCommand;
        public ICommand LoginCommand => _loginCommand ??= new Command(async () => await LoginAsync(), () => !IsBusy);

        public ICommand TogglePasswordVisibilityCommand => new Command(() => IsPasswordHidden = !IsPasswordHidden);

        public ICommand GoToSignUpCommand => new Command(async () => await GoToSignUpAsync());
        #endregion

        #region methods

        private void ClearUsernameError()
        {
            if (UsernameErrorVisible)
            {
                UsernameError = string.Empty;
                UsernameErrorVisible = false;
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

        private bool ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                UsernameError = "Username is required";
                UsernameErrorVisible = true;
                return false;
            }

            if (Username.Length < 3)
            {
                UsernameError = "Username must be at least 3 characters";
                UsernameErrorVisible = true;
                return false;
            }

            UsernameError = string.Empty;
            UsernameErrorVisible = false;
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
        #endregion

        #region Tasks

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            var usernameValid = ValidateUsername();
            var passwordValid = ValidatePassword();

            if (!usernameValid || !passwordValid)
            {
                return;
            }

            IsBusy = true;
            OnPropertyChanged(nameof(LoginCommand));

            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = Username,
                    Password = Password
                };


                var response = await ShopDataStore.Login(loginRequest);
                Debug.WriteLine($"response:{response.Message}");

                if (response != null && !string.IsNullOrEmpty(response.ApiKey))
                {
                    await _sessionService.SaveSessionAsync(response);

                    bool isAdmin = response.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                                  response.Role.Equals(UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);

                    if (Shell.Current is AppShell appShell)
                    {
                        appShell.SetFlyoutItemsVisibility(isAdmin);
                    }

                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    var errorMessage = response?.Message ?? "Invalid username or password";
                    UsernameError = errorMessage;
                    UsernameErrorVisible = true;
                    PasswordError = errorMessage;
                    PasswordErrorVisible = true;
                }

                //if (result.IsSuccess && result.Result != null)
                //{
                //    await _sessionService.SaveSessionAsync(result.Result);

                //    await Shell.Current.GoToAsync("//HomePage");
                //}
                //else
                //{
                //    var errorMessage = result.ErrorMessage ?? "Invalid username or password";
                //    UsernameError = errorMessage;
                //    UsernameErrorVisible = true;
                //    PasswordError = errorMessage;
                //    PasswordErrorVisible = true;
                //}
            }
            catch (Exception ex)
            {
                UsernameError = "An error occurred. Please try again.";
                UsernameErrorVisible = true;
                Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(LoginCommand));
            }
        }

        private async Task GoToSignUpAsync()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

