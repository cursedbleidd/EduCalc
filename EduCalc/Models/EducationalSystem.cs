using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EduCalc.Models;
public class EducationalSystem : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> _errors = new();
    private readonly CompositeNode _root;
    
    public CompositeNode Root => _root;
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    
    public bool HasErrors => _errors.Any();

    public EducationalSystem()
    {
        _root = new CompositeNode("Root", new[] { 0.25, 0.25, 0.25, 0.25 });
        InitializeTree();
    }

    private void InitializeTree()
    {
        // Компонент F (Материальная база)
        var fNode = new CompositeNode("F", new[] { 0.5, 0.5 });
        var f1Node = new CompositeNode("f1", new[] { 0.33, 0.33, 0.33 });
        var f2Node = new CompositeNode("f2", new[] { 0.25, 0.5, 0.25 });

        // Листья для f1
        var totalAreaPerStudent = new TreeNode("TotalAreaPerStudent", () => TotalArea / StudentCount * 4);
        var computerPerStudent = new TreeNode("ComputerPerStudent", () => (ComputerCount / (double)StudentCount) * 300);
        var bookPerStudent = new TreeNode("BookPerStudent", () => Math.Min((BookCount / (double)StudentCount) * 100, 100));

        f1Node.Children.Add(totalAreaPerStudent);
        f1Node.Children.Add(computerPerStudent);
        f1Node.Children.Add(bookPerStudent);

        // Листья для f2
        var teachersEdu = new TreeNode("TeachersWithHigherEdu", () => TeachersWithHigherEdu);
        var certifiedTeachers = new TreeNode("CertifiedTeachers", () => CertifiedTeachers);
        var teachersAge = new TreeNode("TeachersAge", () => {
            double total = JuniorTeachers + MidCareerTeachers + SeniorTeachers;
            return 100 - (Math.Abs(MidCareerTeachers - JuniorTeachers) / total) * 100
                     - (Math.Abs(MidCareerTeachers - SeniorTeachers) / total) * 100;
        });

        f2Node.Children.Add(teachersEdu);
        f2Node.Children.Add(certifiedTeachers);
        f2Node.Children.Add(teachersAge);

        fNode.Children.Add(f1Node);
        fNode.Children.Add(f2Node);

        // Компонент G (Организация обучения)
        var gNode = new CompositeNode("G", new[] { 0.4, 0.2, 0.2, 0.2 });
        
        var examScores = new CompositeNode("ExamScores", new[] { 0.25, 0.25, 0.25, 0.25 });
        var ogeCore = new TreeNode("OGECore", () => OGECoreAvg);
        var ogeOptional = new TreeNode("OGEOptional", () => OGEOptionalAvg);
        var egeCore = new TreeNode("EGECore", () => EGECoreAvg);
        var egeOptional = new TreeNode("EGEOptional", () => EGEOptionalAvg);

        examScores.Children.Add(ogeCore);
        examScores.Children.Add(ogeOptional);
        examScores.Children.Add(egeCore);
        examScores.Children.Add(egeOptional);

        var honors = new TreeNode("Honors", () => 
            (HonorsGraduates / (double)TotalGraduates) * 100 * 5);
        var capacity = new TreeNode("Capacity", () => 
            ExcessPercent == 0 ? 100 : 100 - ExcessPercent);
        var profile = new TreeNode("Profile", () => {
            double val = ProfileSeniors / 100 * 3 * AdvancedJuniors;
            return Math.Max(50, Math.Min(100, val));
        });

        gNode.Children.Add(examScores);
        gNode.Children.Add(honors);
        gNode.Children.Add(capacity);
        gNode.Children.Add(profile);

        // Компонент H (Инновационная деятельность)
        var hNode = new CompositeNode("H", new[] { 0.33, 0.33, 0.33 });
        
        var olympiadSuccess = new TreeNode("OlympiadSuccess", () => 
            (VSOHWinners / (double)SeniorStudents) * 100 * 10);
        var digitalClubs = new TreeNode("DigitalClubs", () => DigitalClubs);
        var additionalEdu = new TreeNode("AdditionalEdu", () => AdditionalEdu);
        var careerGuidance = new TreeNode("CareerGuidance", () => CareerGuidance);
        var projectWork = new TreeNode("ProjectWork", () => ProjectWork);
        var arr = new[] { olympiadSuccess, digitalClubs, additionalEdu, careerGuidance, projectWork };
        // H берет три лучших показателя
        foreach (var q in arr)
            hNode.Children.Add(q);

        // Компонент Y (Когнитивные способности)
        var yNode = new CompositeNode("Y", new[] { 0.33, 0.33, 0.33 });
        
        var memory = new CompositeNode("Memory", new[] { 0.25, 0.25, 0.25, 0.25 });
        var shortTerm = new TreeNode("ShortTermMemory", () => ShortTermMemory);
        var procedural = new TreeNode("ProceduralMemory", () => ProceduralMemory);
        var semantic = new TreeNode("SemanticMemory", () => SemanticMemory);
        var episodic = new TreeNode("EpisodicMemory", () => EpisodicMemory);

        memory.Children.Add(shortTerm);
        memory.Children.Add(procedural);
        memory.Children.Add(semantic);
        memory.Children.Add(episodic);

        var creativity = new TreeNode("Creativity", () => Creativity);
        var logic = new TreeNode("Logic", () => Logic);

        yNode.Children.Add(memory);
        yNode.Children.Add(creativity);
        yNode.Children.Add(logic);

        _root.Children.Add(fNode);
        _root.Children.Add(gNode);
        _root.Children.Add(hNode);
        _root.Children.Add(yNode);
    }
    public Dictionary<string, double> GetRecomendations(TreeNode node, double targetScore)
    {
        double diffScore = targetScore - node.CalculatedValue;
        var dict = node.GetWeightsWithNames();
        double coef = diffScore / dict.Values.Sum(d => d * d);
        return dict.ToDictionary(kv => kv.Key, kv => kv.Value * coef);
    }

    // Свойства для ввода данных
    private double _totalArea;
    public double TotalArea
    {
        get => _totalArea;
        set
        {
            if (value < 0)
                AddError(nameof(TotalArea), "Площадь не может быть отрицательной");
            else
                ClearErrors(nameof(TotalArea));
            _totalArea = value;
            OnPropertyChanged();
        }
    }

    private int _studentCount;
    public int StudentCount
    {
        get => _studentCount;
        set
        {
            if (value <= 0)
                AddError(nameof(StudentCount), "Количество учеников должно быть больше нуля");
            else
                ClearErrors(nameof(StudentCount));
            _studentCount = value;
            OnPropertyChanged();
        }
    }

    private int _computerCount;
    public int ComputerCount
    {
        get => _computerCount;
        set
        {
            if (value < 0)
                AddError(nameof(ComputerCount), "Количество компьютеров не может быть отрицательным");
            else
                ClearErrors(nameof(ComputerCount));
            _computerCount = value;
            OnPropertyChanged();
        }
    }

    private int _bookCount;
    public int BookCount
    {
        get => _bookCount;
        set
        {
            if (value < 0)
                AddError(nameof(BookCount), "Количество книг не может быть отрицательной");
            else
                ClearErrors(nameof(BookCount));
            _bookCount = value;
            OnPropertyChanged();
        }
    }

    // Педагогические кадры (f2)
    private double _teachersWithHigherEdu;
    public double TeachersWithHigherEdu
    {
        get => _teachersWithHigherEdu;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(TeachersWithHigherEdu), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(TeachersWithHigherEdu));
            _teachersWithHigherEdu = value;
            OnPropertyChanged();
        }
    }

    private double _certifiedTeachers;
    public double CertifiedTeachers
    {
        get => _certifiedTeachers;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(CertifiedTeachers), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(CertifiedTeachers));
            _certifiedTeachers = value;
            OnPropertyChanged();
        }
    }

    private int _juniorTeachers;
    public int JuniorTeachers
    {
        get => _juniorTeachers;
        set
        {
            if (value < 0)
                AddError(nameof(JuniorTeachers), "Число не может быть отрицательным");
            else
                ClearErrors(nameof(JuniorTeachers));
            _juniorTeachers = value;
            OnPropertyChanged();
        }
    }

    private int _midCareerTeachers;
    public int MidCareerTeachers
    {
        get => _midCareerTeachers;
        set
        {
            if (value < 0)
                AddError(nameof(MidCareerTeachers), "Число не может быть отрицательным");
            else
                ClearErrors(nameof(MidCareerTeachers));
            _midCareerTeachers = value;
            OnPropertyChanged();
        }
    }

    private int _seniorTeachers;
    public int SeniorTeachers
    {
        get => _seniorTeachers;
        set
        {
            if (value < 0)
                AddError(nameof(SeniorTeachers), "Число не может быть отрицательным");
            else
                ClearErrors(nameof(SeniorTeachers));
            _seniorTeachers = value;
            OnPropertyChanged();
        }
    }

    // Организация обучения (G)
    private double _ogeCoreAvg;
    public double OGECoreAvg
    {
        get => _ogeCoreAvg;
        set
        {
            if (value < 0)
                AddError(nameof(OGECoreAvg), "Балл не может быть отрицательным");
            else
                ClearErrors(nameof(OGECoreAvg));
            _ogeCoreAvg = value;
            OnPropertyChanged();
        }
    }

    private double _ogeOptionalAvg;
    public double OGEOptionalAvg
    {
        get => _ogeOptionalAvg;
        set
        {
            if (value < 0)
                AddError(nameof(OGEOptionalAvg), "Балл не может быть отрицательным");
            else
                ClearErrors(nameof(OGEOptionalAvg));
            _ogeOptionalAvg = value;
            OnPropertyChanged();
        }
    }

    private double _egeCoreAvg;
    public double EGECoreAvg
    {
        get => _egeCoreAvg;
        set
        {
            if (value < 0)
                AddError(nameof(EGECoreAvg), "Балл не может быть отрицательным");
            else
                ClearErrors(nameof(EGECoreAvg));
            _egeCoreAvg = value;
            OnPropertyChanged();
        }
    }

    private double _egeOptionalAvg;
    public double EGEOptionalAvg
    {
        get => _egeOptionalAvg;
        set
        {
            if (value < 0)
                AddError(nameof(EGEOptionalAvg), "Балл не может быть отрицательным");
            else
                ClearErrors(nameof(EGEOptionalAvg));
            _egeOptionalAvg = value;
            OnPropertyChanged();
        }
    }

    private int _honorsGraduates;
    public int HonorsGraduates
    {
        get => _honorsGraduates;
        set
        {
            if (value < 0)
                AddError(nameof(HonorsGraduates), "Число не может быть отрицательным");
            else
                ClearErrors(nameof(HonorsGraduates));
            _honorsGraduates = value;
            OnPropertyChanged();
        }
    }

    private int _totalGraduates;
    public int TotalGraduates
    {
        get => _totalGraduates;
        set
        {
            if (value <= 0)
                AddError(nameof(TotalGraduates), "Число выпускников должно быть больше нуля");
            else
                ClearErrors(nameof(TotalGraduates));
            _totalGraduates = value;
            OnPropertyChanged();
        }
    }

    private double _excessPercent;
    public double ExcessPercent
    {
        get => _excessPercent;
        set
        {
            if (value < 0)
                AddError(nameof(ExcessPercent), "Процент не может быть отрицательным");
            else
                ClearErrors(nameof(ExcessPercent));
            _excessPercent = value;
            OnPropertyChanged();
        }
    }

    private double _profileSeniors;
    public double ProfileSeniors
    {
        get => _profileSeniors;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(ProfileSeniors), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(ProfileSeniors));
            _profileSeniors = value;
            OnPropertyChanged();
        }
    }

    private double _advancedJuniors;
    public double AdvancedJuniors
    {
        get => _advancedJuniors;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(AdvancedJuniors), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(AdvancedJuniors));
            _advancedJuniors = value;
            OnPropertyChanged();
        }
    }

    // Инновационная деятельность (H)
    private int _vsohWinners;
    public int VSOHWinners
    {
        get => _vsohWinners;
        set
        {
            if (value < 0)
                AddError(nameof(VSOHWinners), "Число победителей не может быть отрицательным");
            else
                ClearErrors(nameof(VSOHWinners));
            _vsohWinners = value;
            OnPropertyChanged();
        }
    }

    private int _seniorStudents;
    public int SeniorStudents
    {
        get => _seniorStudents;
        set
        {
            if (value <= 0)
                AddError(nameof(SeniorStudents), "Число старшеклассников должно быть больше нуля");
            else
                ClearErrors(nameof(SeniorStudents));
            _seniorStudents = value;
            OnPropertyChanged();
        }
    }

    private double _digitalClubs;
    public double DigitalClubs
    {
        get => _digitalClubs;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(DigitalClubs), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(DigitalClubs));
            _digitalClubs = value;
            OnPropertyChanged();
        }
    }

    private double _additionalEdu;
    public double AdditionalEdu
    {
        get => _additionalEdu;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(AdditionalEdu), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(AdditionalEdu));
            _additionalEdu = value;
            OnPropertyChanged();
        }
    }

    private double _careerGuidance;
    public double CareerGuidance
    {
        get => _careerGuidance;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(CareerGuidance), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(CareerGuidance));
            _careerGuidance = value;
            OnPropertyChanged();
        }
    }

    private double _projectWork;
    public double ProjectWork
    {
        get => _projectWork;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(ProjectWork), "Процент должен быть от 0 до 100");
            else
                ClearErrors(nameof(ProjectWork));
            _projectWork = value;
            OnPropertyChanged();
        }
    }

    // Когнитивные способности (Y)
    private double _shortTermMemory;
    public double ShortTermMemory
    {
        get => _shortTermMemory;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(ShortTermMemory), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(ShortTermMemory));
            _shortTermMemory = value;
            OnPropertyChanged();
        }
    }

    private double _proceduralMemory;
    public double ProceduralMemory
    {
        get => _proceduralMemory;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(ProceduralMemory), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(ProceduralMemory));
            _proceduralMemory = value;
            OnPropertyChanged();
        }
    }

    private double _semanticMemory;
    public double SemanticMemory
    {
        get => _semanticMemory;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(SemanticMemory), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(SemanticMemory));
            _semanticMemory = value;
            OnPropertyChanged();
        }
    }

    private double _episodicMemory;
    public double EpisodicMemory
    {
        get => _episodicMemory;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(EpisodicMemory), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(EpisodicMemory));
            _episodicMemory = value;
            OnPropertyChanged();
        }
    }

    private double _creativity;
    public double Creativity
    {
        get => _creativity;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(Creativity), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(Creativity));
            _creativity = value;
            OnPropertyChanged();
        }
    }

    private double _logic;
    public double Logic
    {
        get => _logic;
        set
        {
            if (value < 0 || value > 100)
                AddError(nameof(Logic), "Уровень должен быть от 0 до 100");
            else
                ClearErrors(nameof(Logic));
            _logic = value;
            OnPropertyChanged();
        }
    }

    // Вычисляемые параметры
    public double F => _root.Children[0].CalculatedValue;
    public double G => _root.Children[1].CalculatedValue;
    public double H => _root.Children[2].CalculatedValue;
    public double Y => _root.Children[3].CalculatedValue;
    public string S => DetermineS();

    private string DetermineS()
    {
        if (HasErrors) return "Есть ошибки ввода";

        bool yHigh = Y >= 80;
        bool yAboveAvg = Y >= 60;
        bool yAvg = Y >= 40;
        bool yBelowAvg = Y >= 20;

        int countFghHigh = 0;
        int countFghAboveAvg = 0;
        int countFghAvg = 0;
        int countFghBelowAvg = 0;

        foreach (var val in new[] { F, G, H })
        {
            if (val >= 80) countFghHigh++;
            else if (val >= 60) countFghAboveAvg++;
            else if (val >= 40) countFghAvg++;
            else if (val >= 20) countFghBelowAvg++;
        }

        if (yHigh && countFghHigh >= 2)
            return "Высокий";
        if ((yHigh || yAboveAvg) && countFghAboveAvg >= 2)
            return "Выше среднего";
        if ((yHigh || yAboveAvg || yAvg) && countFghAvg >= 2)
            return "Средний";
        if ((yBelowAvg && countFghAvg == 0) || countFghBelowAvg == 1)
            return "Ниже среднего";
        return "Низкий";
    }

    public Dictionary<string, (double coefficient, double value)> CompressTree()
    {
        var result = new Dictionary<string, (double, double)>();
        CompressNode(_root, result, 1.0);
        return result;
    }

    private void CompressNode(TreeNode node, Dictionary<string, (double, double)> result, double parentCoefficient)
    {
        if (node is CompositeNode composite)
        {
            for (int i = 0; i < composite.Children.Count; i++)
            {
                var child = composite.Children[i];
                var coefficient = composite.Coefficients[i] * parentCoefficient;
                CompressNode(child, result, coefficient);
            }
        }
        else
        {
            result[node.Name] = (parentCoefficient, node.Value);
        }
    }

    public IEnumerable GetErrors(string propertyName)
    {
        if (_errors.TryGetValue(propertyName, out List<string> errors))
            return errors;
        return Enumerable.Empty<string>();
    }

    public void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName)) 
            _errors[propertyName] = new List<string>();
        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    public void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
