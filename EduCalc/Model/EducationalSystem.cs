﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using EduCalc.Entity;
using EduCalc.Entity.Level;
using EduCalc.Entity.Tree;

namespace EduCalc.Models;
public class EducationalSystem : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> _errors = new();
    private readonly CompositeNode _root;
    
    public CompositeNode Root => _root;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private List<InputComponentSetting> _componentsSettings;
    public List<InputComponentSetting> ComponentsSettings
    {
        get => _componentsSettings;
        set
        {
            _componentsSettings = value;
            OnPropertyChanged();
        }
    }
    public bool HasErrors => _errors.Any();

    public EducationalSystem()
    {
        _root = new CompositeNode("Root", new[] { 0.25, 0.25, 0.25, 0.25 });
        _componentsSettings = new();
        InitializeTree();
    }

    private void InitializeTree()
    {
        // Компонент F (Материальная база)
        var fNode = new CompositeNode("F", new[] { 0.5, 0.5 });
        var f1Node = new CompositeNode("f1", new[] { 0.33, 0.33, 0.33 });
        var f2Node = new CompositeNode("f2", new[] { 0.25, 0.5, 0.25 });

        // Листья для f1
        var totalAreaPerStudent = new TreeNode("TotalAreaPerStudent", "f11", "Площадь здания на ученика",
            () => StudentCount == 0 ? .0 : TotalArea / StudentCount * 4);
        var computerPerStudent = new TreeNode("ComputerPerStudent", "f12", "Компьютеров на 300 учеников",
            () => StudentCount == 0 ? .0 : (ComputerCount / (double)StudentCount) * 300);
        var bookPerStudent = new TreeNode("BookPerStudent", "f13", "Книг на ученика",
            () => StudentCount == 0 ? .0 : Math.Min((BookCount / (double)StudentCount) * 100, 100));

        f1Node.Children.Add(totalAreaPerStudent);
        f1Node.Children.Add(computerPerStudent);
        f1Node.Children.Add(bookPerStudent);
        ComponentsSettings.Add(totalAreaPerStudent);
        ComponentsSettings.Add(computerPerStudent);
        ComponentsSettings.Add(bookPerStudent);

        // Листья для f2
        var teachersEdu = new TreeNode("TeachersWithHigherEdu", "f21", "Процент учителей с высшим образованием", () => TeachersWithHigherEdu);
        var certifiedTeachers = new TreeNode("CertifiedTeachers", "f22", "Процент аттестованных учителей", () => CertifiedTeachers);
        var teachersAge = new TreeNode("TeachersAge", "f23", "Возрастной показатель учителей",() => {
            double total = JuniorTeachers + MidCareerTeachers + SeniorTeachers;
            if (total == 0)
                return .0;
            return 100 - (Math.Abs(MidCareerTeachers - JuniorTeachers) / total) * 100
                     - (Math.Abs(MidCareerTeachers - SeniorTeachers) / total) * 100;
        });

        f2Node.Children.Add(teachersEdu);
        f2Node.Children.Add(certifiedTeachers);
        f2Node.Children.Add(teachersAge);
        ComponentsSettings.Add(teachersEdu);
        ComponentsSettings.Add(certifiedTeachers);
        ComponentsSettings.Add(teachersAge);

        fNode.Children.Add(f1Node);
        fNode.Children.Add(f2Node);

        // Компонент G (Организация обучения)
        var gNode = new CompositeNode("G", new[] { 0.4, 0.2, 0.2, 0.2 });
        
        var examScores = new CompositeNode("ExamScores", new[] { 0.25, 0.25, 0.25, 0.25 });
        var ogeCore = new TreeNode("OGECore", "g11", "Средний балл ОГЭ по основным", () => OGECoreAvg);
        var ogeOptional = new TreeNode("OGEOptional", "g12", "Средний балл ОГЭ (по выбору)", () => OGEOptionalAvg);
        var egeCore = new TreeNode("EGECore", "g13", "Средний балл ЕГЭ по основным", () => EGECoreAvg);
        var egeOptional = new TreeNode("EGEOptional", "g14", "Средний балл ЕГЭ (по выбору)", () => EGEOptionalAvg);

        examScores.Children.Add(ogeCore);
        examScores.Children.Add(ogeOptional);
        examScores.Children.Add(egeCore);
        examScores.Children.Add(egeOptional);
        ComponentsSettings.Add(ogeCore);
        ComponentsSettings.Add(ogeOptional);
        ComponentsSettings.Add(egeCore);
        ComponentsSettings.Add(egeOptional);

        var honors = new TreeNode("Honors", "g2", "Показатель отличников",
            () => TotalGraduates == 0 ? .0 : (HonorsGraduates / (double)TotalGraduates) * 100 * 5);
        var capacity = new TreeNode("Capacity", "g3", "Показатель переполнения", () => 
            ExcessPercent == 0 ? 100 : 100 - ExcessPercent);
        var profile = new TreeNode("Profile", "g4", "Показатель профильного образования", () => {
            double val = ProfileSeniors / 100 * 3 * AdvancedJuniors;
            return Math.Max(50, Math.Min(100, val));
        });

        gNode.Children.Add(examScores);

        gNode.Children.Add(honors);
        gNode.Children.Add(capacity);
        gNode.Children.Add(profile);
        ComponentsSettings.Add(honors);
        ComponentsSettings.Add(capacity);
        ComponentsSettings.Add(profile);

        // Компонент H (Инновационная деятельность)
        var hNode = new CompositeNode("H", new[] { 1.0 / 3, 1.0 / 3, 1.0 / 3 });
        
        var olympiadSuccess = new TreeNode("OlympiadSuccess", "h1", "Процент победителей ВсОШ (8-11)", () => 
            (VSOHWinners / (double)SeniorStudents) * 100 * 10);
        var digitalClubs = new TreeNode("DigitalClubs", "h2", "Процент вовлеченных в цифровые внеурочные активности", () => DigitalClubs);
        var additionalEdu = new TreeNode("AdditionalEdu", "h3", "Процент вовлеченных в доп образование", () => AdditionalEdu);
        var careerGuidance = new TreeNode("CareerGuidance", "h4", "Процент вовлеченных в проф.ориентационные мероприятия", () => CareerGuidance);
        var projectWork = new TreeNode("ProjectWork", "h5", "Процент вовлеченных в проектную деятельность", () => ProjectWork);
        var arr = new[] { olympiadSuccess, digitalClubs, additionalEdu, careerGuidance, projectWork };
        // H берет три лучших показателя
        foreach (var q in arr)
        {
            hNode.Children.Add(q);
            ComponentsSettings.Add(q);
        }

        // Компонент Y (Когнитивные способности)
        var yNode = new CompositeNode("Y", new[] { 0.33, 0.33, 0.33 });
        
        var memory = new CompositeNode("Memory", new[] { 0.25, 0.25, 0.25, 0.25 });
        var shortTerm = new TreeNode("ShortTermMemory", "y11", "Короткосрочная память", () => ShortTermMemory);
        var procedural = new TreeNode("ProceduralMemory", "y12", "Процессуальная память", () => ProceduralMemory);
        var semantic = new TreeNode("SemanticMemory", "y13", "Семантическая память", () => SemanticMemory);
        var episodic = new TreeNode("EpisodicMemory", "y14", "Эпизодическая память", () => EpisodicMemory);

        memory.Children.Add(shortTerm);
        memory.Children.Add(procedural);
        memory.Children.Add(semantic);
        memory.Children.Add(episodic);
        ComponentsSettings.Add(shortTerm);
        ComponentsSettings.Add(procedural);
        ComponentsSettings.Add(semantic);
        ComponentsSettings.Add(episodic);

        var creativity = new TreeNode("Creativity", "y2", "Креативность", () => Creativity);
        var logic = new TreeNode("Logic", "y3", "Логика", () => Logic);

        yNode.Children.Add(memory);

        yNode.Children.Add(creativity);
        yNode.Children.Add(logic);
        ComponentsSettings.Add(creativity);
        ComponentsSettings.Add(logic);

        _root.Children.Add(fNode);
        _root.Children.Add(gNode);
        _root.Children.Add(hNode);
        _root.Children.Add(yNode);
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

    public List<Recommend> CalcRecommendations(LevelNode targetLevel)
    {
        List<Recommend> recommendations = new List<Recommend>();
        
        List<TreeNode> nodes = [_root.Children[0], _root.Children[1], _root.Children[2]];
        TreeNode nodeY = _root.Children[3];
        switch (targetLevel)
        {
            case LevelNode.Max:
                nodes.Add(nodeY);
                recommendations.AddRange(CalcAll(targetLevel, nodes));
                break;
            case LevelNode.High:
                if (nodeY.CalculatedValue < LevelNode.High.ToValue())
                    recommendations.AddRange(nodeY.GetRecomendations(LevelNode.High.ToValue(), ComponentsSettings));
                recommendations.AddRange(ConditionalCalc(nodes, targetLevel));
                break;                    
            case LevelNode.AboveAverage:
            case LevelNode.Average:
                LevelNode lowerLevel = targetLevel.Lower();
                if (nodeY.CalculatedValue >= targetLevel.ToValue())
                {
                    recommendations.AddRange(ConditionalCalc(nodes, targetLevel));
                }
                else
                {
                    if (nodeY.CalculatedValue < lowerLevel.ToValue())
                    {
                        recommendations.AddRange(nodeY.GetRecomendations(lowerLevel.ToValue(), ComponentsSettings));
                    }
                    recommendations.AddRange(CalcAll(targetLevel, nodes));
                }
                break;
            case LevelNode.BelowAverage:
                recommendations.AddRange(ConditionalCalc(nodes, LevelNode.BelowAverage));
                break;
            case LevelNode.Low:
                break;
        }

        return recommendations;
    }
    private List<Recommend> CalcAll(LevelNode targetScore, List<TreeNode> nodes)
    => nodes.Where(n => n.CalculatedValue < targetScore.ToValue()).SelectMany(n => n.GetRecomendations(targetScore.ToValue(), ComponentsSettings)).ToList();

    private List<Recommend> ConditionalCalc(List<TreeNode> nodes, LevelNode targetLevel)
    {
        LevelNode lowerLevel = targetLevel.Lower();
        List<Recommend> recommendations = new();
        var topNodes = nodes.Where(n => n.CalculatedValue < targetLevel.ToValue()).OrderByDescending(n => n.CalculatedValue);
        var aboveAvg = nodes.Count(n => n.CalculatedValue >= lowerLevel.ToValue());
        var avg = nodes.Count(n => n.CalculatedValue < lowerLevel.ToValue());

        if (!(aboveAvg <= 1 && avg == 0))
        {
            foreach (var node in topNodes.Take(aboveAvg + avg - 1))
            {
                recommendations.AddRange(node.GetRecomendations(targetLevel.ToValue(), ComponentsSettings));
            }
            if (topNodes.Last().CalculatedValue < lowerLevel.ToValue())
                recommendations.AddRange(topNodes.Last().GetRecomendations(lowerLevel.ToValue(), ComponentsSettings));
        }
        return recommendations;
    }

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
            if (val < 20) countFghBelowAvg++;
            if (val < 40) countFghAvg++;
            if (val < 60) countFghAboveAvg++;
            if (val < 80) countFghHigh++;
        }

        if (yHigh && countFghHigh <= 1 && countFghAboveAvg == 0)
            return "Высокий";
        if ((yAboveAvg && countFghAboveAvg <= 1 && countFghAvg == 0) || (yAvg && countFghAboveAvg == 0))
            return "Выше среднего";
        if ((yAvg && countFghAvg <= 1 && countFghBelowAvg == 0) || (yBelowAvg && countFghAvg == 0))
            return "Средний";
        if (countFghBelowAvg <= 1)
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
        if (_errors.TryGetValue(propertyName, out List<string>? errors))
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

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
