using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoodwink
{
    public class QuizizzStats
    {
        public int played { get; set; }
        public int totalPlayers { get; set; }
        public int totalCorrect { get; set; }
        public int totalQuestions { get; set; }
    }

    public class CreatorPrivileges
    {
        public int gameLimit { get; set; }
        public int playerLimit { get; set; }
        public bool adsFree { get; set; }
        public bool asyncGames { get; set; }
        public bool infiniteGameExpiry { get; set; }
        public bool branding { get; set; }
        public bool xlsExportEmail { get; set; }
        public bool tagQuestionTopics { get; set; }
        public bool analyticsAndReports { get; set; }
        public bool reopenGames { get; set; }
        public bool audio { get; set; }
        public bool video { get; set; }
        public bool editPublicQuiz { get; set; }
        public bool editPublicPresentation { get; set; }
        public bool themes { get; set; }
        public bool answerExplanation { get; set; }
    }

    public class Creator
    {
        public string occupation { get; set; }
    }

    public class QuizizzInfo
    {
        public string name { get; set; }
        public List<string> subjects { get; set; }
        public string image { get; set; }
        public List<string> grade { get; set; }
        public QuizizzStats stats { get; set; }
        public string _id { get; set; }
        public CreatorPrivileges creatorPrivileges { get; set; }
        public Creator creator { get; set; }
    }
}
