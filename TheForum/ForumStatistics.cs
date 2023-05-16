using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForum
{
    public class ForumStatistics : INotifyPropertyChanged
    {
        private Forum Forum { get; }
        int[] QRAmount = new int[3];

        public event PropertyChangedEventHandler? PropertyChanged;
        public int QAmount { get; } // Amount of questions
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
        }

        private int[] GetQuetionAnswerAmount()
        {
            return Forum.GetQuestionReplyAmount();
        }
    }
}
