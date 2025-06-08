using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCalc.Models
{
    public static class LevelNodeExtensions
    {
        public static string ToDescription(this LevelNode level)
        {
            return level switch
            {
                LevelNode.Low => "Низкий",
                LevelNode.BelowAverage => "Ниже среднего",
                LevelNode.Average => "Средний",
                LevelNode.AboveAverage => "Выше среднего",
                LevelNode.High => "Высокий",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }

        public static double ToValue(this LevelNode level)
        {
            return (double)level;
        }
        public static LevelNode FromString(string level)
        {
            return level switch
            {
                "Низкий" => LevelNode.Low,
                "Ниже среднего" => LevelNode.BelowAverage,
                "Средний" => LevelNode.Average,
                "Выше среднего" => LevelNode.AboveAverage,
                "Высокий" => LevelNode.High,
                _ => throw new ArgumentException("Неизвестный уровень образования.", nameof(level))
            };
        }
    }
}
