using System.ComponentModel;

namespace WinstonCheckIn.Services;

public class AuthenticationStateService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public bool IsAuthenticated { get; private set; }
    public string? CurrentUser { get; private set; }

    public void Login(string username)
    {
        IsAuthenticated = true;
        CurrentUser = username;
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(CurrentUser));
    }

    public void Logout()
    {
        IsAuthenticated = false;
        CurrentUser = null;
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(CurrentUser));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class LoginService
{
    private readonly IConfiguration _configuration;
    private readonly AuthenticationStateService _authStateService;

    public LoginService(IConfiguration configuration, AuthenticationStateService authStateService)
    {
        _configuration = configuration;
        _authStateService = authStateService;
    }

    public bool Authenticate(string username, string password)
    {
        var configUsername = _configuration["LoginCredentials:Username"];
        var configPassword = _configuration["LoginCredentials:Password"];
        
        if (username == configUsername && password == configPassword)
        {
            _authStateService.Login(username);
            return true;
        }
        return false;
    }
}