using System.Data;
using System.Text.RegularExpressions;

namespace CompilerProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;

            // 2. Define search patterns for the Tiny Language
            string comments = @"(/\*.*?\*/)";
            string strings = @"("".*?"")";
            string keywords = @"\b(int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|main)\b";
            string numbers = @"\b\d+(\.\d+)?\b";
            string identifiers = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            string operators = @"(:=)|(<>|<|>|=)|(&&|\|\|)|([+\-*/])";
            string symbols = @"([;(),{}])";

            // Combining patterns into one master pattern to catch stuck-together tokens
            string masterPattern = $"{comments}|{strings}|{keywords}|{numbers}|{identifiers}|{operators}|{symbols}";

            // 3. Prepare the Lexeme Table
            DataTable dt = new DataTable();
            dt.Columns.Add("Lexeme");
            dt.Columns.Add("Token Type");

            // 4. Extract all matches using the Regex library
            MatchCollection matches = Regex.Matches(inputString, masterPattern);

            // 5. Classification loop to name each caught Lexeme
            foreach (Match m in matches)
            {
                string lex = m.Value;
                string type = "Unknown";

                if (Regex.IsMatch(lex, "^" + keywords + "$"))
                    type = "Keyword";
                else if (Regex.IsMatch(lex, "^" + numbers + "$"))
                    type = "Number";
                else if (Regex.IsMatch(lex, "^" + identifiers + "$"))
                    type = "Identifier";
                else if (Regex.IsMatch(lex, "^" + strings + "$"))
                    type = "String";
                else if (Regex.IsMatch(lex, "^" + comments + "$"))
                    type = "Comment_Statement";
                else if (lex == ":=")
                    type = "Assignment_Statement";
                else if (lex == "+" || lex == "-" || lex == "*" || lex == "/")
                    type = "Arithmatic_Operator";
                else if (lex == "<" || lex == ">" || lex == "=" || lex == "<>")
                    type = "Condition_Operator";
                else if (lex == "&&" || lex == "||")
                    type = "Boolean_Operator";
                else if (Regex.IsMatch(lex, "^" + symbols + "$"))
                    type = "Symbol";

                dt.Rows.Add(lex, type);
            }

            // 6. Bind results to DataGridView
            dataGridView1.DataSource = dt;

            // 7. Show total token count in a popup (MessageBox) 
            MessageBox.Show("Total Lexemes Found: " + matches.Count.ToString());
        }
    }
}
