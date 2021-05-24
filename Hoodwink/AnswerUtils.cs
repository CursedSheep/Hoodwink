using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using HtmlAgilityPack;

namespace Hoodwink
{
    //GetAnswers
    class AnswerUtils
    {
        public static JObject getAnswerData(RoomData roomHash, QuizizzInfo quizizzInfo)
        {
            var searchArgs = new SearchArgs();
            var qRange = new QuestionsRange();
            searchArgs.occupation = new List<string>
                    {
                        "teacher",
                        "teacher_school",
                        "teacher_university",
                        "other",
                        "student"
                    };
            searchArgs.SubjectsAggs = quizizzInfo.subjects;
            searchArgs.grade = quizizzInfo.grade;
            qRange.numberOfQuestions = new List<int> { roomHash.room.questions.Count, roomHash.room.questions.Count };
            quizizzInfo.name = HttpUtility.UrlEncode(Regex.Replace(quizizzInfo.name, "[^\\p{L}\\p{N}]+", " "));
            var QuestionsData = AnswerUtils.GetSetData<Newtonsoft.Json.Linq.JObject>(string.Concat(new string[]
            {
                        "https://quizizz.com/api/main/search?sortKey=_score&filterList=",
                        JsonConvert.SerializeObject(searchArgs),
                        "&rangeList=",
                        JsonConvert.SerializeObject(qRange),
                        "&page=SearchPage&from=0&query=",
                        quizizzInfo.name,
                        "&size=40"
            }));
            return QuestionsData;
        }
        public static List<QuizItem> GetAnswersLive2(Structures.QuizizzInfo2.Root questionData)
        {
            List<QuizItem> items = new List<QuizItem>();
            foreach(var q in questionData.questions)
            {
                QuizItem n1 = new QuizItem();
                n1.Question = q.structure.query.text;
                n1.Type = q.type;
                List<string> answers = new List<string>();
                switch (q.type)
                {
                    case "MCQ":
                        {
                            var answerIndex = (int)(long)q.structure.answer;
                            if (answerIndex == -1) continue;
                            string ans = q.structure.options[answerIndex].text;
                            if (ans == "")
                                ans = q.structure.options[answerIndex].media[0].ToString();

                            answers.Add(ans);
                        }
                        break;
                    case "MSQ":
                        {
                            var AnswerIndices = (int[])((JArray)q.structure.answer).ToObject(typeof(int[]));
                            foreach (var i in AnswerIndices)
                            {
                                string ans = q.structure.options[i].text;
                                if (ans == "")
                                    ans = q.structure.options[i].media[0].ToString();

                                answers.Add(ans);
                            }
                        }
                        break;
                    case "BLANK":
                        {
                            foreach(var ans in q.structure.options)
                            {
                                if (ans.media.Count > 0)
                                    answers.Add(ans.media[0].ToString());
                                else
                                    answers.Add(ans.text);
                            }
                        }
                        break;
                }
                n1.Answers = answers.ToArray();

                if (q.structure.query.media.Count > 0)
                    n1.ImageUrl = q.structure.query.media[0].ToString();
                else
                    n1.ImageUrl = "";

                items.Add(n1);
            }
            return items;
        }
        //
        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]

        public static List<QuizItem> GetAnswersLive(JObject questiondata, string id)
        {
            List<QuizItem> quizItems = new List<QuizItem>();
            var r = questiondata["data"]["hits"] as JArray;
            foreach(var item in r)
            {
                var q = item["info"]["questions"];
                if(q[0]["_id"].ToString() == id)
                {
                    foreach (var jsonquestiondata in q)
                    {
                        //var p = typeof(JProperty).GetRuntimeProperties();
                        //var getChildren = p.First(x => x.Name == "ChildrenTokens").GetValue(jsonquestiondata) as IList<JToken>;
                        var questions = jsonquestiondata; /*getChildren.ElementAt(0)*/
                        QuizItem qi = new QuizItem();
                        List<string> answers = new List<string>();
                        var atasf = questions["structure"]["query"]["text"].ToString();
                        qi.Question = atasf;
                        var type = questions["type"].ToString();
                        switch (type)
                        {
                            case "BLANK":
                                {
                                    foreach (var ans in questions["structure"]["options"])
                                    {
                                        answers.Add(ans["text"].ToString());
                                    }
                                }
                                break;
                            case "MSQ":
                                {
                                    string decoded = questions["structure"]["answer"].ToString();
                                    var answerIndex = JsonConvert.DeserializeObject<List<int>>(decoded);
                                    foreach (var i in answerIndex)
                                    {
                                        string ans = questions["structure"]["options"][i]["text"].ToString();
                                        if (ans == "")
                                            ans = questions["structure"]["options"][i]["media"][0]["url"].ToString();
                                        answers.Add(ans);
                                    }
                                }
                                break;
                            case "MCQ":
                                {
                                    string decoded = questions["structure"]["answer"].ToString();
                                    int answerIndex = Convert.ToInt32(decoded);
                                    if (answerIndex == -1) continue;
                                    string ans = questions["structure"]["options"][answerIndex]["text"].ToString();
                                    if (ans == "")
                                        ans = questions["structure"]["options"][answerIndex]["media"][0]["url"].ToString();
                                    answers.Add(ans);
                                }
                                break;
                        }
                        qi.Answers = answers.ToArray();
                        qi.Type = type;
                        qi.ImageUrl = TryGetMediaUrl(questions);
                        qi.ID = questions["_id"].ToString();
                        quizItems.Add(qi);
                    }
                    return quizItems;
                }
            }
            return null;
        }

        public static T GetSetData<T>(string link)
        {
            try
            {
                using (WebClient wb = new WebClient())
                {
                    string urlz = link;
                    string str = Encoding.UTF8.GetString(wb.DownloadData(urlz));
                    var dsd = JsonConvert.DeserializeObject<T>(str);
                    return dsd;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(T); 
        }
        public static async Task<RoomData> GetRoomData(string LocalStorage)
        {
            JObject str1 = JsonConvert.DeserializeObject<JObject>(LocalStorage);
            object roomCode = str1["game"]["roomCode"];
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
                { "roomCode", roomCode.ToString() }
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://game.quizizz.com/play-api/v4/checkRoom", content);

                var responseString = await response.Content.ReadAsStringAsync();
                var des2 = JsonConvert.DeserializeObject<RoomData>(responseString);
                return des2;
            }
        }
        private static string TryGetMediaUrl(JToken j)
        {
            try
            {
                return j["structure"]["query"]["media"][0]["url"].ToString();
            }
            catch
            {
                return "";
            }
        }
        public static string ConvertToPlainText(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }  
        public static int CountWords(string plainText)
        {
            return !String.IsNullOrEmpty(plainText) ? plainText.Split(' ', '\n').Length : 0;
        }


        public static string Cut(string text, int length)
        {
            if (!String.IsNullOrEmpty(text) && text.Length > length)
            {
                text = text.Substring(0, length - 4) + " ...";
            }
            return text;
        }


        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                        case "br":
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }
    }
}
