using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EduCalc.Wpf.Commands;
using EduCalc.Models;

namespace EduCalc.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private EducationalSystem _system = new();
        public EducationalSystem System
        {
            get => _system;
            set
            {
                _system = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public string CalculatedResults => $"F: {System.F:F2}, G: {System.G:F2}, H: {System.H:F2}, Y: {System.Y:F2}, Уровень: {System.S}";

        public ICommand CalculateCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
            ExportCommand = new RelayCommand(Export);
            ImportCommand = new RelayCommand(Import);
        }

        private void Calculate() => OnPropertyChanged(nameof(CalculatedResults));
        private void Export() { /* Реализация экспорта в JSON */ }
        private void Import() { /* Реализация импорта из JSON */ }

        private void Validate()
        {
            if (System.StudentCount <= 0)
                System.AddError("StudentCount", "Количество учеников должно быть больше нуля");
            else
                System.ClearErrors("StudentCount");

            // Добавьте другие проверки по необходимости
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
