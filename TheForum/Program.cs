﻿namespace TheForum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Forum x = new Forum("попиздушки");
            User y = new User("kozel33", x);
            User z = new User("ebar'mam", x);


            x.NewContent += y.OnNewContent;

            y.CreateQuestion("Хто я нахуй?");
            y.ChangeNotificationType("2");
            y.CreateQuestion("А хто ты нахуй?");
            z.CreateQuestion("Как рулить?");


            y.AnswerQuestion(3, "Козел ты");
            z.AnswerQuestion(3, "рулем!!!!");
            z.AnswerQuestion(1, "Козелииина");
            z.AnswerQuestion(2, "Однозначно козлина нафиг");

            //x.ForumQuestions();

            //x.GetQuestion(1);
            //x.GetQuestion(2);
           



        }
    }
}