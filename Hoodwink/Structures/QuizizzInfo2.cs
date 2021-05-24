using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoodwink.Structures
{
    class QuizizzInfo2
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Math
        {
            public List<object> latex { get; set; }
            public object template { get; set; }
        }

        public class Explain
        {
            public Math math { get; set; }
            public string type { get; set; }
            public bool hasMath { get; set; }
            public List<object> media { get; set; }
            public string text { get; set; }
        }

        public class Option
        {
            public Math math { get; set; }
            public string type { get; set; }
            public bool hasMath { get; set; }
            public List<object> media { get; set; }
            public string text { get; set; }
            public object matcher { get; set; }
        }

        public class Query
        {
            public Math math { get; set; }
            public object type { get; set; }
            public bool hasMath { get; set; }
            public List<object> media { get; set; }
            public string text { get; set; }
        }

        public class Structure
        {
            public Explain explain { get; set; }
            public List<Option> options { get; set; }
            public Query query { get; set; }
            public object answer { get; set; }
            public bool hasMath { get; set; }
        }

        public class Question
        {
            public string type { get; set; }
            public Structure structure { get; set; }
        }

        public class Root
        {
            public string quizId { get; set; }
            public List<Question> questions { get; set; }
        }
    }
}
