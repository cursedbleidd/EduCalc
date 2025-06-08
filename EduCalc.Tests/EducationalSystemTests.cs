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
            //new
            var F = system.Root.Children.First(c => c.Name == "F");

            var hold = F.GetRecomendations(80);

            //old
            Assert.That(system.F, Is.EqualTo(66.57).Within(0.01));
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
            //new
            var G = system.Root.Children.First(c => c.Name == "G");

            var hold = G.GetRecomendations(60);

            //old
            Assert.That(system.G, Is.EqualTo(57.65).Within(0.01));
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
            //new
            var H = system.Root.Children.First(c => c.Name == "H");

            var hold = H.GetRecomendations(80);

            //old
            Assert.That(system.H, Is.EqualTo(68.61).Within(0.01));
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
            //new
            var Y = system.Root.Children.First(c => c.Name == "Y");

            var hold = Y.GetRecomendations(80);

            //old
            Assert.That(system.Y, Is.EqualTo(60.23).Within(0.01));
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
            //new
            var Y = system.Root.Children.First(c => c.Name == "Y");

            var hold = Y.GetRecomendations(80);

            var arr = hold.ToArray();

            var sum = 0.33 * (0.25 * (arr[0].Inc + arr[0].Value + arr[1].Inc + arr[1].Value + arr[2].Inc + arr[2].Value + arr[3].Inc + arr[3].Value) + arr[4].Inc + arr[4].Value + arr[5].Inc + arr[5].Value);
            //old
            Assert.That(system.Y, Is.EqualTo(70.15).Within(0.01));
        }

        [Test]
        public void DetermineS_ReturnsCorrectLevel()
        {
            var system = new EducationalSystem
            {
                // ��������� �������� �� ������� ������
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

            Assert.That(system.S, Is.EqualTo("���� ��������"));
        }

        [Test]
        public void ValidateNegativeTotalArea_ThrowsError()
        {
            var system = new EducationalSystem { TotalArea = -100 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.TotalArea)),
                       Contains.Item("������� �� ����� ���� �������������"));
        }

        [Test]
        public void ValidateZeroStudentCount_ThrowsError()
        {
            var system = new EducationalSystem { StudentCount = 0 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.StudentCount)),
                       Contains.Item("���������� �������� ������ ���� ������ ����"));
        }

        [Test]
        public void ValidatePercentageOutOfRange_ThrowsError()
        {
            var system = new EducationalSystem { TeachersWithHigherEdu = 101 };
            Assert.That(system.GetErrors(nameof(EducationalSystem.TeachersWithHigherEdu)),
                       Contains.Item("������� ������ ���� �� 0 �� 100"));
        }
    }
}