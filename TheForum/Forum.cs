using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TheForum
{
    public delegate void ForumChangesHandler(string userName, int contentId, string contentType);
    public class Forum
    {
        private string forumName;
        public ForumStatistics Statistics { get; }
        private List<Question> questions = new List<Question>();
        public ForumChangesHandler NewContent = null;

        public Forum(string forumName)
        {
            this.forumName = forumName;
            Statistics = new ForumStatistics(this);
        }

        public void CreateQuestion(string question, User user)
        {
            qHistory.Add(DateTime.Now, question);
            questions.Add(new Question(questions.Count + 1, user));
            string userName = questions.Last().UserInfo.UserName;
            int questionId = questions.Last().QId;
            NewContent?.Invoke(userName, questionId, "question");
        }
        public void AnswerQuestion(int qId, string answer, User user)
        {
            if (qId > 0)
            {
               questions[qId - 1].AddAnswer(answer, user);
            }
            string replierName = questions[qId - 1].repliers.Last().UserName;
            int replyId = questions[qId - 1].Answers.Count;
            NewContent?.Invoke(replierName, qId, "answer");
        }

        public void GetQuestion(int qId)
        {
            var question = questions[qId - 1];
            var userName = question.UserInfo.UserName;
            int aId = 0;
            if(qId < 10)
                Console.WriteLine($"\nQuestion (ID [0{question.QId}]) {qHistory.ElementAt(qId - 1)} - asked by {userName}");
            else
                Console.WriteLine($"\nQuestion (ID [{question.QId}]) {qHistory.ElementAt(qId - 1)} - asked by {userName}");
            Console.WriteLine("  Replies:\n");
            foreach (var answer in question.Answers)
            {
                if(aId < 10)
                    Console.WriteLine($" - {question.repliers[aId].UserName} replied: {answer} - ID [0{aId + 1}]");
                else
                    Console.WriteLine($" - {question.repliers[aId].UserName} replied: {answer} - ID [{aId + 1}]");
                aId++;
            }
        }
        public string GetOnlyQuestion(int qId) // Need only for User event handler 
        {
            var question = questions[qId - 1];
            return Convert.ToString(qHistory.ElementAt(qId - 1))!;
        }
        public string[] GetOnlyAnswer(int qId) // Need only for User event handler 
        {
            var question = questions[qId - 1];
            var answer = new string[2];
            if (question.Answers.Count > 10)
            {
                answer[0] = $"{question.Answers.Count}"; 
                answer[1] = $"{question.Answers.Last()}";
            }
            else
            {
                answer[0] = $"0{question.Answers.Count}";
                answer[1] = $"{question.Answers.Last()}";
            }

            return answer;
        }
        public string GetUserInfo(int qId) // Need only for User event handler 
        {
            var question = questions[qId - 1];
            return question.UserInfo.UserName;
        }
        public int[] GetQuestionReplyAmount() // Returns necessery values for ForumStatistics class
        {
            var qAmount = 0;
            var aAmount = 0;
            var aNone = 0;
            try
            {
                qAmount = qHistory.Count;
            }
            catch (InvalidOperationException) 
            {
                qAmount = 0;
            }

            foreach (var question in questions)
            {
                try
                {
                    aAmount += question.Answers.Count();
                }
                catch(InvalidOperationException) 
                {
                    aAmount+= 0;
                    aNone++;
                }
            }
            var result = new int[3] { qAmount, aAmount, aNone };
            return result;
        }
        public void QuestionAnswers(int qId) // Maybe useless
        {
            if (qId > 0)
            {
                int aId = 1;
                Console.WriteLine($"\nAnswers on question \"{qHistory.ElementAt(qId - 1)}\":\n");
                foreach (var aHist in questions[qId - 1].Answers)
                {
                    if (aId < 10)
                        Console.WriteLine($" - {aHist} - ID [0{aId}]");
                    else
                        Console.WriteLine($" - {aHist} - ID [{aId}]");
                    aId++;
                }
            }
        }
        public void ForumQuestions()
        {
            int id = 0;
            Console.WriteLine($"\nForum history:\n");
            foreach (var qHist in qHistory)
            {
                Console.WriteLine($" - {questions[id].UserInfo.UserName} asked: {qHist} - ID [{questions[id].QId}]");
                id++;
            }
        }
        //private string answer;
        
        
        //private List<Forum> forumQuestions;
        //public string Answer 
        //{
        //    get => answer;
        //    set
        //    {
        //        answer = value;
        //        answers.Add(DateTime.Now, answer);
        //    }
        //}

        

        private IDictionary<DateTime, string> qHistory = new Dictionary<DateTime, string>();

        //public IReadOnlyDictionary<DateTime, string> QuestionHistory
        //        => (IReadOnlyDictionary<DateTime, string>)aHistory;
        //public IReadOnlyDictionary<DateTime, string> ForumHistory
        //       => (IReadOnlyDictionary<DateTime, string>)qHistory;


        //public void ForumQuestions()
        //{
        //    Console.WriteLine(string.Join(" ", forumQuestions));
        //}
        private class Question // Special class for questions and its answers
        {
            public int QId { get; }
            private List<string> answers = new List<string>();
            public List<User> repliers = new List<User>();
            public User UserInfo { get; }
            public Question(int id, User user)
            {
                QId = id;
                UserInfo = user;
            }
            public void AddAnswer(string answer, User user)
            {
                answers.Add(answer);
                qAnswers.Add(DateTime.Now, answer);
                repliers.Add(user);
            }
            
            private IDictionary<DateTime, string> qAnswers = new Dictionary<DateTime, string>();

            public IReadOnlyDictionary<DateTime, string> Answers => (IReadOnlyDictionary<DateTime, string>)qAnswers;
        }
    }
}
