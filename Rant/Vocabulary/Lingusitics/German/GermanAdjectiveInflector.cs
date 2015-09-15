using System;
using System.Collections.Generic;
using System.Text;

namespace Rant.Vocabulary.Lingusitics.German
{
    internal static class GermanAdjectiveInflector
    {
        public static string Inflect(
            string adjBase,
            Gender gender,
            InflectionType type,
            GrammaticalCase fall,
            bool plural)
        {
            if (plural)
            {
                if (type == InflectionType.Strong &&
                    (fall == GrammaticalCase.Nominative || fall == GrammaticalCase.Accusative))
                {
                    return Fuse(adjBase, "e");
                }
                return Fuse(adjBase, "en");
            }

            switch (type)
            {
                case InflectionType.Strong:
                    switch (fall)
                    {
                        case GrammaticalCase.Nominative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "er");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "e");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "es");
                            }
                            break;
                        case GrammaticalCase.Accusative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "en");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "e");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "es");
                            }
                            break;
                        case GrammaticalCase.Dative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "em");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "er");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "em");
                            }
                            break;
                        case GrammaticalCase.Genitive:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "en");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "er");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "en");
                            }
                            break;
                    }
                    break;
                case InflectionType.Mixed:
                    switch (fall)
                    {
                        case GrammaticalCase.Nominative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "er");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "e");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "es");
                            }
                            break;
                        case GrammaticalCase.Accusative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "en");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "e");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "es");
                            }
                            break;
                        case GrammaticalCase.Dative:
                        case GrammaticalCase.Genitive:
                            return Fuse(adjBase, "en");
                    }
                    break;
                case InflectionType.Weak:
                    switch (fall)
                    {
                        case GrammaticalCase.Nominative:
                            return Fuse(adjBase, "e");
                        case GrammaticalCase.Accusative:
                            switch (gender)
                            {
                                case Gender.Masculine:
                                    return Fuse(adjBase, "en");
                                case Gender.Feminine:
                                    return Fuse(adjBase, "e");
                                case Gender.Neuter:
                                    return Fuse(adjBase, "e");
                            }
                            break;
                        case GrammaticalCase.Dative:
                        case GrammaticalCase.Genitive:
                            return Fuse(adjBase, "en");
                    }
                    break;
            }
            return adjBase;
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