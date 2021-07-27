using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class QuestionBankBean
    {
        public string question_id { get; set; }
        public string answer { get; set; }
        public string level_id { get; set; }
        public string question_text { get; set; }
        public string question_text_series { get; set; }
        public List<string> NumberList { get; set; }
        public string operator_series { get; set; }
        public string topic_id { get; set; }
        public void AddAnswer(string ans)
        {
            if (NumberList == null)
                NumberList = new List<string>();
            NumberList.Add(ans);
        }
        public string GetAnswer()
        {
            if (NumberList != null)
            {
                float fanser = 0;
                float myanswer = 0;
                foreach (string a in NumberList)
                {
                    float.TryParse(a, out fanser);
                    myanswer += fanser;
                }
                answer = myanswer.ToString();
            }
            return answer;
        }
        public string GetQuestionText()
        {
            if (NumberList != null)
            {
                float fanser = 0;
                question_text = "";
                foreach (string a in NumberList)
                {
                    float.TryParse(a, out fanser);
                    if (question_text.Length == 0)
                        question_text = a;
                    else
                        question_text += "," + a;
                }
            }
            return question_text;
        }

        public string GetQuestionSeries()
        {
            if (NumberList != null)
            {
                float fanser = 0;
                question_text = "";
                foreach (string a in NumberList)
                {
                    float.TryParse(a, out fanser);
                    if (question_text.Length == 0)
                        question_text = a;
                    else
                        question_text += "," + a;
                }
            }
            return question_text;
        }
        public string GetOperatorSeries()
        {
            string operatorSeries = "";
            if (NumberList != null)
            {
                List<float> list = new List<float>();
                string[] operators = { "+", "-", "*", "%" };
                foreach (string a in NumberList)
                {
                    int oprCount = 0;
                    for (int ol = 0; ol < operators.Length; ol++)
                    {
                        if (a.Contains(operators[ol]))
                        {
                            if (oprCount > 0)
                            {
                                if (operatorSeries.Length > 0)
                                    operatorSeries = operatorSeries + ","+operators[ol];
                                else
                                    operatorSeries = operators[ol];
                            }
                            else
                            {
                                if (operatorSeries.Length > 0)
                                    operatorSeries = operatorSeries + "," + operators[ol];
                                else
                                    operatorSeries = operators[ol];
                            }
                            oprCount++;
                        }

                    }
                    if (oprCount == 0)
                    {
                        if (operatorSeries.Length > 0)
                            operatorSeries = operatorSeries + ",+";
                        else
                            operatorSeries = "+";
                    }
                }
            }
            return operatorSeries;
        }
    }
}
