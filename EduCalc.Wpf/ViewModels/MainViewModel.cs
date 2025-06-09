using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EduCalc.Wpf.Commands;
using EduCalc.Models;
using System.Collections.Generic;
using EduCalc.Wpf.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace EduCalc.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private EducationalSystem _system = new();
        public ObservableCollection<string> Levels { get; set; } = [
            "Ниже среднего", "Средний", "Выше среднего", "Высокий", "Максимальный"
        ];
        private string _selectedLevel;
        public string SelectedLevel 
        { 
            get => _selectedLevel; 
            set
            {
                _selectedLevel = value;
                OnPropertyChanged();
            } 
        }
        public TreeNode MaterialBase => _system.Root.Children[0];
        public TreeNode TeachingOrganization => _system.Root.Children[1];
        public TreeNode Innovation => _system.Root.Children[2];
        public TreeNode CognitiveAbilities => _system.Root.Children[3];

        public EducationalSystem System
        {
            get => _system;
            set
            {
                _system = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MaterialBase));
                OnPropertyChanged(nameof(TeachingOrganization));
                OnPropertyChanged(nameof(Innovation));
                OnPropertyChanged(nameof(CognitiveAbilities));
            }
        }

        public string CalculatedResults => $"F: {System.F:F2}, G: {System.G:F2}, H: {System.H:F2}, Y: {System.Y:F2}, Уровень: {System.S}";

        public ICommand CalculateCommand { get; }
        public ICommand ShowRecommendationsCommand { get; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
            ShowRecommendationsCommand = new RelayCommand(ShowRecommendations);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculatedResults)));
        }

        private void Calculate() 
        {
            OnPropertyChanged(nameof(CalculatedResults));
        }

        private void ShowRecommendations()
        {
            LevelNode selectedLevel;
            try
            {
                selectedLevel = LevelNodeExtensions.FromString(SelectedLevel);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Выберите желаемый уровень");
                return;
            }
            LevelNode currentLevel = LevelNodeExtensions.FromString(System.S);
            if (selectedLevel <= currentLevel)
            {
                MessageBox.Show("Желаемый уровень уже достигнут. Выберите уровень выше.");
                return;
            }
            var recommendations = System.CalcRecommendations(selectedLevel).Select(r => $"{r.Description} ({r.Id}) увеличьте на {r.Inc:F2}").ToList();
            var window = new RecommendationsWindow();
            window.DataContext = new RecommendationsViewModel(window, recommendations);
            window.ShowDialog();
        }

        private List<string> GenerateRecommendations()
        {
            var recommendations = new List<string>();

            if (System.F < 80)
                recommendations.Add("Материальная база требует улучшения.\n" +
                    "Рекомендуется увеличить параметры:\n" +
                    "f11 на 14,95\n" +
                    "f12 на 14,95\n" +
                    "f21 на 11,33\n" +
                    "f22 на 22,66\n" +
                    "f23 на 11,33");

            if (System.G < 80)
                recommendations.Add("Необходимо повысить качество организации обучения.\n" +
                    "Рекомендуется увеличить параметры:\n" +
                    "g11 на 1,96\n" +
                    "g12 на 1,96\n" +
                    "g13 на 1,96\n" +
                    "g14 на 1,96\n" +
                    "g2 на 3,92\n" +
                    "g4 на 3,92");
            if (System.H < 80)
                recommendations.Add("Рекомендуется развивать инновационную деятельность.\n" +
                    "Рекомендуется увеличить параметры:\n" +
                    "h1 на 26,42\n" +
                    "h4 на 10,17");

            if (System.Y < 80)
                recommendations.Add("Необходимо уделить внимание развитию когнитивных способностей учащихся.\n" +
                    "Рекомендуется увеличить параметры:\n" +
                    "y11 на 6,66\n" +
                    "y12 на 6,66\n" +
                    "y13 на 6,66\n" +
                    "y14 на 6,66\n" +
                    "y2 на 26,63\n" +
                    "y3 на 26,63");

            if (recommendations.Count == 0)
                recommendations.Add("Показатели в норме. Продолжайте поддерживать текущий уровень.");

            return recommendations;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
