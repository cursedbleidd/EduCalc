using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCalc.Models
{
    public static class LevelNodeExtensions
    {
        public static string ToDescription(this LevelNode level) => level switch
        {
            LevelNode.Low => "Низкий",
            LevelNode.BelowAverage => "Ниже среднего",
            LevelNode.Average => "Средний",
            LevelNode.AboveAverage => "Выше среднего",
            LevelNode.High => "Высокий",
            LevelNode.Max => "Максимальный",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        public static double ToValue(this LevelNode level) => (double)level;
        public static LevelNode Lower(this LevelNode level) => level switch
        {
            LevelNode.BelowAverage => LevelNode.Low,
            LevelNode.Average => LevelNode.BelowAverage,
            LevelNode.AboveAverage => LevelNode.Average,
            LevelNode.High => LevelNode.AboveAverage,
            LevelNode.Max => LevelNode.High,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
        public static LevelNode FromString(string level) => level switch
        {
            "Низкий" => LevelNode.Low,
            "Ниже среднего" => LevelNode.BelowAverage,
            "Средний" => LevelNode.Average,
            "Выше среднего" => LevelNode.AboveAverage,
            "Высокий" => LevelNode.High,
            "Максимальный" => LevelNode.Max,
            _ => throw new ArgumentException("Неизвестный уровень образования.", nameof(level))
        };
    }
}
