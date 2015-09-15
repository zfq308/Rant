using System;
using System.Collections.Generic;
using System.Text;

namespace Rant.Vocabulary.Lingusitics.German
{
    internal static class GermanAdjectiveInflector
    {
        private static readonly Dictionary<string, string> specialComparatives = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"gut", "besser"},
            {"hoch", "höher" }
        };

        private static readonly Dictionary<string, string> specialSuperlatives = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"gut", "best"}
        };

        private static readonly HashSet<string> umlautWords =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "alt", "arm", "dumm", "gesund", "grob", "groß", "hart",
                "hoch", "jung", "kalt", "klug", "kurz", "lang", "oft",
                "scharf", "schwach", "schwarz", "stark", "warm"
            };

        public static string Inflect(
            string adjBase,
            Gender gender,
            InflectionType type,
            GrammaticalCase fall,
            Comparison comparison)
        {

            return adjBase;
        }

        private static string GetComparisonForm(string adjBase, Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.Comparative:
                    {
                        string result;
                        if (!specialComparatives.TryGetValue(adjBase, out result))
                        {
                            result = Fuse(adjBase, "er");
                        }
                        return result;
                    }
                case Comparison.Superlative:
                    {
                        string result;
                        if (!specialSuperlatives.TryGetValue(adjBase, out result))
                        {
                            result = Fuse(adjBase, "st");
                        }
                        return result;
                    }
                default:
                    return adjBase;
            }
        }

        private static string Fuse(string a, string b)
        {
            int i = 0;
            while (i < b.Length && (a.Length - (i + 1)) >= 0 && a[a.Length - (i + 1)] == b[i])
            {
                i++;
            }
            return a.Substring(0, a.Length - i + 1) + b.Substring(i);
        }

        private static string AddUmlaut(string word)
        {
            var sb = new StringBuilder();
            bool umlauted = false;
            foreach (var c in word)
            {
                if (umlauted)
                {
                    sb.Append(c);
                }
                else
                {
                    umlauted = true;
                    switch (c)
                    {
                        case 'A':
                            sb.Append('Ä');
                            break;
                        case 'a':
                            sb.Append('ä');
                            break;
                        case 'O':
                            sb.Append('Ö');
                            break;
                        case 'o':
                            sb.Append('ö');
                            break;
                        case 'U':
                            sb.Append('Ü');
                            break;
                        case 'u':
                            sb.Append('ü');
                            break;
                        default:
                            umlauted = false;
                            sb.Append(c);
                            break;
                    }
                }
            }
            return sb.ToString();
        }
    }
}