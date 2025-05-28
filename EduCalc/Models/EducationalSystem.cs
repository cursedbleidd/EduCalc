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

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string propertyName)
    {
        if (_errors.TryGetValue(propertyName, out List<string> errors) && errors != null)
            return errors;
        return Enumerable.Empty<string>();
        
    }

    public void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName)) _errors[propertyName] = new List<string>();
        if (!_errors[propertyName].Contains(error)) _errors[propertyName].Add(error);
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    // Материальная база (f1)
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
                AddError(nameof(BookCount), "Количество книг не может быть отрицательным");
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
    public double F => CalculateF();
    public double G => CalculateG();
    public double H => CalculateH();
    public double Y => CalculateY();
    public string S => DetermineS();

    private double CalculateF() 
    {
        // f11 = (TotalArea / StudentCount) * 4
        double f11 = (TotalArea / StudentCount) * 4;

        // f12 = (ComputerCount / StudentCount) * 300
        double f12 = (ComputerCount / (double)StudentCount) * 300;

        // f13 = (BookCount / StudentCount) * 100 (если >26 → 100)
        double f13 = (BookCount / (double)StudentCount) * 100;
        if (f13 > 100) f13 = 100;

        // f1 = 0.33*f11 + 0.33*f12 + 0.33*f13
        double f1 = 0.33 * f11 + 0.33 * f12 + 0.33 * f13;

        // f23 = 100 - ((n2 - n1)/TotalTeachers)*100 - ((n3 - n2)/TotalTeachers)*100
        double totalTeachers = JuniorTeachers + MidCareerTeachers + SeniorTeachers;
        double f23 = 100 - (Math.Abs(MidCareerTeachers - JuniorTeachers) / totalTeachers) * 100
                     - (Math.Abs(MidCareerTeachers - SeniorTeachers) / totalTeachers) * 100;

        // f2 = 0.25*f21 + 0.5*f22 + 0.25*f23
        double f2 = 0.25 * TeachersWithHigherEdu + 0.5 * CertifiedTeachers + 0.25 * f23;

        // F = 0.5*f1 + 0.5*f2
        return 0.5 * f1 + 0.5 * f2;
    }

    private double CalculateG()
    {
        // g1 = 0.25*(OGECoreAvg + OGEOptionalAvg + EGECoreAvg + EGEOptionalAvg)
        double g1 = 0.25 * (OGECoreAvg + OGEOptionalAvg + EGECoreAvg + EGEOptionalAvg);

        // g2 = (HonorsGraduates / TotalGraduates) * 100 * 5
        double g2 = (HonorsGraduates / (double)TotalGraduates) * 100 * 5;

        // g3 = 100 - ExcessPercent
        double g3 = ExcessPercent == 0 ? 100 : 100 - ExcessPercent;

        // g4 = sqrt(ProfileSeniors * AdvancedJuniors)
        double g4 = ProfileSeniors / 100 * 3 * AdvancedJuniors;
        if (g4 < 50)
            g4 = 50;
        else if (g4 > 100)
            g4 = 100;

        // G = 0.4*g1 + 0.2*g2 + 0.2*g3 + 0.2*g4
        return 0.4 * g1 + 0.2 * g2 + 0.2 * g3 + 0.2 * g4;
    }

    private double CalculateH()
    {
        // h1 = (VSOHWinners / SeniorStudents) * 100 * 10
        double h1 = (VSOHWinners / (double)SeniorStudents) * 100 * 10;

        // h2 = DigitalClubs, h3 = AdditionalEdu, h4 = CareerGuidance, h5 = ProjectWork
        double[] hValues = { h1, DigitalClubs, AdditionalEdu, CareerGuidance, ProjectWork };
        Array.Sort(hValues);
        Array.Reverse(hValues);
        return (hValues[0] + hValues[1] + hValues[2]) / 3;
    }

    private double CalculateY()
    {
        // y1 = 0.25*(ShortTermMemory + ProceduralMemory + SemanticMemory + EpisodicMemory)
        double y1 = 0.25 * (ShortTermMemory + ProceduralMemory + SemanticMemory + EpisodicMemory);
        double y2 = Creativity;
        double y3 = Logic;
        return 0.33 * y1 + 0.33 * y2 + 0.33 * y3;
    }

    private string DetermineS()
    {
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

        // Правила из таблицы 1 статьи
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

    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
