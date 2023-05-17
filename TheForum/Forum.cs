using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TheForum
{
    //public delegate void ForumChangesHandler(string userName, int contentId, string contentType);
    public class Forum
    {
        public string ForumName { get; }
        public ForumStatistics Statistics { get; private set; }
        private List<Question> questions = new List<Question>();
        public EventHandler? NewContent = null;
        public EventHandler? UpdateStats = null;

        public Forum(string forumName)
        {
            ForumName = forumName;
            Statistics = new ForumStatistics(this);
        }

        public void CreateQuestion(string question, User user)
        {
            qHistory.Add(DateTime.Now, question);
            questions.Add(new Question(questions.Count + 1, user));
            string userName = questions.Last().UserInfo.UserName;
            int questionId = questions.Last().QId;
            NewContent?.Invoke(this, new ForumEventArgs(userName, questionId, "question"));
            UpdateStats?.Invoke(this, new ForumEventArgs(GetQuestionReplyAmount()));
        }
        public void AnswerQuestion(int qId, string answer, User user)
        {
            if (qId > 0)
            {
               questions[qId - 1].AddAnswer(answer, user);
            }
            string replierName = questions[qId - 1].repliers.Last().UserName;
            int replyId = questions[qId - 1].Answers.Count;
            NewContent?.Invoke(this, new ForumEventArgs(replierName, qId, "answer"));
            UpdateStats?.Invoke(this, new ForumEventArgs(GetQuestionReplyAmount()));
        }

        public void GetQuestion(int qId)
        {
            var question = questions[qId - 1];
            var userName = question.UserInfo.UserName;
            int aId = 0;
            if(qId < 10)
                Console.WriteLine($"Question (ID [0{question.QId}]) {qHistory.ElementAt(qId - 1)} - asked by {userName}");
            else
                Console.WriteLine($"Question (ID [{question.QId}]) {qHistory.ElementAt(qId - 1)} - asked by {userName}");
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
            var qAmount = qHistory.Count;
            var aAmount = 0;
            var aNone = 0;

            foreach (var question in questions)
            {
                try
                {
                    aAmount += question.Answers.Count;
                    if(question.Answers.Count == 0)
                        aNone++;
                }
                catch(InvalidOperationException)
                {
                    aAmount += 0;
                    if (question.Answers.Count == 0)
                        aNone++;
                }
            }

            var result = new int[3] { qAmount, aAmount, aNone };
            return result;
        }

        public void ForumQuestions() // List of forum questions
        {
            int id = 0;
            Console.WriteLine($"Forum history:\n");
            foreach (var qHist in qHistory)
            {
                if(id < 10)
                    Console.WriteLine($" - {questions[id].UserInfo.UserName} asked: {qHist} - ID [0{questions[id].QId}]");
                else
                    Console.WriteLine($" - {questions[id].UserInfo.UserName} asked: {qHist} - ID [{questions[id].QId}]");
                id++;
            }
        }

        private IDictionary<DateTime, string> qHistory = new Dictionary<DateTime, string>();

        public class ForumEventArgs : EventArgs
        {
            public string UserName { get; }
            public int ContentId { get; }
            public string ContentType { get; }

            public int[] QRAmount { get; }
            public ForumEventArgs(string userName, int contentId, string contentType)
            {
                UserName = userName;
                ContentId = contentId;
                ContentType = contentType;
            }
            public ForumEventArgs(int[] qRAmount)
            {
                QRAmount= qRAmount;
            }
        }
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
