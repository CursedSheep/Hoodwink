using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoodwink
{
    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
    }

    public class Version
    {
        public string type { get; set; }
        public int version { get; set; }
    }

    public class Options
    {
        public string memeset { get; set; }
        public bool studentLeaderboard { get; set; }
        public bool timer { get; set; }
        public string timer_3 { get; set; }
        public bool jumble { get; set; }
        public bool jumbleAnswers { get; set; }
        public bool memes { get; set; }
        public string showAnswers_2 { get; set; }
        public string studentQuizReview_2 { get; set; }
        public bool showAnswers { get; set; }
        public bool studentQuizReview { get; set; }
        public int limitAttempts { get; set; }
        public bool studentMusic { get; set; }
        public string redemption { get; set; }
        public string powerups { get; set; }
        public bool nicknameGenerator { get; set; }
        public bool loginRequired { get; set; }
        public int questionsPerAttempt { get; set; }
    }

    public class Create
    {
    }

    public class AssignedTo
    {
    }

    public class Data
    {
        public object title { get; set; }
        public object description { get; set; }
    }

    public class GroupsInfo
    {
        public string mode { get; set; }
        public Create create { get; set; }
        public List<object> gcl { get; set; }
        public List<object> assigned { get; set; }
        public bool hasGCL { get; set; }
        public AssignedTo assignedTo { get; set; }
        public Data data { get; set; }
    }

    public class Subscription
    {
        public int playerLimit { get; set; }
        public object trialEndAt { get; set; }
        public bool adsFree { get; set; }
        public bool branding { get; set; }
    }

    public class VersionTraits
    {
        public bool isQuizWithoutCorrectAnswer { get; set; }
        public int totalSlides { get; set; }
    }

    public class Traits
    {
        public bool isQuizWithoutCorrectAnswer { get; set; }
        public VersionTraits versionTraits { get; set; }
    }

    public class Room
    {
        public string hash { get; set; }
        public string type { get; set; }
        public double expiry { get; set; }
        public long createdAt { get; set; }
        public List<Version> version { get; set; }
        public string code { get; set; }
        public object assignments { get; set; }
        public bool deleted { get; set; }
        public string experiment { get; set; }
        public string hostId { get; set; }
        public string hostSessionId { get; set; }
        public string hostOccupation { get; set; }
        public Options options { get; set; }
        public List<string> questions { get; set; }
        public string quizId { get; set; }
        public List<object> groupIds { get; set; }
        public GroupsInfo groupsInfo { get; set; }
        public object startedAt { get; set; }
        public string state { get; set; }
        public int totalCorrect { get; set; }
        public int totalPlayers { get; set; }
        public int totalQuestions { get; set; }
        public object assignmentTitle { get; set; }
        public string versionId { get; set; }
        public object collectionId { get; set; }
        public object unitId { get; set; }
        public object replayOf { get; set; }
        public object courseId { get; set; }
        public bool reopenable { get; set; }
        public bool reopened { get; set; }
        public object soloApis { get; set; }
        public Subscription subscription { get; set; }
        public bool simGame { get; set; }
        public int totalAnswerableQuestions { get; set; }
        public Traits traits { get; set; }
        public string organization { get; set; }
        public bool isShared { get; set; }
    }

    public class Player
    {
        public bool isAllowed { get; set; }
        public bool loginRequired { get; set; }
        public List<object> attempts { get; set; }
    }

    public class RoomData
    {
        public object __cid__ { get; set; }
        public Error error { get; set; }
        public Room room { get; set; }
        public Player player { get; set; }
    }


}
