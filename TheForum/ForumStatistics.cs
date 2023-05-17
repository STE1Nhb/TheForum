using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TheForum
{
    public class ForumStatistics
    {
        private Forum Forum { get; }
        int[] QRAmount = new int[3];
        public EventHandler? OnNewContent = null;
        public int QAmount { get; private set; } // Amount of questions
        public int RAmount { get; private set; } // Amount of replies
        public int RAVG { get; private set; } // Average amount of replies
        public int RNone { get; private set; } // Amount of questions without replies                                    
        public int RMinOne { get; private set; } // Amount of questions with at least one reply

        public ForumStatistics(Forum forum)
        {
            Forum = forum;

            QRAmount = GetQuetionAnswerAmount();
            QAmount = QRAmount[0];
            RAmount = QRAmount[1];
            RAVG = RAmount / (QAmount == 0 ? 1 : QAmount);
            RNone = QRAmount[2];
            RMinOne = QAmount - RNone;
            OnNewContent = OnNewContentDefault;
            Forum.UpdateStats += OnNewContent;
        }

        public void OnNewContentDefault(object? sender, EventArgs args)
        {
            Forum.ForumEventArgs fArgs = (Forum.ForumEventArgs)args;
            if (fArgs.QRAmount[0] != QAmount)
                QAmount = fArgs.QRAmount[0];
            if (fArgs.QRAmount[1] != RAmount) 
                RAmount = fArgs.QRAmount[1];
            if (RAVG != fArgs.QRAmount[1] / (fArgs.QRAmount[0] == 0 ? 1 : fArgs.QRAmount[0]))
                RAVG = fArgs.QRAmount[1] / fArgs.QRAmount[0];
            if(RNone != fArgs.QRAmount[2])
                RNone = fArgs.QRAmount[2];
            if (RMinOne != fArgs.QRAmount[0] - fArgs.QRAmount[2])
                RMinOne = fArgs.QRAmount[0] - fArgs.QRAmount[2];
        }

        private int[] GetQuetionAnswerAmount()
        {
            return Forum.GetQuestionReplyAmount();
        }
    }
}
