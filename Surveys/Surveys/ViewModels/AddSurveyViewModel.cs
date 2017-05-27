using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.ViewModels
{
    public class AddSurveyViewModel
    {
        public ObservableCollection<QuestionViewModel> Items { get; set; } = new ObservableCollection<QuestionViewModel>();
    }

    public class QuestionViewModel : BaseViewModel
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

    }
}
