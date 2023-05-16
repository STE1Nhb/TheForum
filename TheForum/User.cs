using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForum
{
    public class User
    {
        public string UserName { get; private set; }
        private Forum Forum { get; }
        private int NotificationType { get; set; }
        public ForumChangesHandler OnNewContent = null;
        public User(string name, Forum forumName)
        {
            UserName = name;
            Forum = forumName;
            NotificationType = 1;
            OnNewContent = this.OnNewContentDefault;
        }

        public virtual void OnNewContentDefault(string userName,int contentId, string contentType)
        {
            switch(NotificationType)
            { 
                case 1:
                    if (contentType == "question" && userName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(contentId);

                        Console.WriteLine($"\nDear {UserName}! There is some new content on the forum:\n");
                        if (Convert.ToInt32(contentId) < 10)
                            Console.WriteLine($" - {userName} asked:\n [0{contentId}] {question}");
                        else
                            Console.WriteLine($" - {userName} asked:\n [{contentId}] {question}");
                    }
                    else if(contentType == "answer" && userName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(contentId);
                        string[] reply = Forum.GetOnlyAnswer(contentId);

                        if (Forum.GetUserInfo(contentId) != UserName)
                        {
                            Console.WriteLine($"\nDear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(contentId) < 10)
                                Console.WriteLine($" - {userName} replied on question [0{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                            else
                                Console.WriteLine($" - {userName} replied on question [{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                        }
                        else 
                        {
                            Console.WriteLine($"\nDear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(contentId) < 10)
                                Console.WriteLine($" - {userName} replied on your question [0{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                            else
                                Console.WriteLine($" - {userName} replied on your question [{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                        }
                    }
                    break;
                case 2:
                    if (contentType == "answer" && userName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(contentId);
                        string[] reply = Forum.GetOnlyAnswer(contentId);

                        if (Forum.GetUserInfo(contentId) == UserName)
                        {
                            Console.WriteLine($"\nDear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(contentId) < 10 && Convert.ToInt32(reply[0]) < 10)
                                Console.WriteLine($" - {userName} replied on your question [0{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                            else
                                Console.WriteLine($" - {userName} replied on your question [{contentId}] {question}:\n [{reply[0]}] {reply[1]}");
                        }
                    }
                    break;
            }
        }
        public void CreateQuestion(string question)
        {
            Forum.CreateQuestion(question, this);
        }
        public void AnswerQuestion(int qId, string answer)
        {
            Forum.AnswerQuestion(qId, answer, this);
        }
        public void ChangeNotificationType(string type)
        {
            if(type.Trim().ToLower() == "all" || type == "1")
            {
                NotificationType = 1;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you'll recieve notifications about any change on the forum.");
            }
            else if(type.Trim().ToLower() == "personal" || type == "2") 
            {
                NotificationType = 2;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you'll recieve notifications only about replies on your questions.");
            }
            else if(type.Trim().ToLower() == "none" || type == "3")
            {
                NotificationType = 3;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you won't recieve any notifications.");
            }
            else
            {
                NotificationType = 1;
                Console.WriteLine("Our forum doesn't support such type of notification! The notification type was changed to default (all)");
            }
        }
        public void GetNotificationType() 
        {
            if(NotificationType == 1)
            {
                Console.WriteLine("You recieve notifications about all changes on the forum (new questions and its answers).");
            }
            else if(NotificationType == 2)
            {
                Console.WriteLine("You recieve notifications only about replies on your questions.");
            }
            else
            {
                Console.WriteLine("You don't recieve any notifications from the forum");
            }
        }

    }
}
