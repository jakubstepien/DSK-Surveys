using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.ViewModels
{
    public class AddSurveyViewModel : BaseViewModel
    {
        private int hour = 0;

        public int Hour
        {
            get { return hour; }
            set { hour = value; NotifyPropChanged(); }
        }

        private int minute = 0;

        public int Minute
        {
            get { return minute; }
            set { minute = value; NotifyPropChanged(); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropChanged(); }
        }


        public ObservableCollection<QuestionViewModel> Items { get; set; } = new ObservableCollection<QuestionViewModel>();

    }

    public class QuestionViewModel : BaseViewModel, IDataErrorInfo
    {
        private string text;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Text" && string.IsNullOrWhiteSpace(Text))
                {
                    return "Pole nie może być puste";
                }
                return null;
            }
        }

        public string Error => null;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

    }
}
