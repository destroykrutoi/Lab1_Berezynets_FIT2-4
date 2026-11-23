using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StudentCabinet.Models;

namespace StudentCabinet.ViewModels;

public class StudentViewModel : INotifyPropertyChanged
{
    private readonly Student _student = new();

    public StudentViewModel()
    {
        AddStudentCommand = new Command(AddStudent, CanAddStudent);
    }

    public string FullName
    {
        get => _student.FullName;
        set
        {
            // перевірка на рівність — без зайвих сповіщень
            if (_student.FullName != value)
            {
                _student.FullName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Greeting));
                // стан кнопки "Додати" залежить від ПІБ
                ((Command)AddStudentCommand).ChangeCanExecute();
            }
        }
    }

    public string Group
    {
        get => _student.Group;
        set
        {
            if (_student.Group != value)
            {
                _student.Group = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Greeting));
            }
        }
    }

    public double AverageScore
    {
        get => _student.AverageScore;
        set
        {
            // бал округлюється до десятих, щоб повзунок давав зручні значення
            var rounded = Math.Round(value, 1);
            if (_student.AverageScore != rounded)
            {
                _student.AverageScore = rounded;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsExcellent));
            }
        }
    }

    // Обчислювані властивості для односторонньої прив'язки
    public string Greeting =>
        string.IsNullOrWhiteSpace(FullName)
            ? "Введіть дані студента"
            : $"Студент: {FullName}, група {Group}";

    public bool IsExcellent => AverageScore >= 4.0;

    // Частина 2: колекція та команда
    public ObservableCollection<Student> Students { get; } = new();

    public ICommand AddStudentCommand { get; }

    private void AddStudent()
    {
        Students.Add(new Student
        {
            FullName = FullName,
            Group = Group,
            AverageScore = AverageScore
        });

        FullName = string.Empty;
        Group = string.Empty;
        AverageScore = 0;
    }

    private bool CanAddStudent() => !string.IsNullOrWhiteSpace(FullName);

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
