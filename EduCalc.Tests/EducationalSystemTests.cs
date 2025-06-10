using NUnit.Framework;
using System;
using EduCalc.Models;

namespace EduCalc.Tests
{
    [TestFixture]
    public class EducationalSystemTests
    {
        [Test]
        public void CalculateF_ReturnsCorrectValue()
        {
            var system = new EducationalSystem
            {
                TotalArea = 17666,
                StudentCount = 1226,
                ComputerCount = 228,
                BookCount = 32878,

                TeachersWithHigherEdu = 75.41,
                CertifiedTeachers = 44.26,
                JuniorTeachers = 19,
                MidCareerTeachers = 23,
                SeniorTeachers = 19
            };
            
            TreeNode F = system.Root.Children.First(c => c.Name == "F");

            LevelNode target = LevelNode.High;

            List<Recommend> recommends = F.GetRecomendations(target.ToValue());


            Assert.That(system.F, Is.EqualTo(66.57).Within(0.01));
            Assert.That(recommends.Sum(r => r.Inc * r.Coef) + F.CalculatedValue, Is.EqualTo(target.ToValue()).Within(0.01));

        }

        [Test]
        public void CalculateG_ReturnsCorrectValue()
        {
            var system = new EducationalSystem
            {
                OGECoreAvg = 54.4,
                OGEOptionalAvg = 58.5,
                EGECoreAvg = 67.2,
                EGEOptionalAvg = 56.8,
                HonorsGraduates = 4,
                TotalGraduates = 101,
                ExcessPercent = 0,
                ProfileSeniors = 100,
                AdvancedJuniors = 0
            };
            
            TreeNode G = system.Root.Children.First(c => c.Name == "G");
            LevelNode target = LevelNode.AboveAverage;
            List<Recommend> recommends = G.GetRecomendations(target.ToValue());

            Assert.That(system.G, Is.EqualTo(57.65).Within(0.01));
            Assert.That(recommends.Sum(r => r.Inc * r.Coef) + G.CalculatedValue, Is.EqualTo(target.ToValue()).Within(0.01));
        }

        [Test]
        public void CalculateH_ReturnsCorrectValue()
        {
            var system = new EducationalSystem
            {
                VSOHWinners = 4,
                SeniorStudents = 250,
                DigitalClubs = 1.3,
                AdditionalEdu = 100,
                CareerGuidance = 89.83,
                ProjectWork = 4.99
            };
            
            TreeNode H = system.Root.Children.First(c => c.Name == "H");
            LevelNode target = LevelNode.High;
            List<Recommend> recommends = H.GetRecomendations(target.ToValue());

            Assert.That(system.H, Is.EqualTo(68.61).Within(0.01));
            Assert.That(recommends.Sum(r => r.Inc * r.Coef) + H.CalculatedValue, Is.EqualTo(target.ToValue()).Within(0.01));
        }

        [Test]
        public void CalculateY_ReturnsCorrectValue()
        {
            //old
            var system = new EducationalSystem
            {
                ShortTermMemory = 45.56,
                ProceduralMemory = 54.8,
                SemanticMemory = 48.68,
                EpisodicMemory = 49.86,
                Creativity = 66.87,
                Logic = 65.9
            };
            
            TreeNode Y = system.Root.Children.First(c => c.Name == "Y");
            LevelNode target = LevelNode.High;
            List<Recommend> recommends = Y.GetRecomendations(target.ToValue());
            
            Assert.That(system.Y, Is.EqualTo(60.23).Within(0.01));
            Assert.That(recommends.Sum(r => r.Inc * r.Coef) + Y.CalculatedValue, Is.EqualTo(target.ToValue()).Within(0.01));
        }
        [Test]
        public void CalculateY_ReturnsCorrectValue2()
        {
            //old
            var system = new EducationalSystem
            {
                ShortTermMemory = 45.56,
                ProceduralMemory = 54.8,
                SemanticMemory = 48.68,
                EpisodicMemory = 49.86,
                Creativity = 66.87,
                Logic = 96.0
            };
            
            TreeNode Y = system.Root.Children.First(c => c.Name == "Y");
            LevelNode target = LevelNode.High;
            List<Recommend> recommends = Y.GetRecomendations(target.ToValue());

            Assert.That(system.Y, Is.EqualTo(70.15).Within(0.01));
            Assert.That(recommends.Sum(r => r.Inc * r.Coef) + Y.CalculatedValue, Is.EqualTo(target.ToValue()).Within(0.01));
        }

        [Test]
        public void DetermineS_ReturnsCorrectLevel()
        {
            var system = new EducationalSystem
            {
                ShortTermMemory = 45.56,
                ProceduralMemory = 54.8,
                SemanticMemory = 48.68,
                EpisodicMemory = 49.86,
                Creativity = 66.87,
                Logic = 65.9,

                TotalArea = 17666,
                StudentCount = 1226,
                ComputerCount = 228,
                BookCount = 32878,
                TeachersWithHigherEdu = 75.41,
                CertifiedTeachers = 44.26,
                JuniorTeachers = 19,
                MidCareerTeachers = 23,
                SeniorTeachers = 19,

                OGECoreAvg = 54.4,
                OGEOptionalAvg = 58.5,
                EGECoreAvg = 67.2,
                EGEOptionalAvg = 56.8,
                HonorsGraduates = 4,
                TotalGraduates = 101,
                ExcessPercent = 0,
                ProfileSeniors = 100,
                AdvancedJuniors = 0,

                VSOHWinners = 4,
                SeniorStudents = 250,
                DigitalClubs = 1.3,
                AdditionalEdu = 100,
                CareerGuidance = 89.83,
                ProjectWork = 4.99
            };

            Assert.That(system.S, Is.EqualTo("Выше среднего"));

            //Assert.That(system.CalcRecommendations(LevelNode.High).Sum(r => r.Inc * r.Coef), Is.EqualTo(LevelNode.High.ToValue()).Within(0.01));
        }

        [Test]
        public void ValidateNegativeTotalArea_ThrowsError()
        {
            var system = new EducationalSystem { TotalArea = -100 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.TotalArea)),
                       Contains.Item("Площадь не может быть отрицательной"));
        }

        [Test]
        public void ValidateZeroStudentCount_ThrowsError()
        {
            var system = new EducationalSystem { StudentCount = 0 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.StudentCount)),
                       Contains.Item("Количество учеников должно быть больше нуля"));
        }

        [Test]
        public void ValidatePercentageOutOfRange_ThrowsError()
        {
            var system = new EducationalSystem { TeachersWithHigherEdu = 101 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.TeachersWithHigherEdu)),
                       Contains.Item("Процент должен быть от 0 до 100"));
        }
    }
}