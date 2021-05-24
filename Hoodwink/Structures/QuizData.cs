using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoodwink
{
    public class Cours
    {
        public string displayName { get; set; }
        public string internalName { get; set; }
        public string _id { get; set; }
    }

    public class Info
    {
        public List<string> grade { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<Cours> courses { get; set; }
    }

    public class Stats
    {
        public int played { get; set; }
        public int totalPlayers { get; set; }
        public int totalCorrect { get; set; }
        public int totalQuestions { get; set; }
    }

    public class Quizzes
    {
        public Info info { get; set; }
        public string _id { get; set; }
        public Stats stats { get; set; }
    }

    public class BasicQuizInfo
    {
        public List<Quizzes> quizzes { get; set; }
        public int timeTaken { get; set; }
        public string experiment { get; set; }
    }

    public class Meta
    {
        public string service { get; set; }
        public string version { get; set; }
    }

    public class QuizData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public BasicQuizInfo data { get; set; }
        public Meta meta { get; set; }
    }
}
