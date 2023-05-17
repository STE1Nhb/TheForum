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
        private byte NotificationType { get; set; }
        public EventHandler? OnNewContent = null;
        public User(string name, Forum forumName)
        {
            UserName = name;
            Forum = forumName;
            NotificationType = 1;
            OnNewContent = OnNewContentDefault;
        }

        public void OnNewContentDefault(/*string userName,int contentId, string contentType*/object? sender, EventArgs args)
        {
            Forum.ForumEventArgs fArgs = (Forum.ForumEventArgs)args;
            switch(NotificationType)
            { 
                case 1:
                    if (fArgs.ContentType == "question" && fArgs.UserName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(fArgs.ContentId);

                        Console.WriteLine($" - New notification from \"{Forum.ForumName}!\"");
                        Console.WriteLine($"Dear {UserName}! There is some new content on the forum:\n");
                        if (Convert.ToInt32(fArgs.ContentId) < 10)
                            Console.WriteLine($" - {fArgs.UserName} asked:\n [0{fArgs.ContentId}] {question}\n");
                        else
                            Console.WriteLine($" - {fArgs.UserName} asked:\n [{fArgs.ContentId}] {question}\n");
                    }
                    else if(fArgs.ContentType == "answer" && fArgs.UserName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(fArgs.ContentId);
                        string[] reply = Forum.GetOnlyAnswer(fArgs.ContentId);

                        if (Forum.GetUserInfo(fArgs.ContentId) != UserName)
                        {
                            Console.WriteLine($" - New notification from \"{Forum.ForumName}!\"");
                            Console.WriteLine($"Dear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(fArgs.ContentId) < 10)
                                Console.WriteLine($" - {fArgs.UserName} replied on question [0{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]}\n");
                            else
                                Console.WriteLine($" - {fArgs.UserName} replied on question [{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]}\n");
                        }
                        else 
                        {
                            Console.WriteLine($" - New notification from \"{Forum.ForumName}!\"");
                            Console.WriteLine($"Dear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(fArgs.ContentId) < 10)
                                Console.WriteLine($" - {fArgs.UserName} replied on your question [0{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]}\n");
                            else
                                Console.WriteLine($" - {fArgs.UserName} replied on your question [{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]}\n");
                        }
                    }
                    break;
                case 2:
                    if (fArgs.ContentType == "answer" && fArgs.UserName != UserName)
                    {
                        string question = Forum.GetOnlyQuestion(fArgs.ContentId);
                        string[] reply = Forum.GetOnlyAnswer(fArgs.ContentId);

                        if (Forum.GetUserInfo(fArgs.ContentId) == UserName)
                        {
                            Console.WriteLine($" - New notification from \"{Forum.ForumName}!\"");
                            Console.WriteLine($"Dear {UserName}! There is some new content on the forum:\n");
                            if (Convert.ToInt32(fArgs.ContentId) < 10 && Convert.ToInt32(reply[0]) < 10)
                                Console.WriteLine($" - {fArgs.UserName} replied on your question [0{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]} \n");
                            else
                                Console.WriteLine($" - {fArgs.UserName} replied on your question [{fArgs.ContentId}] {question}:\n [{reply[0]}] {reply[1]} \n");
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
            if(type.Trim().ToLower() == "all" || type == "1") // all notifications
            {
                NotificationType = 1;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you'll recieve notifications about any change on the forum.\n");
            }
            else if(type.Trim().ToLower() == "personal" || type == "2") // only personal changes (new answers on own questions)
            {
                NotificationType = 2;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you'll recieve notifications only about replies on your questions.\n");
            }
            else if(type.Trim().ToLower() == "none" || type == "3") // no notifications anymore :(
            {
                NotificationType = 3;
                Console.WriteLine("Notification type has changed successfully!\nFrom this moment you won't recieve any notifications.\n");
            }
            else // Help in case of problems
            {
                NotificationType = 1;
                Console.WriteLine("Our forum doesn't support such type of notification! The notification type was changed to default (all)\n");
                Console.WriteLine("Handled types are:\n - 1 or all - if you want to reciece all the notifications.");
                Console.WriteLine(" - 2 or personal - if you want to revieve information only about your questions.");
                Console.WriteLine(" - 3 or none - if you want to stand without notifications for some reason :(");
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

        public void GetQuestion(int qId)
        {
            Console.WriteLine($"Dear {this.UserName}! Here is your information:");
            Forum.GetQuestion(qId);
        }
        public void ForumQuestions()
        {
            Console.WriteLine($"Dear {this.UserName}! Here is your information:");
            Forum.ForumQuestions();
        }


    }
}
