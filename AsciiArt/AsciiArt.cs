using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsciiArt
{
    /// <summary>
    /// ASCII sanat (banner) üretimi için yardımcı sınıf.
    /// Desteklenen karakterler: A-Z, a-z ve boşluk. Türkçe karakterler Latin karşılıklarına dönüştürülür.
    /// </summary>
    public static class AsciiArt
    {
        private const int Height = 8;
        private const int SpaceWidth = 5;

        private static readonly Dictionary<char, char> TurkishMap = new()
        {
            ['ç'] = 'c',
            ['Ç'] = 'C',
            ['ğ'] = 'g',
            ['Ğ'] = 'G',
            ['ı'] = 'i',
            ['İ'] = 'I',
            ['ö'] = 'o',
            ['Ö'] = 'O',
            ['ş'] = 's',
            ['Ş'] = 'S',
            ['ü'] = 'u',
            ['Ü'] = 'U'
        };

        // Font data: [0] = uppercase, [1] = lowercase. Each contains Height rows, each row contains 26 columns (A..Z or a..z).
        private static readonly string[][][] Font =
        {
            new string[][]
            {
                new string[] { "  ##  ", "######", "  #### ", "#####  ", "#######", "########", "  #### ", "##   ##", " #### ", "  ####", " ###  ##", "####   ", "##   ##", "##   ##", " ##### ", " ######", " ##### ", " ######", " ##### ", "######", "##   ##", "##   ##", "##   ##", "##  ##", "##  ##", "#######" },
                new string[] { " #### ", "##  ##", " ##  ##", " ## ## ", " ##   #", " ##     ", " ##  ##", "##   ##", "  ##  ", "    ##", " ##  ## ", " ##    ", "### ###", "###  ##", "##   ##", " ##  ##", "##   ##", " ##  ##", "##   ##", "# ## #", "##   ##", "##   ##", "##   ##", "##  ##", "##  ##", "#   ## " },
                new string[] { "##  ##", "##  ##", "##     ", " ##  ##", " ## #  ", " ##     ", "##     ", "##   ##", "  ##  ", "    ##", " ## ##  ", " ##    ", "#######", "#### ##", "##   ##", " ##  ##", "##   ##", " ##  ##", "#      ", "  ##  ", "##   ##", " ## ## ", "##   ##", " #### ", "##  ##", "   ##  " },
                new string[] { "##  ##", "##### ", "##     ", " ##  ##", " ####  ", " ###### ", "##     ", "#######", "  ##  ", "    ##", " ####   ", " ##    ", "#######", "## ####", "##   ##", " ##### ", "##   ##", " ##### ", " ##### ", "  ##  ", "##   ##", " ## ## ", "## # ##", "  ##  ", " #### ", "  ##   " },
                new string[] { "######", "##  ##", "##     ", " ##  ##", " ## #  ", " ##     ", "##  ###", "##   ##", "  ##  ", "##  ##", " ## ##  ", " ##   #", "## # ##", "##  ###", "##   ##", " ##    ", "##  ###", " ## ## ", "     ##", "  ##  ", "##   ##", "  ###  ", "#######", " #### ", "  ##  ", " ##    " },
                new string[] { "##  ##", "##  ##", " ##  ##", " ## ## ", " ##   #", " ##     ", " ##  ##", "##   ##", "  ##  ", "##  ##", " ##  ## ", " ##  ##", "##   ##", "##   ##", "##   ##", " ##    ", " ##### ", " ##  ##", "##   ##", "  ##  ", "##   ##", "  ###  ", "### ###", "##  ##", "  ##  ", "##    #" },
                new string[] { "##  ##", "######", "  #### ", "#####  ", "#######", "###     ", "  #####", "##   ##", " #### ", " #### ", "###  ## ", "#######", "##   ##", "##   ##", " ##### ", "####   ", "   ####", "#### ##", " ##### ", " #### ", " ##### ", "   #   ", "##   ##", "##  ##", " #### ", "#######" },
                new string[] { "      ", "      ", "       ", "       ", "       ", "        ", "       ", "       ", "      ", "       ","        ", "       ", "       ", "       ", "       ", "       ", "       ", "       ", "       ", "      ", "       ", "       ", "       ", "      ", "      ", "       " }
            },
            new string[][]
            {
                new string[] { "      ", "###    ", "      ", "   ### ", "      ", "  ### ", "       ", "###    ", "  ##  ", "    ##", "###    ", "### ", "       ", "      ", "      ", "       ", "       ", "       ", "       ", " ##   ", "       ", "      ", "       ", "      ", "      ", "      "},
                new string[] { "      ", " ##    ", "      ", "    ## ", "      ", " ## ##", "       ", " ##    ", "      ", "      ", " ##    ", " ## ", "       ", "      ", "      ", "       ", "       ", "       ", "       ", " ##   ", "       ", "      ", "       ", "      ", "      ", "      "},
                new string[] { " #### ", " ##    ", " #### ", "    ## ", " #### ", "  #   ", " ### ##", " ##    ", " ###  ", "   ###", " ##  ##", " ## ", "##  ## ", "##### ", " #### ", "###### ", " ######", "###### ", " ##### ", "##### ", "##  ## ", "##  ##", "##   ##", "##  ##", "##  ##", "######"},
                new string[] { "    ##", " ##### ", "##  ##", " ##### ", "##  ##", "####  ", "##  ## ", " ##### ", "  ##  ", "    ##", " ## ## ", " ## ", "#######", "##  ##", "##  ##", " ##  ##", "##  ## ", " ##  ##", "##     ", " ##   ", "##  ## ", "##  ##", "## # ##", " #### ", "##  ##", "#  ## "},
                new string[] { " #####", " ##  ##", "##    ", "##  ## ", "######", " ##   ", "##  ## ", " ##  ##", "  ##  ", "    ##", " ####  ", " ## ", "## # ##", "##  ##", "##  ##", " ##  ##", "##  ## ", " ##    ", " ##### ", " ##   ", "##  ## ", "##  ##", "#######", "  ##  ", "##  ##", "  ##  "},
                new string[] { "##  ##", " ##  ##", "##  ##", "##  ## ", "##    ", " ##   ", " ##### ", " ##  ##", "  ##  ", "##  ##", " ## ## ", " ## ", "##   ##", "##  ##", "##  ##", " ##### ", " ##### ", " ##    ", "     ##", " ## ##", "##  ## ", " #### ", "#######", " #### ", " #####", " ##  #"},
                new string[] { " #####", "###### ", " #### ", " ######", " #####", "####  ", "    ## ", "###  ##", " #### ", "##  ##", " ##  ##", "####", "##   ##", "##  ##", " #### ", " ##    ", "    ## ", "####   ", "###### ", "  ### ", "###### ", "  ##  ", " ## ## ", "##  ##", "    ##", "######"},
                new string[] { "      ", "       ", "      ", "       ", "      ", "      ", "#####  ", "       ", "      ", " #### ", "       ", "    ", "       ", "      ", "      ", "####   ", "   ####", "       ", "       ", "      ", "       ", "      ", "  # #  ", "      ", "##### ", "      "}
            }
        };

        /// <summary>
        /// Verilen metni ASCII sanat formatında render eder.
        /// </summary>
        /// <param name="text">Render edilecek metin (null olamaz).</param>
        /// <param name="fillChar">Deseni doldurmak için kullanılacak karakter (varsayılan '#').</param>
        /// <returns>Satır satır ASCII banner.</returns>
        /// <exception cref="ArgumentNullException">text null ise fırlatılır.</exception>
        public static string[] Render(string text, char fillChar = '#')
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            // Türkçe karakterleri dönüştür
            text = MapTurkishCharacters(text);

            // Geçerli karakterleri filtrele: A-Z, a-z, boşluk
            var filtered = new string(text.Where(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ').ToArray());

            if (filtered.Length == 0)
                return Array.Empty<string>();

            var builders = new StringBuilder[Height];
            for (int i = 0; i < Height; i++)
                builders[i] = new StringBuilder();

            foreach (char c in filtered)
            {
                if (c == ' ')
                {
                    for (int row = 0; row < Height; row++)
                        builders[row].Append(' ', SpaceWidth);
                    continue;
                }

                if (char.IsUpper(c))
                    AppendCharacter(builders, Font[0], c - 'A', fillChar);
                else if (char.IsLower(c))
                    AppendCharacter(builders, Font[1], c - 'a', fillChar);
            }

            var result = new string[Height];
            for (int i = 0; i < Height; i++)
                result[i] = builders[i].ToString();

            return result;
        }

        private static string MapTurkishCharacters(string text)
        {
            // Hızlı yol: eğer metinde Türkçe karakter yoksa direkt döndür
            bool contains = false;
            foreach (char c in text)
            {
                if (TurkishMap.ContainsKey(c))
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
                return text;

            var sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (TurkishMap.TryGetValue(c, out var mapped))
                    sb.Append(mapped);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private static void AppendCharacter(StringBuilder[] builders, string[][] fontPerCase, int index, char fillChar)
        {
            if (index < 0 || index >= 26)
                return;

            for (int row = 0; row < Height; row++)
            {
                // pattern içindeki '#' karakterlerini anlık olarak doldur
                string pattern = fontPerCase[row][index];
                if (pattern.IndexOf('#') >= 0)
                    builders[row].Append(pattern.Replace('#', fillChar));
                else
                    builders[row].Append(pattern);

                // Harfler arasında bir boşluk
                builders[row].Append(' ');
            }
        }
    }
}