using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoodwink
{
    public class SearchArgs
    {
        [JsonProperty("grade_type.aggs")]
        public object GradeTypeAggs { get; set; }
        public List<string> occupation { get; set; }
        public object cloned { get; set; }
        [JsonProperty("subjects.aggs")]
        public List<string> SubjectsAggs { get; set; }
        [JsonProperty("lang.aggs")]
        public object LangAggs { get; set; }
        public List<string> grade { get; set; }
        public object isProfane { get; set; }
    }

}
