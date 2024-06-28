using System.Windows;
using Repository.Entities;
using Service;

namespace BookManager;

public partial class LoginWindow : Window
{
    private readonly UserAccountService _user;
    public LoginWindow()
    {
        _user = new UserAccountService();
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        UserAccount user = _user.GetAll().Where(u =>
            u.Email == EmailTextBox.Text && u.Password == PasswordTextBox.Text && u.Role != 3).FirstOrDefault();
        if (user != null)
        {
            MainWindow mainWindow = new(user);
            mainWindow.Show();
            Close();
        }
        else
        {
            MessageBox.Show("You have no permission to access this function!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }

}