using System.Text;

namespace Hoodwink
{
    //useless for now
    class QuizizzEncoder
    {
        public static string extractHeader(string str)
        {
            try
            {
                char c = (char)(str[str.Length - 2] - 33);
                return str.Substring(0, c);
            }
            catch
            {
                return str;
            }
        }
        public static string extractData(string str)
        {
            char c = (char)(str[str.Length - 2] - 33);
            return str.Substring(c, str.Length -(c +2));
        }
        private static char safeAdd(int t, int e)
        {
            int o = t + e;
            return o > 65535 ? (char)(o - 65535 + 0 - 1) : o < 0 ? (char)(65535 - (0 - o) + 1) : (char)(o);
        }
        public static int extractVersion(string str)
        {
            int.TryParse(str[str.Length - 1].ToString(), out int res);
            return res;
        }
        private static bool verifyCharCode(int t)
        {
            return !(t >= 55296 && t <= 56319 || t >= 56320 && t <= 57343);

        }
        private static char addOffset(int t, int e, int o, int s)
        {
            return 2 == s ? verifyCharCode(t) ? (char)(safeAdd(t, o % 2 == 0 ? e : -e)) : (char)(t) : (char)(safeAdd(t, o % 2 == 0 ? e : -e));

        }
        public static string decodeRaw(string t, bool e,string oz = "quizizz.com")
        {
            int s = extractVersion(t);
            int r = 0;
            r = e ? oz[0] : oz[0] + oz[oz.Length - 1];
            r = -r;
            StringBuilder n = new StringBuilder();
            for (int o = 0; o < t.Length; o++)
            {
                char c = t[o];
                var a = e ? safeAdd(c, r) : addOffset(c, r, o, s);
                n.Append((char)a);

            }
            return n.ToString();

        }
        public static string decode(string t, bool e = false)
        {
            if (e)
            {
                var ez = extractHeader(t);
                return decodeRaw(ez, true);

            }
            else {
                var ez = decode(extractHeader(t), true);
                var  o = extractData(t);
                return decodeRaw(o, false, ez);
            }
        }
    }
}
