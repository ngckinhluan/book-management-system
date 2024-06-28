using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.ApplicationServices;
using Repository.Entities;
using Service;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace BookManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly UserAccount _user;
    private readonly BookService _book;
    private readonly BookCategoryService _category;

    public MainWindow(UserAccount user)
    {
        _user = user;
        _book = new BookService();
        _category = new BookCategoryService();
        InitializeComponent();
        List<BookCategory> categories = _category.GetAll();
        BookGenreComboBox.ItemsSource = categories;
        BookGenreComboBox.DisplayMemberPath = "BookGenreType";
        BookGenreComboBox.SelectedValuePath = "BookCategoryId";
    }

    private void LoadList()
    {
        List<Book> books = _book.GetAll();
        List<BookCategory> categories = _category.GetAll();
        var joinList = books.Join(
            categories,
            b => b.BookCategoryId,
            c => c.BookCategoryId,
            (b, c) => new
            {
                b.BookId,
                b.BookName,
                b.Description,
                b.PublicationDate,
                b.Quantity,
                b.Price,
                b.Author,
                b.BookCategoryId,
                c.BookGenreType
            }
        ).ToList();
        BookListDataGrid.ItemsSource = joinList;
    }

    private void BookMainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadList();
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }

    private bool IsAdmin() => _user.Role == 1;

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsAdmin())
        {
            if (ValidateBookInput())
            {
                Book book = new()
                {
                    BookId = int.Parse(BookIdTextBox.Text),
                    BookName = BookNameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    PublicationDate = DateTime.Parse(PublicationDateTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text),
                    Price = double.Parse(PriceTextBox.Text),
                    Author = AuthorTextBox.Text,
                    BookCategoryId = (int)BookGenreComboBox.SelectedValue
                };
                bool added = _book.Add(book);
                if (added)
                {
                    MessageBox.Show("Book added successfully!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadList();
                    ClearTextBox();
                }
            }
        }
        else
        {
            MessageBox.Show("You do not have permission to access this function!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsAdmin())
        {
            if (BookListDataGrid.SelectedItem != null)
            {
                Book book = _book.GetAll().Find(b => b.BookId == int.Parse(BookIdTextBox.Text));
                //var selectedBook = (dynamic)BookListDataGrid.SelectedItem;
                //int bookId = selectedBook.BookId;
                //bool deleted = _book.Delete(bookId);
                bool deleted = _book.Remove(book);
                if (deleted)
                {
                    MessageBox.Show("Book deleted successfully!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadList();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("You do not have permission to access this function!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ClearTextBox()
    {
        BookIdTextBox.Text = string.Empty;
        BookNameTextBox.Text = string.Empty;
        DescriptionTextBox.Text = string.Empty;
        PublicationDateTextBox.Text = string.Empty;
        QuantityTextBox.Text = string.Empty;
        PriceTextBox.Text = string.Empty;
        AuthorTextBox.Text = string.Empty;
        BookGenreComboBox.SelectedIndex = 0;
    }

    private bool ValidateBookInput()
    {
        if (BookGenreComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select a book category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        if (string.IsNullOrWhiteSpace(BookIdTextBox.Text) ||
            string.IsNullOrWhiteSpace(BookNameTextBox.Text) ||
            string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
            string.IsNullOrWhiteSpace(PublicationDateTextBox.Text) ||
            string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
            string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
            string.IsNullOrWhiteSpace(AuthorTextBox.Text))
        {
            MessageBox.Show("All fields are required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0 || price >= 4000000 ||
            !int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0 || quantity >= 4000000)
        {
            MessageBox.Show("Price and Quantity must be greater than or equal to 0 and less than 4,000,000.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        if (BookNameTextBox.Text.Length < 5 || BookNameTextBox.Text.Length > 90 || !BookNameTextBox.Text.Split().All(word => char.IsUpper(word[0])))
        {
            MessageBox.Show("Book name must be 5-90 characters long and each word must begin with a capital letter.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        return true;
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        List<Book> searchList = _book.SearchByKeyword(b =>
            b.BookName.ToLower().Contains(SearchTextBox.Text.ToLower()) ||
            b.Description.ToLower().Contains(SearchTextBox.Text.ToLower()));
        List<BookCategory> categories = _category.GetAll();
        var joinSearchList = searchList.Join(
            categories,
            b => b.BookCategoryId,
            c => c.BookCategoryId,
            (b, c) => new
            {
                b.BookId,
                b.BookName,
                b.Description,
                b.PublicationDate,
                b.Quantity,
                b.Price,
                b.Author,
                b.BookCategoryId,
                c.BookGenreType
            }
        ).ToList();
        BookListDataGrid.ItemsSource = joinSearchList;
    }

    private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsAdmin())
        {
            if (BookListDataGrid.SelectedItem != null)
            {
                if (!ValidateBookInput()) return;
                Book book = _book.GetAll().Find(b => b.BookId == int.Parse(BookIdTextBox.Text));
                if (book != null)
                {
                    book.BookName = BookNameTextBox.Text;
                    book.Description = DescriptionTextBox.Text;
                    book.PublicationDate = DateTime.Parse(PublicationDateTextBox.Text);
                    book.Quantity = int.Parse(QuantityTextBox.Text);
                    book.Price = double.Parse(PriceTextBox.Text);
                    book.Author = AuthorTextBox.Text;
                    book.BookCategoryId = (int)BookGenreComboBox.SelectedValue;
                };
                bool updated = _book.Update(book); 
                if (updated)
                {
                    MessageBox.Show("Book updated successfully!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadList(); 
                }
                else
                {
                    MessageBox.Show("Failed to update the book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a book to update!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("You do not have permission to access this function!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void BookListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BookListDataGrid.SelectedItem != null)
        {
            var selectedBook = (dynamic)BookListDataGrid.SelectedItem;
            BookIdTextBox.Text = selectedBook.BookId.ToString();
            BookNameTextBox.Text = selectedBook.BookName;
            DescriptionTextBox.Text = selectedBook.Description;
            PublicationDateTextBox.Text = selectedBook.PublicationDate.ToString("d");
            QuantityTextBox.Text = selectedBook.Quantity.ToString();
            PriceTextBox.Text = selectedBook.Price.ToString();
            AuthorTextBox.Text = selectedBook.Author;
            BookGenreComboBox.SelectedValue = selectedBook.BookCategoryId;
            BookIdTextBox.IsReadOnly = true;
        }
    }

}