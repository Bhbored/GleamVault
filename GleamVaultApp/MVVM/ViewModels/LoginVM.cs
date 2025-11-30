using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GleamVault.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginVM : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _isPasswordHidden = true;
        private bool _isBusy = false;
        private string _emailError = string.Empty;
        private string _passwordError = string.Empty;
        private bool _emailErrorVisible = false;
        private bool _passwordErrorVisible = false;

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

        private ICommand? _loginCommand;
        public ICommand LoginCommand => _loginCommand ??= new Command(async () => await LoginAsync(), () => !IsBusy);

        public ICommand TogglePasswordVisibilityCommand => new Command(() => IsPasswordHidden = !IsPasswordHidden);

        public ICommand GoToSignUpCommand => new Command(async () => await GoToSignUpAsync());

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

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            var emailValid = ValidateEmail();
            var passwordValid = ValidatePassword();

            if (!emailValid || !passwordValid)
            {
                return;
            }

            IsBusy = true;
            OnPropertyChanged(nameof(LoginCommand));

            try
            {
                await Task.Delay(1500);

                if (Email == "admin@gleamvault.com" && Password == "admin123")
                {
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    EmailError = "Invalid email or password";
                    EmailErrorVisible = true;
                    PasswordError = "Invalid email or password";
                    PasswordErrorVisible = true;
                }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

